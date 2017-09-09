using Orleans.Sagas.Samples.Exceptions;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Activities
{
    public class ChewBubblegumActivity : Activity
    {
        public override Task Execute()
        {
            // comment in to test compensation.
            //throw new AllOuttaGumException();
            return Task.CompletedTask;
        }

        public override Task Compensate()
        {
            return Task.CompletedTask;
        }
    }
}
