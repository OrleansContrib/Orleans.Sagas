namespace Orleans.Sagas
{
    public class SagaState
    {
        public object[] Configs { get; set; }
        public int NumCompletedActivities { get; set; }
        public SagaStatus Status { get; set; }
        public int CompensationIndex { get; set; }
    }
}