using Microsoft.Extensions.Logging;
using Orleans.Sagas.Samples.Activities;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Examples
{
    class DependencyInjectionSample : Sample
    {
        public DependencyInjectionSample(IGrainFactory grainFactory, ILogger<Sample> logger) : base(grainFactory, logger)
        {
        }

        public override async Task Execute()
        {
            var saga = await GrainFactory
                .CreateSaga()
                .AddActivity<RequestActivity>(x => x.Add("Url", "https://dotnet.github.io/orleans/"))
                .AddActivity<RequestActivity>(x => x.Add("Url", "https://dot.net"))
                .AddActivity<RequestActivity>(x => x.Add("Url", "https://yahoo.com"))
                .ExecuteSagaAsync();

            await saga.Wait();
        }
    }
}
