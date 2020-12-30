using Orleans.Placement;
using System.Threading.Tasks;

namespace Orleans.Sagas
{
    public class SagaCancellationGrain : Grain<SagaCancellationGrainState>, ISagaCancellationGrain
    {
        public async Task RequestAbort()
        {
            if (!State.AbortRequested)
            {
                State.AbortRequested = true;
                await WriteStateAsync();
            }
        }

        public Task<bool> HasAbortBeenRequested()
        {
            return Task.FromResult(State.AbortRequested);
        }
    }
}
