using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Activities
{
    public class WaitActivity : IActivity
    {
        private readonly ILogger<WaitActivity> logger;

        public WaitActivity(ILogger<WaitActivity> logger)
        {
            this.logger = logger;
        }

        public async Task Execute(IActivityContext context)
        {
            await WaitAsync(context);
        }

        public async Task Compensate(IActivityContext context)
        {
            await WaitAsync(context);
        }

        private async Task WaitAsync(IActivityContext context, [CallerMemberName] string operation = null)
        {
            var waitTimeSeconds = context.SagaProperties.Get<int>("WaitTimeSeconds");
            logger.LogInformation($"{operation.TrimEnd('e')}ing for {waitTimeSeconds} seconds...");
            await Task.Delay(waitTimeSeconds * 1000);
        }
    }
}
