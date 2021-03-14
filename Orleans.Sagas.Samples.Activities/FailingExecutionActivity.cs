using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Activities
{
    public class FailingExecutionActivity : IActivity
    {
        public Task Execute(IActivityContext context)
        {
            throw new CustomException(1, "Could not execute this activity");
        }

        public Task Compensate(IActivityContext context)
        {
            return Task.CompletedTask;
        }
    }
}
