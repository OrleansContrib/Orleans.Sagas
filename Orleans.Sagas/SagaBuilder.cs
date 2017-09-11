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

        public SagaBuilder(IGrainFactory grainFactory)
        {
            Id = Guid.NewGuid();
            this.activities = new List<IActivity>();
            this.grainFactory = grainFactory;
        }

        public ISagaBuilder AddActivity(IActivity activity)
        {
            activities.Add(activity);
            return this;
        }

        public async Task<ISagaGrain> ExecuteSaga()
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