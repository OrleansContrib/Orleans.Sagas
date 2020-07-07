using Orleans.Runtime;
using System;

namespace Orleans.Sagas
{
    public interface IActivityRuntimeContext
    {
        Guid SagaId { get; }
        IGrainFactory GrainFactory { get; }
        IGrainActivationContext GrainContext { get; }
        ISagaPropertyBag SagaProperties { get; }
    }
}
