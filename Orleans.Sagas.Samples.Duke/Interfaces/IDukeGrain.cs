using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Duke.Interfaces
{
    public interface IDukeGrain : IGrainWithIntegerKey
    {
        Task Execute();
        Task ExecuteAndAbort();
        Task AbortWithoutExecution();
        Task AbortThenExecute();
    }
}
