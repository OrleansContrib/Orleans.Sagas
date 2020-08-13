using Microsoft.Extensions.Logging;
using Orleans.Sagas.Samples.Activities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Examples
{
    public class ConcurrencySample : Sample
    {
        public ConcurrencySample(IGrainFactory client, ILogger<Sample> logger) : base(client, logger)
        {
        }

        public override async Task Execute()
        {
            var sagas = new List<ISagaGrain>();
            for (int i = 0; i < 20; i++)
            {
                sagas.Add(await ExecuteNormally());
            }
            await sagas.Wait();
        }

        private async Task<ISagaGrain> ExecuteNormally()
        {
            var sagaBuilder = GrainFactory.CreateSaga();

            sagaBuilder
                .AddActivity<KickAssActivity>(x => x.Add("KickAssCount", 7))
                .AddActivity<ChewBubblegumActivity>();

            return await sagaBuilder.ExecuteSagaAsync();
        }
    }
}
