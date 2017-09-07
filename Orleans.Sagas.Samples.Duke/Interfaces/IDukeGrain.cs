using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Duke.Interfaces
{
    public interface IDukeGrain : IGrainWithGuidKey
    {
        Task Go();
    }
}
