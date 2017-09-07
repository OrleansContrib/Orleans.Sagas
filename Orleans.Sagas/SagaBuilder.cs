using Orleans.Sagas;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orleans
{
    public class SagaBuilder : ISagaBuilder
    {
        private List<Tuple<Type, object>> activities;
        private IGrainFactory grainFactory;

        public Guid Id { get; private set; }

        public SagaBuilder(IGrainFactory grainFactory)
        {
            Id = Guid.NewGuid();
            this.activities = new List<Tuple<Type, object>>();
            this.grainFactory = grainFactory;
        }

        public void AddActivity<TActivity>() where TActivity : IActivity
        {
            AddActivity<TActivity>(null);
        }

        public void AddActivity<TActivity>(object config) where TActivity : IActivity
        {
            activities.Add(new Tuple<Type, object>(typeof(TActivity), config));
        }

        public async Task Execute()
        {
            await grainFactory.GetGrain<ISagaGrain>(Id).Execute(activities);
        }
    }
}