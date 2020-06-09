using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Sagas.Samples.Examples;
using System.Net;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder(args)
                .UseOrleans(siloBuilder =>
                {
                    siloBuilder
                        .UseLocalhostClustering()
                        .Configure<ClusterOptions>(opts =>
                        {
                            opts.ClusterId = nameof(Sagas);
                            opts.ServiceId = nameof(Sagas);
                        })
                        .Configure<EndpointOptions>(opts =>
                        {
                            opts.AdvertisedIPAddress = IPAddress.Loopback;
                        })
                        .ConfigureServices(services =>
                        {
                            services.AddTransient<BankTransferSample>();
                            services.AddTransient<DukeSample>();
                            services.AddTransient<TravelSample>();
                            services.AddTransient<ConcurrencySample>();
                            services.AddHostedService<SampleRunner>();
                        })
                        .AddMemoryGrainStorageAsDefault();
                })
                .RunConsoleAsync();
        }
    }
}
