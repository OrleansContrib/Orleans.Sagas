using Microsoft.Extensions.Hosting;
using Orleans.Sagas.Samples.Examples;
using System.Threading;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples
{
    class SampleRunner : IHostedService
    {
        private readonly Sample[] samples;

        public SampleRunner(
            BankTransferSample bankTransferSample,
            ConcurrencySample concurrencySample,
            DukeSample dukeSample,
            TravelSample travelSample)
        {
            samples = new Sample[]
            {
                bankTransferSample,
                concurrencySample,
                dukeSample,
                travelSample
            };
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            foreach (var sample in samples)
            {
                await sample.Execute();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
