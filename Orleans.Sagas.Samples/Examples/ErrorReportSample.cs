using Microsoft.Extensions.Logging;
using Orleans.Sagas.Samples.Activities;
using Orleans.Sagas.Samples.Activities.ValueObjects;
using System;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Examples
{
    class ErrorReportSample : Sample
    {
        private readonly SerializableErrorTranslator _errorTranslator = new SerializableErrorTranslator();

        public ErrorReportSample(IGrainFactory grainFactory, ILogger<Sample> logger) : base(grainFactory, logger)
        {
        }

        public override async Task Execute()
        {
            Describe("Activity that throws exception while executing, while using custom error translator");
            var sagaWithFailingExecutionActivityWithDefinedErrorTranslator =
                await GrainFactory.CreateSaga()
                .AddActivity<FailingExecutionActivity>()
                .AddErrorTranslator(_errorTranslator)
                .ExecuteSagaAsync();

            await sagaWithFailingExecutionActivityWithDefinedErrorTranslator.Wait();

            await ValidateSagaErrorAsync(sagaWithFailingExecutionActivityWithDefinedErrorTranslator, _errorTranslator, typeof(CustomException));


            Describe("Activity that throws exception while executing, without using custom error translator");
            var sagaWithFailingExecutionActivityWithNoErrorTranslator =
                await GrainFactory.CreateSaga()
                .AddActivity<FailingExecutionActivity>()
                .ExecuteSagaAsync();

            await sagaWithFailingExecutionActivityWithNoErrorTranslator.Wait();

            await ValidateSagaErrorAsync(sagaWithFailingExecutionActivityWithNoErrorTranslator, errorTranslator: null);
        }

        private void Describe(string activityType)
        {
            Logger.LogInformation($"Testing erroring report of type '{activityType}'...");
        }

        private async Task ValidateSagaErrorAsync(ISagaGrain saga, SerializableErrorTranslator errorTranslator = null, Type expectedExceptionType = null)
        {
            var errorAsString = await saga.GetSagaError();

            if (errorAsString != null)
            {
                Logger.LogInformation("Exception been thrown as expected");

                if (errorTranslator != null)
                {
                    ValidateErrorTranslate(errorAsString, errorTranslator, expectedExceptionType);
                }

                return;
            }

            Logger.LogError("Activity ran without throwing exceptions");
        }

        private void ValidateErrorTranslate(string errorAsString, SerializableErrorTranslator errorTranslator, Type expectedExceptionType)
        {
            var exception = errorTranslator.TranslateBack(errorAsString);
            if (exception.GetType() == expectedExceptionType)
            {
                Logger.LogInformation("Exception been translated as expected");
            }
            else
            {
                Logger.LogError("Error translate failed.");
            }
        }
    }
}
