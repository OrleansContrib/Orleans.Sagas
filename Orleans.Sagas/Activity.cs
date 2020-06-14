using System;
using System.Threading.Tasks;
using Orleans.Runtime;

namespace Orleans.Sagas
{
    public abstract class Activity<TConfig> : Activity, IActivity<TConfig>
    {
        public TConfig Config { get; set; }
    }

    public abstract class Activity : IActivity
    {
        public string Name { get; }

        public Activity()
        {
            Name = GetType().Name;
        }

        public abstract Task Execute(Guid sagaId, IGrainFactory grainFactory, IGrainActivationContext grainContext);
        public abstract Task Compensate(Guid sagaId, IGrainFactory grainFactory, IGrainActivationContext grainContext);
    }
}
