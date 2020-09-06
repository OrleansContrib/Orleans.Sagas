using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Activities
{
    public class RequestActivity : IActivity
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<RequestActivity> logger;

        public RequestActivity(HttpClient httpClient, ILogger<RequestActivity> logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
        }

        public async Task Execute(IActivityContext context)
        {
            var url = context.SagaProperties.Get<string>("Url");
            var response = await httpClient.GetAsync(url);
            logger.LogInformation($"Retrieved {response.Content.Headers.ContentLength} bytes from '{url}'.");
        }

        public Task Compensate(IActivityContext context)
        {
            return Task.CompletedTask;
        }
    }
}
