using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Activities
{
    public class BookHireCarActivity : Activity<BookHireCarConfig>
    {
        public override Task Compensate(IActivityRuntimeContext context)
        {
            return Task.CompletedTask;
        }

        public override Task Execute(IActivityRuntimeContext context)
        {
            context.SagaProperties.Add("NumSuitcases", 3);

            return Task.CompletedTask;
        }
    }
}
