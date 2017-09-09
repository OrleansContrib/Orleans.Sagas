namespace Orleans.Sagas
{
    public class SagaCancellationGrainState
    {
        public bool AbortRequested { get; set; }
    }
}