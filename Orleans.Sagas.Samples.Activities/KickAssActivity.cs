using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Activities
{
    public class KickAssActivity : IActivity
    {
        public Task Execute(IActivityContext context)
        {
            var kickAssCount = context.SagaProperties.GetInt("KickAssCount");
            return Task.CompletedTask;
        }

        public Task Compensate(IActivityContext context)
        {
            return Task.CompletedTask;
        }
    }
}
