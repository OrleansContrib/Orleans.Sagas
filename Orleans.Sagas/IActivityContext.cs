using Orleans.Runtime;
using System;

namespace Orleans.Sagas
{
    public interface IActivityContext
    {
        Guid SagaId { get; }
        IGrainFactory GrainFactory { get; }
        IGrainContext GrainContext { get; }
        ISagaPropertyBag SagaProperties { get; }
        string GetSagaError();
    }
}
