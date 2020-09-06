using System;
using System.Collections.Generic;

namespace Orleans.Sagas
{
    public class SagaState
    {
        public List<ActivityDefinition> Activities { get; set; }
        public int NumCompletedActivities { get; set; }
        public SagaStatus Status { get; set; }
        public int CompensationIndex { get; set; }
        public bool HasBeenAborted { get; set; }
        public Dictionary<string, string> Properties { get; set; }
    }
}