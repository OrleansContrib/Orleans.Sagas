using Orleans.Runtime;
using System;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Activities
{
    public class BookPlaneActivity : Activity<BookPlaneConfig>
    {
        public override Task Execute(IActivityRuntimeContext context)
        {
            // comment in to test compensation.
            //throw new SeatUnavailableException();
            return Task.CompletedTask;
        }

        public override Task Compensate(IActivityRuntimeContext context)
        {
            return Task.CompletedTask;
        }
    }
}
