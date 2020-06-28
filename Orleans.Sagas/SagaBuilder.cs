using Orleans.Sagas;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orleans.Sagas
{
    public class SagaBuilder : ISagaBuilder
    {
        private readonly List<ActivityDefinition> activities;
        private readonly IGrainFactory grainFactory;

        public Guid Id { get; private set; }

        public SagaBuilder(IGrainFactory grainFactory) : this(grainFactory, Guid.NewGuid())
        {
        }

        public SagaBuilder(IGrainFactory grainFactory, Guid id)
        {
            this.grainFactory = grainFactory;
            Id = id;
            activities = new List<ActivityDefinition>();
        }

        public ISagaBuilder AddActivity<TActivity>() where TActivity : IActivity
        {
            activities.Add(new ActivityDefinition(typeof(TActivity)));
            return this;
        }

        public ISagaBuilder AddActivity<TActivity, TConfig>(TConfig config) where TActivity : IActivity<TConfig>
        {
            // todo: serialize dynamic activity config safely.
            activities.Add(new ActivityDefinition<TConfig>(typeof(TActivity), config));
            return this;
        }

        public ISagaBuilder AddActivity<TActivity, TConfig>(Action<TConfig> configDelegate) where TActivity : IActivity<TConfig>
        {
            var config = Activator.CreateInstance<TConfig>();
            configDelegate.Invoke(config);
            AddActivity<TActivity, TConfig>(config);
            return this;
        }

        public async Task<ISagaGrain> ExecuteSagaAsync()
        {
            if (activities.Count == 0)
            {
                throw new IndexOutOfRangeException("At least one activity must be present.");
            }

            var sagaGrain = grainFactory.GetGrain<ISagaGrain>(Id);

            await sagaGrain.Execute(activities);

            return sagaGrain;
        }
    }
}