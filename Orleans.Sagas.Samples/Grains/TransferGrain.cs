using System.Threading.Tasks;
using Orleans.Sagas.Samples.Interfaces;
using Orleans.Sagas.Samples.Activities;

namespace Orleans.Sagas.Samples.Grains
{
    public class TransferGrain : Grain, ITransferGrain
    {
        public async Task<ISagaGrain> RequestTransfer(int sourceAccount, int targetAccount, int amount)
        {
            var sagaBuilder = GrainFactory.CreateSaga();

            sagaBuilder.AddActivity<BalanceModificationActivity>(new BalanceModificationConfig
            {
                Account = sourceAccount,
                Amount = -amount
            });

            sagaBuilder.AddActivity<BalanceModificationActivity>(new BalanceModificationConfig
            {
                Account = targetAccount,
                Amount = amount
            });

            return await sagaBuilder.ExecuteSaga();
        }
    }
}
