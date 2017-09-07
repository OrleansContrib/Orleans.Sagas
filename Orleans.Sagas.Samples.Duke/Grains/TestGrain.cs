using System.Threading.Tasks;
using Orleans.Sagas.Samples.Duke.Activities;
using Orleans.Sagas.Samples.Duke.Interfaces;

namespace Orleans.Sagas.Samples.Duke.Grains
{
    public class TestGrain : Grain, ITestGrain
    {
        public async Task Go()
        {
            var saga = GrainFactory.CreateSaga();

            saga.AddActivity<KickAssActivity>(new KickAssConfig { KickAssCount = 7 });
            saga.AddActivity<ChewBubblegumActivity>();

            await saga.Execute();
        }
    }
}
