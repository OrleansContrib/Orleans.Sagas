using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Activities
{
    public class BookHireCarActivity : IActivity
    {
        public Task Compensate(IActivityContext context)
        {
            return Task.CompletedTask;
        }

        public Task Execute(IActivityContext context)
        {
            var hireCarModel = context.SagaProperties.GetInt("HireCarModel");

            context.SagaProperties.Add("NumSuitcases", 3);

            return Task.CompletedTask;
        }
    }
}
