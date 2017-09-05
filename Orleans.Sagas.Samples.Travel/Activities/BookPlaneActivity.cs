using Orleans.Sagas.Samples.Travel.Exceptions;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Travel.Activities
{
    public class BookPlaneActivity : Activity
    {
        public override Task Execute()
        {
            throw new SeatUnavailableException();
        }

        public override Task Compensate()
        {
            return Task.CompletedTask;
        }
    }
}
