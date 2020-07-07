using System;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Activities
{
    public class BookPlaneActivity : Activity<BookPlaneConfig>
    {
        public override Task Execute(IActivityContext context)
        {
            var numSuitcases = context.SagaProperties.GetInt("NumSuitcases");

            // comment in to test compensation.
            //throw new SeatUnavailableException();
            return Task.CompletedTask;
        }

        public override Task Compensate(IActivityContext context)
        {
            return Task.CompletedTask;
        }
    }
}
