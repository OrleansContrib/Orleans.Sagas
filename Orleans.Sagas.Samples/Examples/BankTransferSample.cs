using Orleans.Runtime;
using Orleans.Sagas.Samples.Activities;
using Orleans.Sagas.Samples.Interfaces;
using System;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Examples
{
    public class BankTransferSample : Sample
    {
        public override async Task Execute()
        {
            // add some funds to two bank accounts.
            await GrainFactory.GetGrain<IBankAccountGrain>(1).ModifyBalance(Guid.Empty, 75);
            await GrainFactory.GetGrain<IBankAccountGrain>(2).ModifyBalance(Guid.Empty, 75);

            // enact some transfers.
            await TransferAndWait(1, 2, 25);
            await TransferAndWait(2, 1, 20);

            // comment in the below to blow account ceiling and test compensation.
            //await TransferAndWait(1, 2, 60);
        }

        private async Task TransferAndWait(int from, int to, int amount)
        {
            var saga = await Transfer(from, to, amount);
            await saga.Wait();

            Logger.Info("Account balances:");
            for (int accountId = 1; accountId <= 2; accountId++)
            {
                var account = GrainFactory.GetGrain<IBankAccountGrain>(accountId);
                Logger.Info($"  #{accountId} : {await account.GetBalance()}");
            }
        }

        private async Task<ISagaGrain> Transfer(int from, int to, int amount)
        {
            return await GrainFactory.CreateSaga()
                .AddActivity(new BalanceModificationActivity
                {
                    Config = new BalanceModificationConfig
                    {
                        Account = from,
                        Amount = -amount
                    }
                })
                .AddActivity(new BalanceModificationActivity
                {
                    Config = new BalanceModificationConfig
                    {
                        Account = to,
                        Amount = amount
                    }
                })
                .ExecuteSaga();
        }
    }
}
