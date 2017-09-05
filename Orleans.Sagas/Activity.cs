using System.Threading.Tasks;
using Orleans.Runtime;

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

        public void Initialize(IGrainFactory grainFactory, Logger logger)
        {
            GrainFactory = grainFactory;
            Logger = logger;
        }

        public abstract Task Execute();
        public abstract Task Compensate();
        protected IGrainFactory GrainFactory { get; private set; }
        protected Logger Logger { get; private set; }
    }
}
