using Orleans.Runtime;
using System.Threading.Tasks;

namespace Orleans.Sagas
{
    public interface IActivity
    {
        string Name { get; }
        void Initialize(IGrainFactory grainFactory, Logger logger);
        Task Execute();
        Task Compensate();
    }

    public interface IActivity<TActivityConfig> : IActivity
    {
        void SetConfig(TActivityConfig config);
    }
}
