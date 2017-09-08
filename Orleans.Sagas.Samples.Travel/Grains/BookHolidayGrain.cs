using System.Threading.Tasks;
using Orleans.Sagas.Samples.Travel.Activities;
using Orleans.Sagas.Samples.Travel.Interfaces;

namespace Orleans.Sagas.Samples.Travel.Grains
{
    public class BookHolidayGrain : Grain, IBookHolidayGrain
    {
        public async Task Execute()
        {
            var sagaBuilder = GrainFactory.CreateSaga();

            sagaBuilder.AddActivity<BookHireCarActivity>(new BookHireCarConfig());
            sagaBuilder.AddActivity<BookHotelActivity>(new BookHotelConfig());
            sagaBuilder.AddActivity<BookPlaneActivity>(new BookPlaneConfig());

            await sagaBuilder.ExecuteSaga();
        }
    }
}
