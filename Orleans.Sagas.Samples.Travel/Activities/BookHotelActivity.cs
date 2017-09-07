using System;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Travel.Activities
{
    public class BookHotelActivity : Activity<BookHotelConfig>
    {
        public override Task Compensate()
        {
            return Task.CompletedTask;
        }

        public override Task Execute()
        {
            return Task.CompletedTask;
        }
    }
}
