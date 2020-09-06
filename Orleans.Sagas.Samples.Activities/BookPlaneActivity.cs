using System;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Activities
{
    public class BookPlaneActivity : IActivity
    {
        public Task Execute(IActivityContext context)
        {
            var numSuitcases = context.SagaProperties.Get<int>("NumSuitcases");

            // comment in to test compensation.
            //throw new SeatUnavailableException();
            return Task.CompletedTask;
        }

        public Task Compensate(IActivityContext context)
        {
            return Task.CompletedTask;
        }
    }
}
