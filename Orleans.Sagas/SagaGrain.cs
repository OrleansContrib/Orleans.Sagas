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
        private List<IActivity> activities;
        private bool isResuming;
        private object resumeLock;

        public override Task OnActivateAsync()
        {
            resumeLock = new object();
            return Task.CompletedTask;
        }

        public async Task Abort()
        {
            GetLogger().Warn(0, $"Aborting {GetType().Name} saga.");
            State.Status = SagaStatus.Compensating;
            await WriteStateAsync();
        }

        public async Task Execute(IEnumerable<Tuple<Type, object>> activities)
        {
            if (State.Status == SagaStatus.NotStarted)
            {
                State.Activities = activities;
                InstantiateActivities();
                State.Status = SagaStatus.Executing;
                await WriteStateAsync();
                // TODO: Register reminder here.
            }

            await Resume();
        }

        public Task<SagaStatus> GetStatus()
        {
            return Task.FromResult(State.Status);
        }

        public Task Resume()
        {
            lock (resumeLock)
            {
                if (isResuming)
                {
                    return Task.CompletedTask;
                }

                isResuming = true;
            }

#pragma warning disable CS4014
            ResumeNoWait();
#pragma warning restore CS4014

            lock (resumeLock)
            {
                isResuming = false;
            }

            return Task.CompletedTask;
        }

        public override string ToString()
        {
            return this.GetPrimaryKey().ToString();
        }

        private async Task ResumeNoWait()
        {
            InstantiateActivities();

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
                    ResumeCompleted();
                    break;
            }
        }

        private void InstantiateActivities()
        {
            if (activities != null)
            {
                return;
            }

            activities = new List<IActivity>();
            foreach (var activityDefinition in State.Activities)
            {
                var type = activityDefinition.Item1;
                var config = activityDefinition.Item2;
                var activity = (IActivity)Activator.CreateInstance(type);
                activities.Add(activity);
                if (config != null)
                {
                    var genericType = typeof(IActivity<>).MakeGenericType(config.GetType());
                    var method = genericType.GetTypeInfo().GetMethod("SetConfig");
                    method.Invoke(activity, new object[] { config });
                }
            }
        }

        private void ResumeNotStarted()
        {
            GetLogger().Error(0, $"Saga {this} is attempting to resume but was never started.");
        }

        private async Task ResumeExecuting()
        {
            while (State.NumCompletedActivities < activities.Count)
            {
                var currentActivity = activities[State.NumCompletedActivities];

                try
                {
                    currentActivity.Initialize(GrainFactory, GetLogger());
                    GetLogger().Info($"Executing activity #{State.NumCompletedActivities} '{currentActivity.Name}'...");
                    await currentActivity.Execute();
                    GetLogger().Info($"...activity #{State.NumCompletedActivities} '{currentActivity.Name}' complete.");
                    State.NumCompletedActivities++;
                    await WriteStateAsync();
                }
                catch (Exception e)
                {
                    GetLogger().Error(0, "Activity '" + currentActivity.GetType().Name + "' in saga '" + GetType().Name + "' failed with " + e.GetType().Name);
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
                    var currentActivity = activities[State.CompensationIndex];

                    currentActivity.Initialize(GrainFactory, GetLogger());
                    GetLogger().Warn(0, $"Compensating for activity #{State.CompensationIndex} '{currentActivity.Name}'...");
                    await currentActivity.Compensate();
                    GetLogger().Warn(0, $"...activity #{State.CompensationIndex} '{currentActivity.Name}' compensation complete.");
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

            State.Status = SagaStatus.Compensated;
            await WriteStateAsync();
        }

        private void ResumeCompleted()
        {
            GetLogger().Info($"Saga {this} has completed with status '{State.Status}'.");
        }

        // TODO: Use for reminder resume. Currently un-used.
        private ISagaGrain CreateReferenceToSelf()
        {
            var types = new Type[] { typeof(Guid), typeof(string) };
            var method = typeof(IGrainFactory).GetTypeInfo().GetMethod("GetGrain", types);
            var generic = method.MakeGenericMethod(new Type[] { GetType().GetInterfaces().Last() });
            return (ISagaGrain)generic.Invoke(GrainFactory, new object[] { this.GetPrimaryKey(), null });
        }
    }
}
