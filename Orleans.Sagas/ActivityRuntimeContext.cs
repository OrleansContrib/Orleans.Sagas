using Orleans.Runtime;
using System;
using System.Collections.Generic;

namespace Orleans.Sagas
{
    public class ActivityRuntimeContext : IActivityRuntimeContext
    {
        public Guid SagaId { get; }
        public IGrainFactory GrainFactory { get; }
        public IGrainActivationContext GrainContext { get; }
        public ISagaPropertyBag SagaProperties { get; }

        public ActivityRuntimeContext(Guid sagaId, IGrainFactory grainFactory, IGrainActivationContext grainContext, Dictionary<string, object> existingProperties)
        {
            SagaId = sagaId;
            GrainFactory = grainFactory;
            GrainContext = grainContext;
            SagaProperties = new SagaPropertyBag(existingProperties);
        }
    }
}
