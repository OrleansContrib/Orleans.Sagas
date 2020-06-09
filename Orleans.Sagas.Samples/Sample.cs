using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples
{
    public abstract class Sample
    {
        protected IGrainFactory Client { get; private set; }
        protected ILogger<Sample> Logger { get; }

        public Sample(IGrainFactory client, ILogger<Sample> logger)
        {
            Client = client;
            Logger = logger;
        }

        public abstract Task Execute();
    }
}