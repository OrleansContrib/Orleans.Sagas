using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Activities
{
    public class BookHireCarActivity : Activity
    {
        public override Task Compensate(IActivityContext context)
        {
            return Task.CompletedTask;
        }

        public override Task Execute(IActivityContext context)
        {
            var hireCarModel = context.SagaProperties.GetInt("HireCarModel");

            context.SagaProperties.Add("NumSuitcases", 3);

            return Task.CompletedTask;
        }
    }
}
