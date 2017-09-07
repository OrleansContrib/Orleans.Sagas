using System.Threading.Tasks;
using Orleans.Sagas.Samples.Travel.Activities;
using Orleans.Sagas.Samples.Travel.Interfaces;

namespace Orleans.Sagas.Samples.Travel.Grains
{
    public class BookHolidayGrain : SagaGrain, IBookHolidayGrain
    {
        public async Task Go()
        {
            var saga = GrainFactory.CreateSaga();

            saga.AddActivity<BookHireCarActivity>(new BookHireCarConfig());
            saga.AddActivity<BookHotelActivity>(new BookHotelConfig());
            saga.AddActivity<BookPlaneActivity>(new BookPlaneConfig());

            await saga.Execute();
        }
    }
}
