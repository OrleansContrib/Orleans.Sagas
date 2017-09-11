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
                .AddActivity<BookHireCarActivity>(new BookHireCarConfig())
                .AddActivity<BookHotelActivity>(new BookHotelConfig())
                .AddActivity<BookPlaneActivity>(new BookPlaneConfig())
                .ExecuteSaga();
        }
    }
}
