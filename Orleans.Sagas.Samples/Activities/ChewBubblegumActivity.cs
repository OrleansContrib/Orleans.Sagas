using Orleans.Runtime;
using Orleans.Sagas.Samples.Exceptions;
using System;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Activities
{
    public class ChewBubblegumActivity : Activity
    {
        public override Task Execute(Guid sagaId, IGrainFactory grainFactory, IGrainActivationContext grainContext)
        {
            // comment in to test compensation.
            //throw new AllOuttaGumException();
            return Task.CompletedTask;
        }

        public override Task Compensate(Guid sagaId, IGrainFactory grainFactory, IGrainActivationContext grainContext)
        {
            return Task.CompletedTask;
        }
    }
}
