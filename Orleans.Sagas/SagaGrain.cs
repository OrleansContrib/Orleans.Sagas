using Microsoft.Extensions.Logging;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orleans.Sagas
{
    public sealed class SagaGrain : Grain<SagaState>, ISagaGrain
    {
        private static readonly string ReminderName = nameof(SagaGrain);

        private readonly IGrainActivationContext grainContext;
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<SagaGrain> logger;
        private bool isActive;
        private IGrainReminder grainReminder;

        public SagaGrain(IGrainActivationContext grainContext, IServiceProvider serviceProvider, ILogger<SagaGrain> logger)
        {
            this.grainContext = grainContext;
            this.serviceProvider = serviceProvider;
            this.logger = logger;
        }

        protected override async Task ReadStateAsync()
        {
            try
            {
                await base.ReadStateAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(1, ex, "Failed to read state");
                await HandleReadingStateError();
            }
        }

        private async Task HandleReadingStateError()
        {
            try
            {
                await UnRegisterReminderAsync();
                isActive = false;
                await ClearStateAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(1, ex, "Failed to handle reading state error");
            }
        }

        public async Task RequestAbort()
        {
            logger.Warn(0, $"Saga {this} received an abort request.");

            // register abort request in separate grain in-case storage is mutating.
            await GetSagaCancellationGrain().RequestAbort();

            await ResumeAsync();
        }

        public async Task Execute(IEnumerable<ActivityDefinition> activities, ISagaPropertyBag sagaProperties)
        {
            if (State.Status == SagaStatus.NotStarted)
            {
                State.Activities = activities.ToList();
                State.Properties = sagaProperties is null
                    ? new Dictionary<string, string>()
                    : ((SagaPropertyBag)sagaProperties).ContextProperties;
                State.Status = SagaStatus.Executing;
                await WriteStateAsync();
                await RegisterReminderAsync();
            }

            await ResumeAsync();
        }

        public Task<SagaStatus> GetStatus()
        {
            return Task.FromResult(State.Status);
        }

        public Task<IReadOnlyDictionary<Type, SagaError>> GetSagaErrors() => throw new NotImplementedException();

        public Task<bool> HasCompleted()
        {
            return Task.FromResult(
                State.Status == SagaStatus.Aborted ||
                State.Status == SagaStatus.Compensated ||
                State.Status == SagaStatus.Executed
            );
        }

        public async Task ReceiveReminder(string reminderName, TickStatus status)
        {
            await ResumeAsync();
        }

        public Task ResumeAsync()
        {
            if (!isActive)
            {
                ResumeNoWaitAsync().Ignore();
            }
            return Task.CompletedTask;
        }

        public override string ToString()
        {
            return this.GetPrimaryKey().ToString();
        }

        private ISagaCancellationGrain GetSagaCancellationGrain()
        {
            return GrainFactory.GetGrain<ISagaCancellationGrain>(this.GetPrimaryKey());
        }

        private async Task RegisterReminderAsync()
        {
            var reminderTime = TimeSpan.FromMinutes(1);
            grainReminder = await RegisterOrUpdateReminder(ReminderName, reminderTime, reminderTime);
        }

        private async Task UnRegisterReminderAsync()
        {
            if (grainReminder == null)
            {
                return;
            }

            try
            {
                await UnregisterReminder(grainReminder);
                grainReminder = null;
            }
            catch (Exception ex)
            {
                logger.LogError(1, ex, "Failed to unregister the reminder");
            }
        }

        private async Task ResumeNoWaitAsync()
        {
            isActive = true;

            try
            {
                if (State.NumCompletedActivities > 0)
                {
                    await CheckForAbortAsync();
                }

                while (State.Status == SagaStatus.Executing ||
                       State.Status == SagaStatus.Compensating)
                {
                    switch (State.Status)
                    {
                        case SagaStatus.Executing:
                            await ResumeExecuting();
                            break;
                        case SagaStatus.Compensating:
                            await ResumeCompensating();
                            break;
                    }
                }

                switch (State.Status)
                {
                    case SagaStatus.NotStarted:
                        ResumeNotStarted();
                        break;
                    case SagaStatus.Executed:
                    case SagaStatus.Compensated:
                    case SagaStatus.Aborted:
                        ResumeCompleted();
                        break;
                }

                await UnRegisterReminderAsync();
            }
            finally
            {
                isActive = false;
            }
        }

        private void ResumeNotStarted()
        {
            logger.Error(0, $"Saga {this} is attempting to resume but was never started.");
        }

        private IActivity GetActivity(ActivityDefinition definition)
        {
            return (IActivity)serviceProvider.GetService(definition.Type);
        }

        private async Task ResumeExecuting()
        {
            while (State.NumCompletedActivities < State.Activities.Count)
            {
                var definition = State.Activities[State.NumCompletedActivities];
                var currentActivity = GetActivity(definition);

                try
                {
                    logger.Debug($"Executing activity #{State.NumCompletedActivities} '{currentActivity.GetType().Name}'...");
                    var context = CreateActivityRuntimeContext(definition);
                    await currentActivity.Execute(context);
                    logger.Debug($"...activity #{State.NumCompletedActivities} '{currentActivity.GetType().Name}' complete.");
                    State.NumCompletedActivities++;
                    AddPropertiesToState(context);
                    await WriteStateAsync();
                }
                catch (Exception e)
                {
                    logger.Warn(0, "Activity '" + currentActivity.GetType().Name + "' in saga '" + GetType().Name + "' failed with " + e.GetType().Name);
                    State.CompensationIndex = State.NumCompletedActivities;
                    State.Status = SagaStatus.Compensating;
                    AddActivityError(definition, e);
                    await WriteStateAsync();

                    return;
                }

                // To ensure running first activity 
                if (await CheckForAbortAsync())
                {
                    return;
                }
            }

            if (await CheckForAbortAsync())
            {
                return;
            }

            State.Status = SagaStatus.Executed;
            await WriteStateAsync();
        }

        private async Task<bool> CheckForAbortAsync()
        {
            if (await GetSagaCancellationGrain().HasAbortBeenRequested())
            {
                if (!State.HasBeenAborted)
                {
                    State.HasBeenAborted = true;
                    State.Status = SagaStatus.Compensating;
                    State.CompensationIndex = State.NumCompletedActivities - 1;
                    await WriteStateAsync();
                }

                return true;
            }

            return false;
        }

        private async Task ResumeCompensating()
        {
            while (State.CompensationIndex >= 0)
            {
                var definition = State.Activities[State.CompensationIndex];
                try
                {
                    var currentActivity = GetActivity(definition);

                    logger.Debug(0, $"Compensating for activity #{State.CompensationIndex} '{currentActivity.GetType().Name}'...");
                    var context = CreateActivityRuntimeContext(definition);
                    await currentActivity.Compensate(context);
                    logger.Debug(0, $"...activity #{State.CompensationIndex} '{currentActivity.GetType().Name}' compensation complete.");
                    State.CompensationIndex--;
                    await WriteStateAsync();
                }
                catch (Exception e)
                {
                    AddActivityError(definition, e);
                    await Task.Delay(5000);
                    // TODO: handle compensation failure with expoential backoff.
                    // TODO: maybe eventual accept failure in a CompensationFailed state?
                }
            }

            State.Status = State.HasBeenAborted
                ? SagaStatus.Aborted
                : SagaStatus.Compensated;
            await WriteStateAsync();
        }

        private void AddPropertiesToState(ActivityContext context)
        {
            var propertyBag = (SagaPropertyBag)context.SagaProperties;
            foreach (var property in propertyBag.ContextProperties)
            {
                State.Properties.Add(property.Key, property.Value);
            }
        }

        private ActivityContext CreateActivityRuntimeContext(ActivityDefinition definition)
        {
            var propertyBag = (SagaPropertyBag)definition.Properties;
            IEnumerable<KeyValuePair<string, string>> properties = State.Properties;

            if (propertyBag != null)
            {
                properties = properties.Concat(propertyBag.ContextProperties);
            }

            return new ActivityContext(
                this.GetPrimaryKey(),
                GrainFactory,
                grainContext,
                properties.ToDictionary(x => x.Key, y => y.Value)
            );
        }

        private void ResumeCompleted()
        {
            logger.Info($"Saga {this} has completed with status '{State.Status}'.");
        }

        private void AddActivityError(ActivityDefinition activityDefinition, Exception exception)
        {
            // Must be backward compatiable way =
        }
    }
}