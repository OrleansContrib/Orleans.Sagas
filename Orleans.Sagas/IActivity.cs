using Orleans.Runtime;
using System;
using System.Threading.Tasks;

namespace Orleans.Sagas
{
    public interface IActivity<TConfig> : IActivity
    {
        TConfig Config { set; get; }
    }

    public interface IActivity
    {
        string Name { get; }
        Task Execute(Guid sagaId, IGrainFactory grainFactory, IGrainActivationContext grainContext);
        Task Compensate(Guid sagaId, IGrainFactory grainFactory, IGrainActivationContext grainContext);
    }
}
