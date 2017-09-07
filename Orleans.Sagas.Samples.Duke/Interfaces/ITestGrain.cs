using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Duke.Interfaces
{
    public interface ITestGrain
    {
        Task Go();
    }
}