using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Interfaces
{
    public interface IBookHolidayGrain : IGrainWithGuidKey
    {
        Task<ISagaGrain> Execute();
    }
}
