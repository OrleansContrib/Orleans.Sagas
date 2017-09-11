using Orleans.Runtime;
using System;
using System.Threading.Tasks;

namespace Orleans.Sagas
{
    public interface IActivity
    {
        string Name { get; }
        void Initialize(Guid sagaId, IGrainActivationContext grainContext);
        Task Execute();
        Task Compensate();
    }
}
