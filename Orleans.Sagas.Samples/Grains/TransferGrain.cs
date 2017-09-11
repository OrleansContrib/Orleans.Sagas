using Orleans.Sagas.Samples.Activities;
using Orleans.Sagas.Samples.Interfaces;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Grains
{
    public class TransferGrain : Grain, ITransferGrain
    {
        public async Task<ISagaGrain> RequestTransfer(int sourceAccount, int targetAccount, int amount)
        {
            return await GrainFactory.CreateSaga()
                .AddActivity(new BalanceModificationActivity
                {
                    Config = new BalanceModificationConfig
                    {
                        Account = sourceAccount,
                        Amount = -amount
                    }
                })
                .AddActivity(new BalanceModificationActivity
                {
                    Config = new BalanceModificationConfig
                    {
                        Account = targetAccount,
                        Amount = amount
                    }
                })
                .ExecuteSaga();
        }
    }
}
