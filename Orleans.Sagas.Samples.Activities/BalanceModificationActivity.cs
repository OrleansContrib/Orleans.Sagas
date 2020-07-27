using Orleans.Sagas.Samples.Activities.Interfaces;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Activities
{
    public class BalanceModificationActivity : Activity<BalanceModificationConfig>
    {
        public override async Task Execute(IActivityContext context)
        {
            var sourceAccount = context.GrainFactory.GetGrain<IBankAccountGrain>(Config.Account);

            await sourceAccount.ModifyBalance(context.SagaId, Config.Amount);
        }

        public override async Task Compensate(IActivityContext context)
        {
            var sourceAccount = context.GrainFactory.GetGrain<IBankAccountGrain>(Config.Account);

            await sourceAccount.RevertBalanceModification(context.SagaId);
        }
    }
}
