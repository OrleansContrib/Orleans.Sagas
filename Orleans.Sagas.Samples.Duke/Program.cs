using Orleans.Runtime.Configuration;
using Orleans.Sagas.Samples.Duke.Interfaces;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Duke
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
            var client = await CreateOrleansAndClient();

            await client.GetGrain<IDukeGrain>(Guid.Empty).Go();
        }

        static async Task<IClusterClient> CreateOrleansAndClient()
        {
            var host = new OrleansHost();

            var config = host.Config;

            config.Defaults.SiloName = "default";
            config.Globals.LivenessType = GlobalConfiguration.LivenessProviderType.MembershipTableGrain;
            config.Globals.ReminderServiceType = GlobalConfiguration.ReminderServiceProviderType.ReminderTableGrain;
            config.Defaults.ProxyGatewayEndpoint = new IPEndPoint(IPAddress.Any, 30000);
            config.Defaults.Port = 11111;
            config.Globals.RegisterStorageProvider("Orleans.Storage.MemoryStorage", "Default");
            config.Defaults.HostNameOrIPAddress = "localhost";
            config.Globals.SeedNodes.Add(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11111));
            config.PrimaryNode = config.Globals.SeedNodes.First();

            host.Run();

            var clientConfig = new ClientConfiguration()
            {
                GatewayProvider = ClientConfiguration.GatewayProviderType.Config
            };

            var hostEntry = await Dns.GetHostEntryAsync("localhost");
            var address = hostEntry.AddressList.First(x => x.AddressFamily == AddressFamily.InterNetwork);
            clientConfig.Gateways.Add(new IPEndPoint(address, 30000));

            var client = new ClientBuilder().UseConfiguration(clientConfig).Build();

            await client.Connect();

            return client;
        }
    }
}