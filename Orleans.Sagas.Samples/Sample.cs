using Orleans.Runtime;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples
{
    public abstract class Sample
    {
        protected IGrainFactory GrainFactory { get; private set; }
        protected Logger Logger { get; private set; }

        public void Initialize(IClusterClient client)
        {
            GrainFactory = client;
            Logger = client.Logger;
        }

        public abstract Task Execute();
    }
}