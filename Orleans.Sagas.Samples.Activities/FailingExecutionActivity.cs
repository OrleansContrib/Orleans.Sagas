using System;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Activities
{
    public class FailingExecutionActivity : IActivity
    {
        public Task Execute(IActivityContext context)
        {
            throw new Exception("Can not execute this activity");
        }

        public Task Compensate(IActivityContext context)
        {
            return Task.CompletedTask;
        }
    }
}
