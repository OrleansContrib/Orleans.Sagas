using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Runtime;

namespace Orleans.Sagas
{
    public abstract class Activity<TConfig> : Activity
    {
        public TConfig Config { set; get; }
    }

    public abstract class Activity : IActivity
    {
        [NonSerialized]
        private Guid sagaId;

        [NonSerialized]
        private IGrainActivationContext grainContext;

        public virtual string Name => this.GetType().Name;

        public void Initialize(Guid sagaId, IGrainActivationContext grainContext)
        {
            this.sagaId = sagaId;
            this.grainContext = grainContext;
        }

        public abstract Task Execute();
        public abstract Task Compensate();

        protected Guid SagaId { get { return sagaId; } }
        protected IGrainActivationContext GrainContext { get { return grainContext; } }
        protected IGrainFactory GrainFactory { get { return grainContext.ActivationServices.GetRequiredService<IGrainFactory>(); } }
    }
}
