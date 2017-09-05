using System.Threading.Tasks;
using System;

namespace Orleans.Sagas
{
    public interface ISagaGrain : IGrainWithGuidKey
    {
        Task Abort();
        Task Execute(params object[] configs);
        Task<SagaStatus> GetStatus();
        Task UpdateSaga();
    }
}