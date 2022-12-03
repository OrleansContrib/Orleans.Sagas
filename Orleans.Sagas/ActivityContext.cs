using Orleans.Runtime;
using System;
using System.Collections.Generic;

namespace Orleans.Sagas
{
    public class ActivityContext : IActivityContext
    {
        public Guid SagaId { get; }
        public IGrainFactory GrainFactory { get; }
        public ISagaPropertyBag SagaProperties { get; }
        private IGrainContextAccessor GrainContextAccessor { get; }
        public IGrainContext GrainContext => GrainContextAccessor.GrainContext;
        
        public ActivityContext(Guid sagaId, IGrainFactory grainFactory, IGrainContextAccessor grainContextAccessor, Dictionary<string, string> existingProperties)
        {
            SagaId = sagaId;
            GrainFactory = grainFactory;
            GrainContextAccessor = grainContextAccessor;
            SagaProperties = new SagaPropertyBag(existingProperties);
        }

        public string GetSagaError()
        {
            if (!SagaProperties.ContainsKey(SagaPropertyBagKeys.ActivityErrorPropertyKey))
            {
                return null;
            }

            return SagaProperties.Get<string>(SagaPropertyBagKeys.ActivityErrorPropertyKey);
        }
    }
}
