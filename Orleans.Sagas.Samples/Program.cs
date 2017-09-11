using Orleans.Sagas.Samples.Examples;
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

            var samples = new List<Sample>{
                new BankTransferSample(),
                new DukeSample(),
                new TravelSample(),
                new ConcurrencySample()
            };

            foreach (var sample in samples)
            {
                sample.Initialize(client);
                await sample.Execute();
            }
        }
    }
}
