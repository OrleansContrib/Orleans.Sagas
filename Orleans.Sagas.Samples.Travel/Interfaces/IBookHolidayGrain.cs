using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Travel.Interfaces
{
    public interface IBookHolidayGrain : IGrainWithGuidKey
    {
        Task Go();
    }
}
