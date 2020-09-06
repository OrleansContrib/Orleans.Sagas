using Orleans.Runtime;
using System;
using System.Collections.Generic;

namespace Orleans.Sagas
{
    public class ActivityContext : IActivityContext
    {
        public Guid SagaId { get; }
        public IGrainFactory GrainFactory { get; }
        public IGrainActivationContext GrainContext { get; }
        public ISagaPropertyBag SagaProperties { get; }

        public ActivityContext(Guid sagaId, IGrainFactory grainFactory, IGrainActivationContext grainContext, Dictionary<string, string> existingProperties)
        {
            SagaId = sagaId;
            GrainFactory = grainFactory;
            GrainContext = grainContext;
            SagaProperties = new SagaPropertyBag(existingProperties);
        }
    }
}
