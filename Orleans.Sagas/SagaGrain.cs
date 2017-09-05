using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Orleans.Sagas
{
    public abstract class SagaGrain : Grain<SagaState>, ISagaGrain
    {
        private List<IActivity> activities;
        private bool hasActivationResumed;
        private IDisposable resumeTimer;
        
        public async Task Abort()
        {
            GetLogger().Warn(0, $"Aborting {GetType().Name} saga.");
            State.Status = SagaStatus.Aborting;
            await WriteStateAsync();
        }

        public async Task Execute(params object[] configs)
        {
            if (State.Status == SagaStatus.NotStarted)
            {
                State.Configs = configs;
                activities = (await DefineSaga()).ToList();
                for (int i = 0; i < configs.Length; i++)
                {
                    var config = configs[i];
                    var type = typeof(IActivity<>).MakeGenericType(config.GetType());
                    var method = type.GetTypeInfo().GetMethod("SetConfig");
                    method.Invoke(activities[i], new object[] { config });
                }
                await RegisterSaga();
                State.Status = SagaStatus.Executing;
                await WriteStateAsync();
                resumeTimer = RegisterTimer(OnResumeTimer, null, TimeSpan.FromSeconds(1), TimeSpan.MaxValue);
            }
        }

        private Task RegisterSaga()
        {
            // TODO: Register saga with a durable coordinator to ensure completion.
            return Task.CompletedTask;
        }

        private async Task OnResumeTimer(object state)
        {
            var self = CreateReferenceToSelf();
            await self.UpdateSaga();
        }

        public Task<SagaStatus> GetStatus()
        {
            return Task.FromResult(State.Status);
        }

        public async Task UpdateSaga()
        {
            if (resumeTimer != null)
            { 
                resumeTimer.Dispose();
                resumeTimer = null;
            }

            if (hasActivationResumed)
            {
                return;
            }

            hasActivationResumed = true;

            await ResumeSaga();
        }

        protected abstract Task<List<IActivity>> DefineSaga();

        private async Task ResumeSaga()
        {
            while (State.Status != SagaStatus.Completed &&
                   State.Status != SagaStatus.Aborted &&
                   State.Status != SagaStatus.NotStarted)
            {
                if (State.Status == SagaStatus.Executing)
                {
                    await HandleExecuting();
                }
                else if (State.Status == SagaStatus.Aborting)
                {
                    await HandleCompensating();
                }
            }

            var message = "Saga '" + GetType().Name + "' completed with status '" + State.Status + "'.";

            if (State.Status == SagaStatus.Completed)
            {
                GetLogger().Info(message);
            }
            else
            {
                GetLogger().Warn(0, message);
            }
        }

        private async Task HandleExecuting()
        {
            if (State.NumCompletedActivities < activities.Count)
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
                    State.Status = SagaStatus.Aborting;
                    await WriteStateAsync();
                }
            }
            else
            {
                State.Status = SagaStatus.Completed;
                await WriteStateAsync();
            }
        }

        private async Task HandleCompensating()
        {
            if (State.CompensationIndex >= 0)
            {
                var currentActivity = activities[State.CompensationIndex];

                currentActivity.Initialize(GrainFactory, GetLogger());
                GetLogger().Warn(0, $"Compensating for activity #{State.CompensationIndex} '{currentActivity.Name}'...");
                await currentActivity.Compensate();
                GetLogger().Warn(0, $"...activity #{State.CompensationIndex} '{currentActivity.Name}' compensation complete.");
                State.CompensationIndex--;
                await WriteStateAsync();
            }
            else
            {
                State.Status = SagaStatus.Aborted;
                await WriteStateAsync();
            }
        }

        private ISagaGrain CreateReferenceToSelf()
        {
            var types = new Type[] { typeof(Guid), typeof(string) };
            var method = typeof(IGrainFactory).GetTypeInfo().GetMethod("GetGrain", types);
            var generic = method.MakeGenericMethod(new Type[] { GetType().GetInterfaces().Last() });
            return (ISagaGrain)generic.Invoke(GrainFactory, new object[] { this.GetPrimaryKey(), null });
        }
    }
}
