using Microsoft.Extensions.Logging;
using Orleans.Sagas.Samples.Activities;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Examples
{
    public class TravelSample : Sample
    {
        public TravelSample(IGrainFactory client, ILogger<Sample> logger) : base(client, logger)
        {
        }

        public override async Task Execute()
        {
            var saga = await Client.CreateSaga()
                .AddActivity(new BookHireCarActivity { Config = new BookHireCarConfig() })
                .AddActivity(new BookHotelActivity())
                .AddActivity(new BookPlaneActivity())
                .ExecuteSaga();

            await saga.Wait();
        }
    }
}
