using Microsoft.Extensions.Logging;
using Orleans.Sagas.Samples.Activities;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Examples
{
    class ErrorReportSample : Sample
    {
        public ErrorReportSample(IGrainFactory grainFactory, ILogger<Sample> logger) : base(grainFactory, logger)
        {
        }

        public override async Task Execute()
        {
            Describe("Activity that throws exception while executing");
            var sagaWithFailingExecutionActivity = await GrainFactory.CreateSaga().AddActivity<FailingExecutionActivity>().ExecuteSagaAsync();
            await sagaWithFailingExecutionActivity.Wait();

            await ValidateSagaErrorAsync(sagaWithFailingExecutionActivity);
        }

        private void Describe(string activityType)
        {
            Logger.LogInformation($"Testing erroring report of type '{activityType}'...");
        }

        private async Task ValidateSagaErrorAsync(ISagaGrain saga)
        {
            var sagaError = await saga.GetSagaError();

            if(sagaError != null && sagaError.Exception is CustomException custom)
            {
                Logger.LogInformation("Exception been thrown as expected", custom.CustomErrorCode);
                return;
            }

            Logger.LogError("Activity ran without throwing exceptions");
        }
    }
}
