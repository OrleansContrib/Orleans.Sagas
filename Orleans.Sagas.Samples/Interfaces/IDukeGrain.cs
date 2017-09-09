using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Interfaces
{
    public interface IDukeGrain : IGrainWithIntegerKey
    {
        Task<ISagaGrain> Execute();
        Task<ISagaGrain> ExecuteAndAbort();
        Task<ISagaGrain> AbortWithoutExecution();
        Task<ISagaGrain> AbortThenExecute();
    }
}
