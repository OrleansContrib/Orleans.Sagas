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

        public ISagaBuilder AddActivity<TActivity>() where TActivity : IActivity
        {
            var info = typeof(TActivity).GetTypeInfo();

            if (info.BaseType.GenericTypeArguments.Length > 0)
            {
                throw new ConfigNotProvidedException();
            }

            AddActivity<TActivity>(null);

            return this;
        }

        public ISagaBuilder AddActivity<TActivity>(object config) where TActivity : IActivity
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
            else if (config != null && !typeArgs[0].IsAssignableFrom(config.GetType()))
            {
                throw new IncompatibleActivityAndConfigException();
            }

            activities.Add(new Tuple<Type, object>(typeof(TActivity), config));

            return this;
        }

        public async Task<ISagaGrain> ExecuteSaga()
        {
            if (activities.Count == 0)
            {
                throw new NoActivitiesInSagaException();
            }

            var sagaGrain = grainFactory.GetGrain<ISagaGrain>(Id);

            await sagaGrain.Execute(activities);

            return sagaGrain;
        }
    }
}