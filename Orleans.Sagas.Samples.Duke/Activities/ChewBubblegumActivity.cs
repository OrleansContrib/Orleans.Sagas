using Orleans.Runtime;
using Orleans.Sagas.Samples.Duke.Exceptions;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Duke.Activities
{
    public class ChewBubblegumActivity : Activity
    {
        public override Task Execute()
        {
            throw new AllOuttaGumException();
        }

        public override Task Compensate()
        {
            return Task.CompletedTask;
        }
    }
}
