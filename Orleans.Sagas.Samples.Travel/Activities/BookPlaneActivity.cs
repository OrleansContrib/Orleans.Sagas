using Orleans.Sagas.Samples.Travel.Exceptions;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Travel.Activities
{
    public class BookPlaneActivity : Activity<BookPlaneConfig>
    {
        public override async Task Execute()
        {
            // testing reminder interleaving by waiting for the first one.
            await Task.Delay(75 * 1000);
            //throw new SeatUnavailableException();
        }

        public override Task Compensate()
        {
            return Task.CompletedTask;
        }
    }
}
