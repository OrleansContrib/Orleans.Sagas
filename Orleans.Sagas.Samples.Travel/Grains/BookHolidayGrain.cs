using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans.Sagas.Samples.Travel.Activities;
using Orleans.Sagas.Samples.Travel.Interfaces;

namespace Orleans.Sagas.Samples.Travel.Grains
{
    public class BookHolidayGrain : SagaGrain, IBookHolidayGrain
    {
        protected override Task<List<IActivity>> DefineSaga()
        {
            return Task.FromResult(new List<IActivity> {
                new BookHireCarActivity(),
                new BookHotelActivity(),
                new BookPlaneActivity()
            });
        }
    }
}
