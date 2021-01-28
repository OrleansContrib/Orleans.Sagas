using System;

namespace Orleans.Sagas
{
    public class SagaError
    {
        public Exception Exception { get; }

        public SagaError(Exception exception)
        {
            Exception = exception;
        }
    }
}