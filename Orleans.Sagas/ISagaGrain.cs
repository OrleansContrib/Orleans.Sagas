using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orleans.Sagas
{
    public interface ISagaGrain : IGrainWithGuidKey
    {
        Task Abort();
        Task Execute(IEnumerable<Tuple<Type, object>> activities);
        Task<SagaStatus> GetStatus();
        Task Resume();
    }
}