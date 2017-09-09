using System.Threading.Tasks;

namespace Orleans.Sagas
{
    public interface ISagaCancellationGrain : IGrainWithGuidKey
    {
        Task RequestAbort();
        Task<bool> HasAbortBeenRequested();
    }
}
