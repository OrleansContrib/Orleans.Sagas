using Orleans.Sagas.Samples.Duke.Activities;
using Orleans.Sagas.Samples.Duke.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Duke.Grains
{
    public class DukeGrain : SagaGrain, IDukeGrain
    {
        protected override Task<List<IActivity>> DefineSaga()
        {
            return Task.FromResult(new List<IActivity> {
                new KickAssActivity(),
                new ChewBubblegumActivity()
            });
        }
    }
}
