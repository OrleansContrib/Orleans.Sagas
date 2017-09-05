namespace Orleans.Sagas
{
    public enum SagaStatus
    {
        NotStarted,
        Aborting,
        Aborted,
        Completed,
        Executing
    }
}