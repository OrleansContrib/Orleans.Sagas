using Microsoft.Extensions.Logging;
using Orleans.Concurrency;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Orleans.Sagas
{
    public sealed class SagaGrain : Grain<SagaState>, ISagaGrain
    {
        private static readonly string ReminderName = nameof(SagaGrain);

        private readonly IGrainActivationContext grainContext;
        private readonly ILogger<SagaGrain> logger;
        private bool isActive;

        public SagaGrain(IGrainActivationContext grainContext, ILogger<SagaGrain> logger)
        {
            this.grainContext = grainContext;
            this.logger = logger;
        }

        public async Task RequestAbort()
        {
            logger.Warn(0, $"Saga {this} received an abort request.");

            if (State.Status == SagaStatus.Aborted)
            {
                return;
            }

            State.HasBeenAborted = true;
            State.Status = State.Status == SagaStatus.NotStarted
                ? SagaStatus.Aborted
                : SagaStatus.Compensating;
            
            // register abort request in separate grain in-case storage is mutating.
            await GetSagaCancellationGrain().RequestAbort();

            if (State.Status == SagaStatus.Compensating)
            {
                await Resume();
            }
        }

        public async Task Execute(IEnumerable<IActivity> activities)
        {
            if (State.Status == SagaStatus.NotStarted)
            {
                State.Activities = activities.ToList();
                State.Status = SagaStatus.Executing;
                await WriteStateAsync();
                await RegisterReminder();
            }

            await Resume();
        }

        public Task<SagaStatus> GetStatus()
        {
            return Task.FromResult(State.Status);
        }

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
            await Resume();
        }

        public Task Resume()
        {
            if (!isActive)
            {
                ResumeNoWait().Ignore();
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

        private async Task<IGrainReminder> RegisterReminder()
        {
            var reminderTime = TimeSpan.FromMinutes(1);
            return await RegisterOrUpdateReminder(ReminderName, reminderTime, reminderTime);
        }

        private async Task ResumeNoWait()
        {
            isActive = true;
            
            if (await GetSagaCancellationGrain().HasAbortBeenRequested())
            {
                await RequestAbort();
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

            var reminder = await RegisterReminder();
            await UnregisterReminder(reminder);

            isActive = false;
        }

        private void ResumeNotStarted()
        {
            logger.Error(0, $"Saga {this} is attempting to resume but was never started.");
        }

        private async Task ResumeExecuting()
        {
            while (State.NumCompletedActivities < State.Activities.Count)
            {
                var currentActivity = State.Activities[State.NumCompletedActivities];

                try
                {
                    currentActivity.Initialize(this.GetPrimaryKey(), this.grainContext);
                    logger.Debug($"Executing activity #{State.NumCompletedActivities} '{currentActivity.Name}'...");
                    await currentActivity.Execute();
                    logger.Debug($"...activity #{State.NumCompletedActivities} '{currentActivity.Name}' complete.");
                    State.NumCompletedActivities++;
                    await WriteStateAsync();
                }
                catch (Exception e)
                {
                    logger.Warn(0, "Activity '" + currentActivity.GetType().Name + "' in saga '" + GetType().Name + "' failed with " + e.GetType().Name);
                    State.CompensationIndex = State.NumCompletedActivities;
                    State.Status = SagaStatus.Compensating;
                    await WriteStateAsync();
                    return;
                }
            }

            State.Status = SagaStatus.Executed;
            await WriteStateAsync();
        }

        private async Task ResumeCompensating()
        {
            while (State.CompensationIndex >= 0)
            {
                try
                {
                    var currentActivity = State.Activities[State.CompensationIndex];

                    currentActivity.Initialize(this.GetPrimaryKey(), this.grainContext);
                    logger.Debug(0, $"Compensating for activity #{State.CompensationIndex} '{currentActivity.Name}'...");
                    await currentActivity.Compensate();
                    logger.Debug(0, $"...activity #{State.CompensationIndex} '{currentActivity.Name}' compensation complete.");
                    State.CompensationIndex--;
                    await WriteStateAsync();
                }
                catch (Exception)
                {
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

        private void ResumeCompleted()
        {
            logger.Info($"Saga {this} has completed with status '{State.Status}'.");
        }
    }
}
