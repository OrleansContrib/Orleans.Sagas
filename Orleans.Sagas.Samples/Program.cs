using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Sagas.Samples.Activities;
using Orleans.Sagas.Samples.Activities.Grains;
using Orleans.Sagas.Samples.Examples;
using System.Net;
using System.Net.Http;
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
                        .UseSagas(typeof(BalanceModificationActivity).Assembly)
                        .ConfigureLogging(logging =>
                        {
                            logging.AddConsole();
                        })
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
                            services.AddSingleton<HttpClient>();

                            services.AddTransient<AbortSample>();
                            services.AddTransient<DependencyInjectionSample>();
                            services.AddTransient<BankTransferSample>();
                            services.AddTransient<DukeSample>();
                            services.AddTransient<TravelSample>();
                            services.AddTransient<ConcurrencySample>();
                            services.AddTransient<ErrorReportSample>();

                            services.AddHostedService<SampleRunner>();
                        })
                       //.AddAdoNetGrainStorageAsDefault(opts =>
                       //{
                       //    opts.Invariant = "System.Data.SqlClient";
                       //    opts.ConnectionString = "Server=.;Database=Orleans.Sagas;Integrated Security=true;";
                       //    //opts.UseJsonFormat = true;
                       //})
                       // .UseAdoNetReminderService(opts =>
                       // {
                       //     opts.Invariant = "System.Data.SqlClient";
                       //     opts.ConnectionString = "Server=.;Database=Orleans.Sagas;Integrated Security=true;";
                       // });
                       .AddMemoryGrainStorageAsDefault()
                       .UseInMemoryReminderService();
                })
                .RunConsoleAsync();
        }
    }
}
