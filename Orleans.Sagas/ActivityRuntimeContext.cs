using Orleans.Runtime;
using System;

namespace Orleans.Sagas
{
    public class ActivityRuntimeContext : IActivityRuntimeContext
    {
        public Guid SagaId { get; }
        public IGrainFactory GrainFactory { get; }
        public IGrainActivationContext GrainContext { get; }

        public ActivityRuntimeContext(Guid sagaId, IGrainFactory grainFactory, IGrainActivationContext grainContext)
        {
            SagaId = sagaId;
            GrainFactory = grainFactory;
            GrainContext = grainContext;
        }
    }
}
