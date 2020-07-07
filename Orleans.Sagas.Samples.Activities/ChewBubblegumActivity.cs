using Orleans.Runtime;
using Orleans.Sagas.Samples.Activities.Exceptions;
using System;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Activities
{
    public class ChewBubblegumActivity : Activity
    {
        public override Task Execute(IActivityRuntimeContext context)
        {
            // comment in to test compensation.
            //throw new AllOuttaGumException();
            return Task.CompletedTask;
        }

        public override Task Compensate(IActivityRuntimeContext context)
        {
            return Task.CompletedTask;
        }
    }
}
