using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Activities
{
    public class KickAssActivity : Activity
    {
        public override Task Execute(IActivityContext context)
        {
            var kickAssCount = context.SagaProperties.GetInt("KickAssCount");
            return Task.CompletedTask;
        }

        public override Task Compensate(IActivityContext context)
        {
            return Task.CompletedTask;
        }
    }
}
