using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Activities.Interfaces
{
    public interface IBookHolidayGrain : IGrainWithGuidKey
    {
        Task<ISagaGrain> Execute();
    }
}
