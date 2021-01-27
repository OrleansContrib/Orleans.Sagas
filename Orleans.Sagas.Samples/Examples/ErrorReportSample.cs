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
            var sagaErrors = await saga.GetSagaErrors();

            foreach(var activityError in sagaErrors)
            {
                if (activityError.Value == null)
                {
                    Logger.LogInformation($"Activity {activityError.Key} ran without errors");
                    continue;
                }

                Logger.LogInformation($"Activity = {activityError.Key}, exception is {activityError.Value.Exception}");
            }
        }
    }
}
