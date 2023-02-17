using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans.Sagas.Samples.Examples;
using System.Threading;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples
{
    class SampleRunner : IHostedService
    {
        private readonly Sample[] samples;
        private readonly ILogger<SampleRunner> logger;

        public SampleRunner(
            AbortSample abortSample,
            DependencyInjectionSample dependencyInjectionSample,
            BankTransferSample bankTransferSample,
            ConcurrencySample concurrencySample,
            DukeSample dukeSample,
            TravelSample travelSample,
            ErrorReportSample errorReportSample,
            ILogger<SampleRunner> logger)
        {
            samples = new Sample[]
            {
                abortSample,
                travelSample,
                dependencyInjectionSample,
                bankTransferSample,
                concurrencySample,
                dukeSample,
                //errorReportSample
            };

            this.logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            foreach (var sample in samples)
            {
                logger.LogDebug($"Running sample '{sample.GetType().Name}'...");
                await sample.Execute();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
