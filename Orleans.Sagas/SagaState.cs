using System;
using System.Collections.Generic;

namespace Orleans.Sagas
{
    public class SagaState
    {
        public IEnumerable<Tuple<Type, object>> Activities { get; set; }
        public object[] Configs { get; set; }
        public int NumCompletedActivities { get; set; }
        public SagaStatus Status { get; set; }
        public int CompensationIndex { get; set; }
    }
}