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
            await GrainFactory
                .CreateSaga()
                .AddActivity<RequestActivity, RequestConfig>(x => x.Url = "https://dotnet.github.io/orleans/" )
                .ExecuteSagaAsync();
        }
    }
}
