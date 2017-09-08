using Orleans.Sagas;
using Orleans.Sagas.Exceptions;
using System;
using System.Collections.Generic;
using System.Reflection;
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
            var info = typeof(TActivity).GetTypeInfo();

            if (info.BaseType.GenericTypeArguments.Length > 0)
            {
                throw new ConfigNotProvidedException();
            }

            AddActivity<TActivity>(null);
        }

        public void AddActivity<TActivity>(object config) where TActivity : IActivity
        {
            var info = typeof(TActivity).GetTypeInfo();

            var typeArgs = info.BaseType.GenericTypeArguments;

            if (config != null && typeArgs.Length == 0)
            {
                throw new ConfigNotRequiredException();
            }
            else if (config == null && typeArgs.Length > 0)
            {
                throw new ConfigNotProvidedException();
            }
            else if (config != null && typeArgs[0] != config.GetType())
            {
                throw new IncompatibleActivityAndConfigException();
            }

            activities.Add(new Tuple<Type, object>(typeof(TActivity), config));
        }

        public async Task Execute()
        {
            if (activities.Count == 0)
            {
                throw new NoActivitiesInSagaException();
            }

            await grainFactory.GetGrain<ISagaGrain>(Id).Execute(activities);
        }
    }
}