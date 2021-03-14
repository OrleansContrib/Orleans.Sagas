using System;

namespace Orleans.Sagas
{
    public class SagaError
    {
        public string ActivityName { get; }

        public Exception Exception { get; }

        public SagaError(string activityName, Exception exception)
        {
            ActivityName = activityName;
            Exception = exception;
        }
    }
}