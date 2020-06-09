using Orleans.Runtime;
using System;
using System.Threading.Tasks;

namespace Orleans.Sagas
{
    public interface IActivity
    {
        string Name { get; }
        Task Execute(Guid sagaId, IGrainFactory grainFactory, IGrainActivationContext grainContext);
        Task Compensate(Guid sagaId, IGrainFactory grainFactory, IGrainActivationContext grainContext);
    }
}
