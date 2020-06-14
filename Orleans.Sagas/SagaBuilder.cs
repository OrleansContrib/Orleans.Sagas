using Orleans.Sagas;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orleans.Sagas
{
    public class SagaBuilder : ISagaBuilder
    {
        private List<IActivity> activities;
        private IGrainFactory grainFactory;

        public Guid Id { get; private set; }

        public SagaBuilder(IGrainFactory grainFactory) : this(grainFactory, Guid.NewGuid())
        {
        }

        public SagaBuilder(IGrainFactory grainFactory, Guid id)
        {
            this.grainFactory = grainFactory;
            Id = id;
            activities = new List<IActivity>();
        }

        public ISagaBuilder AddActivity(IActivity activity)
        {
            activities.Add(activity);
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