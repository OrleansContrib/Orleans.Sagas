using Orleans.Runtime;
using Orleans.Sagas.Samples.Interfaces;
using System;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Activities
{
    public class BalanceModificationActivity : Activity<BalanceModificationConfig>
    {
        public override async Task Execute(Guid sagaId, IGrainFactory grainFactory, IGrainActivationContext grainContext)
        {
            var sourceAccount = grainFactory.GetGrain<IBankAccountGrain>(Config.Account);

            await sourceAccount.ModifyBalance(sagaId, Config.Amount);
        }

        public override async Task Compensate(Guid sagaId, IGrainFactory grainFactory, IGrainActivationContext grainContext)
        {
            var sourceAccount = grainFactory.GetGrain<IBankAccountGrain>(Config.Account);

            await sourceAccount.RevertBalanceModification(sagaId);
        }
    }
}
