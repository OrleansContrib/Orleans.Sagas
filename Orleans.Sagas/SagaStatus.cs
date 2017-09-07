namespace Orleans.Sagas
{
    public enum SagaStatus
    {
        NotStarted,
        Executing,
        Executed,
        Compensating,
        Compensated
    }
}