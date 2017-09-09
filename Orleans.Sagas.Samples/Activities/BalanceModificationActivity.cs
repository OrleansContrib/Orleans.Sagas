using Orleans.Sagas.Samples.Interfaces;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Activities
{
    public class BalanceModificationActivity : Activity<BalanceModificationConfig>
    {
        public override async Task Execute()
        {
            var sourceAccount = GrainFactory.GetGrain<IBankAccountGrain>(Config.Account);

            await sourceAccount.ModifyBalance(SagaId, Config.Amount);
        }

        public override async Task Compensate()
        {
            var sourceAccount = GrainFactory.GetGrain<IBankAccountGrain>(Config.Account);

            await sourceAccount.RevertBalanceModification(SagaId);
        }
    }
}
