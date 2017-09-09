using System.Threading.Tasks;
using Orleans.Runtime;
using System;

namespace Orleans.Sagas
{
    public abstract class Activity<TActivityConfig> : Activity, IActivity<TActivityConfig>
    {
        public void SetConfig(TActivityConfig config)
        {
            Config = config;
        }

        protected TActivityConfig Config { private set; get; }
    }

    public abstract class Activity : IActivity
    {
        public virtual string Name => this.GetType().Name;

        public void Initialize(Guid sagaId, IGrainFactory grainFactory, Logger logger)
        {
            SagaId = sagaId;
            GrainFactory = grainFactory;
            Logger = logger;
        }

        public abstract Task Execute();
        public abstract Task Compensate();

        protected Guid SagaId { get; private set; }
        protected IGrainFactory GrainFactory { get; private set; }
        protected Logger Logger { get; private set; }
    }
}
