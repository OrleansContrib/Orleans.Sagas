using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orleans.Sagas
{
    public class SagaBuilder : ISagaBuilder
    {
        private readonly List<ActivityDefinition> activities;
        private readonly IGrainFactory grainFactory;
        private IErrorTranslator _errorTranslator;

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
            return AddActivity<TActivity>(default(ISagaPropertyBag));
        }

        public ISagaBuilder AddActivity<TActivity>(ISagaPropertyBag properties) where TActivity : IActivity
        {
            // todo: serialize dynamic activity config safely.
            activities.Add(new ActivityDefinition(typeof(TActivity), properties));
            return this;
        }

        public ISagaBuilder AddActivity<TActivity>(Action<ISagaPropertyBag> propertiesDelegate) where TActivity : IActivity
        {
            var properties = Activator.CreateInstance<SagaPropertyBag>();
            propertiesDelegate.Invoke(properties);
            return AddActivity<TActivity>(properties);
        }

        public async Task<ISagaGrain> ExecuteSagaAsync()
        {
            return await ExecuteSagaAsync(null);
        }

        public ISagaBuilder AddErrorTranslator(IErrorTranslator errorTranslator)
        {
            _errorTranslator = errorTranslator;
            return this;
        }

        public async Task<ISagaGrain> ExecuteSagaAsync(Action<ISagaPropertyBag> propertiesDelegate)
        {
            if (activities.Count == 0)
            {
                throw new IndexOutOfRangeException("At least one activity must be present.");
            }

            var sagaGrain = grainFactory.GetGrain<ISagaGrain>(Id);

            SagaPropertyBag properties = null;
            
            if (propertiesDelegate != null)
            {
                properties = Activator.CreateInstance<SagaPropertyBag>();
                propertiesDelegate.Invoke(properties);
            }

            await sagaGrain.Execute(activities, properties, _errorTranslator);

            return sagaGrain;
        }
    }
}