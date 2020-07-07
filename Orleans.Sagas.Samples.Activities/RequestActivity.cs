using Microsoft.Extensions.Logging;
using Orleans.Runtime;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Activities
{
    public class RequestActivity : Activity<RequestConfig>
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<RequestActivity> logger;

        public RequestActivity(HttpClient httpClient, ILogger<RequestActivity> logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
        }

        public override async Task Execute(IActivityRuntimeContext context)
        {
            var response = await httpClient.GetAsync(Config.Url);
            logger.LogInformation($"Retrieved {response.Content.Headers.ContentLength} bytes from '{Config.Url}'.");
        }

        public override Task Compensate(IActivityRuntimeContext context)
        {
            return Task.CompletedTask;
        }
    }
}
