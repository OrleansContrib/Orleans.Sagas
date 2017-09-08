namespace Orleans.Sagas
{
    public enum SagaStatus
    {
        /// <summary>
        /// This saga has not yet been registered and begun executing.
        /// </summary>
        NotStarted,
        /// <summary>
        /// This saga is currently executing (moving forward).
        /// </summary>
        Executing,
        /// <summary>
        /// This saga has completed successfully.
        /// </summary>
        Executed,
        /// <summary>
        /// This saga is currently compensating after a execution failure (moving backward).
        /// </summary>
        Compensating,
        /// <summary>
        /// This saga has completed compensating after a failure.
        /// </summary>
        Compensated,
        /// <summary>
        /// This saga has completed compensating after an abort.
        /// </summary>
        Aborted
    }
}