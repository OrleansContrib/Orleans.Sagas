using Microsoft.Extensions.Logging;
using Orleans.Sagas.Samples.Activities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Examples
{
    public class DukeSample : Sample
    {
        public DukeSample(IGrainFactory grainFactory, ILogger<Sample> logger) : base(grainFactory, logger)
        {
        }

        public override async Task Execute()
        {
            await (await ExecuteNormally()).Wait();
            await (await ExecuteAndAbort()).Wait();
            await (await AbortWithoutExecution()).Wait();
            await (await AbortThenExecute()).Wait();
        }

        private async Task<ISagaGrain> ExecuteNormally()
        {
            Logger.LogWarning(nameof(ExecuteNormally));

            var sagaBuilder = GrainFactory.CreateSaga();

            AddActivities(sagaBuilder);

            return await sagaBuilder.ExecuteSagaAsync();
        }

        public async Task<ISagaGrain> ExecuteAndAbort()
        {
            Logger.LogWarning(nameof(ExecuteAndAbort));

            var sagaBuilder = GrainFactory.CreateSaga();

            AddActivities(sagaBuilder);

            var saga = await sagaBuilder.ExecuteSagaAsync();

            await saga.RequestAbort();

            return saga;
        }

        public async Task<ISagaGrain> AbortWithoutExecution()
        {
            Logger.LogWarning(nameof(AbortWithoutExecution));

            var sagaBuilder = GrainFactory.CreateSaga();

            var saga = GrainFactory.GetSaga(sagaBuilder.Id);

            await saga.RequestAbort();

            return saga;
        }

        public async Task<ISagaGrain> AbortThenExecute()
        {
            Logger.LogWarning(nameof(AbortThenExecute));

            var sagaBuilder = GrainFactory.CreateSaga();

            var saga = GrainFactory.GetSaga(sagaBuilder.Id);

            await saga.RequestAbort();

            AddActivities(sagaBuilder);

            await sagaBuilder.ExecuteSagaAsync();

            return saga;
        }

        private static void AddActivities(ISagaBuilder sagaBuilder)
        {
            sagaBuilder
                .AddActivity<KickAssActivity>(x => x.Add("KickAssCount", 7))
                .AddActivity<ChewBubblegumActivity>();
        }
    }
}
