using Orleans.Sagas.Samples.Activities.Data;
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
            var hireCarModel = context.SagaProperties.Get<int>("HireCarModel");

            context.SagaProperties.Add("NumSuitcases", 3);
            context.SagaProperties.Add(nameof(CarInfo), new CarInfo { Make = "Nissan", Model = "Pathfinder" });

            return Task.CompletedTask;
        }
    }
}
