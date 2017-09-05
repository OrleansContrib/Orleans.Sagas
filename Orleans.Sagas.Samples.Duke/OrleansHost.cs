using Orleans.Runtime;
using Orleans.Runtime.Configuration;
using Orleans.Runtime.Host;
using Orleans.Storage;
using System;
using System.Net;
using System.Reflection;

namespace Orleans.Sagas.Samples.Duke
{
    public class OrleansHost
    {
        private SiloHost siloHost;

        public ClusterConfiguration Config { get; private set; }

        public OrleansHost()
        {
            Config = new ClusterConfiguration();
        }

        public MemoryStorage Storage { get; set; }

        public void Run()
        {
            var args = new string[0];
            var siloArgs = SiloArgs.ParseArguments(args);
            this.siloHost = new SiloHost(siloArgs.SiloName, Config);
            this.siloHost.LoadOrleansConfig();

            if (this.siloHost == null)
            {
                SiloArgs.PrintUsage();
                throw new ArgumentNullException(nameof(siloHost));
            }

            try
            {
                this.siloHost.InitializeOrleansSilo();

                if (this.siloHost.StartOrleansSilo())
                {
                    Console.WriteLine($"Successfully started Orleans silo '{this.siloHost.Name}' as a {this.siloHost.Type} node.");
                }
                else
                {
                    throw new OrleansException($"Failed to start Orleans silo '{this.siloHost.Name}' as a {this.siloHost.Type} node.");
                }
            }
            catch (Exception exc)
            {
                this.siloHost.ReportStartupError(exc);
                Console.Error.WriteLine(exc);
                throw new OrleansException($"Failed to start Orleans silo '{this.siloHost.Name}' as a {this.siloHost.Type} node.", exc);
            }
        }

        public int Stop()
        {
            if (this.siloHost != null)
            {
                try
                {
                    this.siloHost.StopOrleansSilo();
                    this.siloHost.Dispose();
                    Console.WriteLine($"Orleans silo '{this.siloHost.Name}' shutdown.");
                }
                catch (Exception exc)
                {
                    this.siloHost.ReportStartupError(exc);
                    Console.Error.WriteLine(exc);
                    return 1;
                }
            }

            return 0;
        }

        private class SiloArgs
        {
            public SiloArgs(string siloName, string deploymentId)
            {
                this.DeploymentId = deploymentId;
                this.SiloName = siloName;
            }

            public string DeploymentId { get; set; }

            public string SiloName { get; set; }

            public static SiloArgs ParseArguments(string[] args)
            {
                string deploymentId = null;
                string siloName = null;

                for (int i = 0; i < args.Length; i++)
                {
                    string arg = args[i];
                    if (arg.StartsWith("-") || arg.StartsWith("/"))
                    {
                        switch (arg.ToLowerInvariant())
                        {
                            case "/?":
                            case "/help":
                            case "-?":
                            case "-help":
                                // Query usage help. Return null so that usage is printed
                                return null;
                            default:
                                Console.WriteLine($"Bad command line arguments supplied: {arg}");
                                return null;
                        }
                    }
                    else if (arg.Contains("="))
                    {
                        string[] parameters = arg.Split('=');
                        if (string.IsNullOrEmpty(parameters[1]))
                        {
                            Console.WriteLine($"Bad command line arguments supplied: {arg}");
                            return null;
                        }

                        switch (parameters[0].ToLowerInvariant())
                        {
                            case "deploymentid":
                                deploymentId = parameters[1];
                                break;
                            case "name":
                                siloName = parameters[1];
                                break;
                            default:
                                Console.WriteLine($"Bad command line arguments supplied: {arg}");
                                return null;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Bad command line arguments supplied: {arg}");
                        return null;
                    }
                }

                // Default to machine name
                siloName = siloName ?? Dns.GetHostName();
                return new SiloArgs(siloName, deploymentId);
            }

            public static void PrintUsage()
            {
                string consoleAppName = typeof(SiloArgs).GetTypeInfo().Assembly.GetName().Name;
                Console.WriteLine(
                    $@"USAGE: {consoleAppName} [name=<siloName>] [deploymentId=<idString>] [/debug]
                Where:
                name=<siloName> - Name of this silo (optional)
                deploymentId=<idString> - Optionally override the deployment group this host instance should run in 
                (otherwise will use the one in the configuration");
            }
        }
    }
}