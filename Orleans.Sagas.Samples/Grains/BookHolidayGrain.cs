using System.Threading.Tasks;
using Orleans.Sagas.Samples.Activities;
using Orleans.Sagas.Samples.Interfaces;

namespace Orleans.Sagas.Samples.Grains
{
    public class BookHolidayGrain : Grain, IBookHolidayGrain
    {
        public async Task<ISagaGrain> Execute()
        {
            return await GrainFactory.CreateSaga()
                .AddActivity(new BookHireCarActivity { Config = new BookHireCarConfig() })
                .AddActivity(new BookHotelActivity())
                .AddActivity(new BookPlaneActivity())
                .ExecuteSaga();
        }
    }
}
