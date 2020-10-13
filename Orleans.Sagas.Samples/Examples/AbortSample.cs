using Microsoft.Extensions.Logging;
using Orleans.Sagas.Samples.Activities;
using System;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Examples
{
    class AbortSample : Sample
    {
        public AbortSample(IGrainFactory grainFactory, ILogger<Sample> logger) : base(grainFactory, logger)
        {
        }

        public override async Task Execute()
        {
            Describe("Abort before execute");
            Guid id = Guid.NewGuid();
            var sagaBuilder = CreateSaga(id);
            var saga = GrainFactory.GetSaga(id);
            await saga.RequestAbort();
            await sagaBuilder.ExecuteSagaAsync();
            await saga.Wait();
            await ValidateSagaAbortedSuccessfullyAsync(saga);

            Describe("Abort before start");
            var saga1 = await CreateAndExecuteSagaAsync();
            await saga1.RequestAbort();
            await saga1.Wait();
            await ValidateSagaAbortedSuccessfullyAsync(saga1);

            Describe("Abort before first activity ends");
            var saga2 = await CreateAndExecuteSagaAsync();
            await Task.Delay(1000);
            await saga2.RequestAbort();
            await saga2.Wait();
            await ValidateSagaAbortedSuccessfullyAsync(saga2);

            Describe("Abort before second activity ends");
            var saga3 = await CreateAndExecuteSagaAsync();
            await Task.Delay(4000);
            await saga3.RequestAbort();
            await saga3.Wait();
            await ValidateSagaAbortedSuccessfullyAsync(saga3);

            Describe("Abort after last activity ends / sagas has executed");
            var saga4 = await CreateAndExecuteSagaAsync();
            await Task.Delay(8000);
            await saga4.RequestAbort();
            await saga4.Wait();
            await ValidateSagaAbortedSuccessfullyAsync(saga4);

            Describe("Abort after abort completed");
            await saga4.RequestAbort();
            await ValidateSagaAbortedSuccessfullyAsync(saga4);
        }

        private void Describe(string abortType)
        {
            Logger.LogInformation($"Testing abort of type '{abortType}'...");
        }

        private async Task<ISagaGrain> CreateAndExecuteSagaAsync()
        {
            return await GrainFactory
                .CreateSaga()
                .AddActivity<WaitActivity>(x => x.Add("WaitTimeSeconds", 2))
                .AddActivity<WaitActivity>(x => x.Add("WaitTimeSeconds", 4))
                .ExecuteSagaAsync();
        }

        private ISagaBuilder CreateSaga(Guid id)
        {
            return GrainFactory
                .CreateSaga(id)
                .AddActivity<WaitActivity>(x => x.Add("WaitTimeSeconds", 2))
                .AddActivity<WaitActivity>(x => x.Add("WaitTimeSeconds", 4));
        }

        private async Task ValidateSagaAbortedSuccessfullyAsync(ISagaGrain saga)
        {
            var status = await saga.GetStatus();
            Logger.LogInformation($"Saga end status is {status}");
        }
    }
}
