using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Activities
{
    public class ChewBubblegumActivity : IActivity
    {
        public Task Execute(IActivityContext context)
        {
            // comment in to test compensation.
            //throw new AllOuttaGumException();
            return Task.CompletedTask;
        }

        public Task Compensate(IActivityContext context)
        {
            return Task.CompletedTask;
        }
    }
}
