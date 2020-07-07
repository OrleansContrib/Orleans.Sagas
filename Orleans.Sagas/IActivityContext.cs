using Orleans.Runtime;
using System;

namespace Orleans.Sagas
{
    public interface IActivityContext
    {
        Guid SagaId { get; }
        IGrainFactory GrainFactory { get; }
        IGrainActivationContext GrainContext { get; }
        ISagaPropertyBag SagaProperties { get; }
    }
}
