using Microsoft.Extensions.Logging;
using Orleans.Sagas.Samples.Activities;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Examples
{
    public class TravelSample : Sample
    {
        public TravelSample(IGrainFactory grainFactory, ILogger<Sample> logger) : base(grainFactory, logger)
        {
        }

        public override async Task Execute()
        {
            var saga = await GrainFactory.CreateSaga()
                .AddActivity<BookHireCarActivity>()
                .AddActivity<BookHotelActivity>()
                .AddActivity<BookPlaneActivity>()
                .ExecuteSagaAsync(x => x.Add("HireCarModel", 1));

            await saga.Wait();
        }
    }
}
