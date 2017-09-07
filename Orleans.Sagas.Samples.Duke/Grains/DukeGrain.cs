using Orleans.Sagas.Samples.Duke.Activities;
using Orleans.Sagas.Samples.Duke.Interfaces;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Duke.Grains
{
    public class DukeGrain : Grain, IDukeGrain
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
