using Orleans.Sagas.Samples.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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

            await DukeSample(client);
            await TravelSample(client);
            await ConcurrencySample(client);
        }

        static async Task DukeSample(IClusterClient client)
        {
            await WaitForSagasToComplete(new List<ISagaGrain>
            {
                await client.GetGrain<IDukeGrain>(0).Execute(),
                await client.GetGrain<IDukeGrain>(1).ExecuteAndAbort(),
                await client.GetGrain<IDukeGrain>(2).AbortWithoutExecution(),
                await client.GetGrain<IDukeGrain>(3).AbortThenExecute()
            });
        }

        static async Task TravelSample(IClusterClient client)
        {
            await WaitForSagasToComplete(new List<ISagaGrain>
            {
                await client.GetGrain<IBookHolidayGrain>(Guid.Empty).Execute()
            });
        }

        static async Task ConcurrencySample(IClusterClient client)
        {
            var sagas = new List<ISagaGrain>();
            for (int i = 0; i < 10; i++)
            {
                sagas.Add(await client.GetGrain<IDukeGrain>(i).Execute());
            }

            await WaitForSagasToComplete(sagas);
        }

        static async Task WaitForSagasToComplete(List<ISagaGrain> sagas)
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
