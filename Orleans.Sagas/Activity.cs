using System.Threading.Tasks;
using Orleans.Runtime;
using System;

namespace Orleans.Sagas
{
    public abstract class Activity<TConfig> : Activity
    {
        public TConfig Config { set; get; }
    }

    public abstract class Activity : IActivity
    {
        public virtual string Name => this.GetType().Name;
        [NonSerialized]
        private IGrainFactory grainFactory;
        [NonSerialized]
        private Guid guid;
        [NonSerialized]
        private Logger logger;

        public void Initialize(Guid sagaId, IGrainFactory grainFactory, Logger logger)
        {
            SagaId = sagaId;
            GrainFactory = grainFactory;
            Logger = logger;
        }

        public abstract Task Execute();
        public abstract Task Compensate();

        protected Guid SagaId { get { return guid; } private set { guid = value; } }
        protected IGrainFactory GrainFactory { get { return grainFactory; } private set { grainFactory = value; } }
        protected Logger Logger { get { return logger; } private set { logger = value; } }
    }
}
