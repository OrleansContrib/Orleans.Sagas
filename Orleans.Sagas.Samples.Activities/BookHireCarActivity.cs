using Orleans.Runtime;
using System;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Activities
{
    public class BookHireCarActivity : Activity<BookHireCarConfig>
    {
        public override Task Compensate(Guid sagaId, IGrainFactory grainFactory, IGrainActivationContext grainContext)
        {
            return Task.CompletedTask;
        }

        public override Task Execute(Guid sagaId, IGrainFactory grainFactory, IGrainActivationContext grainContext)
        {
            return Task.CompletedTask;
        }
    }
}
