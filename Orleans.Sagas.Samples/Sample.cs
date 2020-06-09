using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples
{
    public abstract class Sample
    {
        protected IGrainFactory GrainFactory { get; private set; }
        protected ILogger<Sample> Logger { get; }

        public Sample(IGrainFactory grainFactory, ILogger<Sample> logger)
        {
            GrainFactory = grainFactory;
            Logger = logger;
        }

        public abstract Task Execute();
    }
}