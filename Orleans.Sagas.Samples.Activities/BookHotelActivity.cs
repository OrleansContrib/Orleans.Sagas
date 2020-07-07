using Orleans.Runtime;
using System;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Activities
{
    public class BookHotelActivity : Activity<BookHotelConfig>
    {
        public override Task Compensate(IActivityRuntimeContext context)
        {
            return Task.CompletedTask;
        }

        public override Task Execute(IActivityRuntimeContext context)
        {
            return Task.CompletedTask;
        }
    }
}
