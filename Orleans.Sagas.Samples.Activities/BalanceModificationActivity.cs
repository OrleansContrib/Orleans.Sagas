using Orleans.Sagas.Samples.Activities.Interfaces;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Activities
{
    public class BalanceModificationActivity : IActivity
    {
        public async Task Execute(IActivityContext context)
        {
            var account = context.SagaProperties.GetInt("Account");
            var amount = context.SagaProperties.GetInt("Amount");

            var sourceAccount = context.GrainFactory.GetGrain<IBankAccountGrain>(account);

            await sourceAccount.ModifyBalance(context.SagaId, amount);
        }

        public async Task Compensate(IActivityContext context)
        {
            var account = context.SagaProperties.GetInt("Account");

            var sourceAccount = context.GrainFactory.GetGrain<IBankAccountGrain>(account);

            await sourceAccount.RevertBalanceModification(context.SagaId);
        }
    }
}
