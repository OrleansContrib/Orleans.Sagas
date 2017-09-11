using Orleans.Runtime;
using Orleans.Sagas.Samples.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).Wait();
            while (true);
        }

        static async Task MainAsync(string[] args)
        {
            var client = await OrleansHost.CreateOrleansSiloAndClient();

            await BankTransferSample(client);
            await DukeSample(client);
            await TravelSample(client);
            await ConcurrencySample(client);
        }

        static async Task BankTransferSample(IClusterClient client)
        {
            await client.GetGrain<IBankAccountGrain>(1).ModifyBalance(Guid.Empty, 75);
            await client.GetGrain<IBankAccountGrain>(2).ModifyBalance(Guid.Empty, 75);

            await TransferAndWait(client, 1, 2, 25);
            await TransferAndWait(client, 2, 1, 20);
            // comment in to blow account ceiling and test compensation.
            //await TransferAndWait(client, 1, 2, 60);
        }

        private static async Task TransferAndWait(IClusterClient client, int from, int to, int amount)
        {
            var logger = client.Logger;

            await WaitForSaga(
                await client.GetGrain<ITransferGrain>(0).RequestTransfer(from, to, amount)
            );

            logger.Info("Account balances:");
            for (int accountId = 1; accountId <= 2; accountId++)
            {
                var account = client.GetGrain<IBankAccountGrain>(accountId);
                logger.Info($"  #{accountId} : {await account.GetBalance()}");
            }
        }

        static async Task DukeSample(IClusterClient client)
        {
            await new List<ISagaGrain>
            {
                await client.GetGrain<IDukeGrain>(0).Execute(),
                await client.GetGrain<IDukeGrain>(1).ExecuteAndAbort(),
                await client.GetGrain<IDukeGrain>(2).AbortWithoutExecution(),
                await client.GetGrain<IDukeGrain>(3).AbortThenExecute()
            }.Wait();
        }

        static async Task TravelSample(IClusterClient client)
        {
            await WaitForSaga(
                await client.GetGrain<IBookHolidayGrain>(Guid.Empty).Execute()
            );
        }

        static async Task ConcurrencySample(IClusterClient client)
        {
            var sagas = new List<ISagaGrain>();
            for (int i = 0; i < 20; i++)
            {
                sagas.Add(await client.GetGrain<IDukeGrain>(i).Execute());
            }

            await WaitForSagas(sagas);
        }

        static async Task WaitForSaga(ISagaGrain saga)
        {
            await WaitForSagas(new List<ISagaGrain> { saga });
        }

        static async Task WaitForSagas(List<ISagaGrain> sagas)
        {
            while (sagas.Count > 0)
            {
                var completed = new List<ISagaGrain>();
                
                foreach (var saga in sagas)
                {
                    if (await saga.HasCompleted())
                    {
                        completed.Add(saga);
                    }
                }

                sagas.RemoveAll(l => completed.Contains(l));

                await Task.Delay(1000);
            }
        }
    }
}
