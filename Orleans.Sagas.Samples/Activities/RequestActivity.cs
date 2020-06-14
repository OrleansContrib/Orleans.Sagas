using Microsoft.Extensions.Logging;
using Orleans.Runtime;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Activities
{
    class RequestActivity : Activity<RequestConfig>
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<RequestActivity> logger;

        public RequestActivity(HttpClient httpClient, ILogger<RequestActivity> logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
        }

        public override async Task Execute(Guid sagaId, IGrainFactory grainFactory, IGrainActivationContext grainContext)
        {
            var response = await httpClient.GetAsync(Config.Url);
            logger.LogInformation($"Retrieved {response.Content.Headers.ContentLength} bytes from '{Config.Url}'.");
        }

        public override Task Compensate(Guid sagaId, IGrainFactory grainFactory, IGrainActivationContext grainContext)
        {
            return Task.CompletedTask;
        }
    }
}
