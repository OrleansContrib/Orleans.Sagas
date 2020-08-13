using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Activities
{
    public class BookHotelActivity : Activity
    {
        public override Task Compensate(IActivityContext context)
        {
            return Task.CompletedTask;
        }

        public override Task Execute(IActivityContext context)
        {
            return Task.CompletedTask;
        }
    }
}
