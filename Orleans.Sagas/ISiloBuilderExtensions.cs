using Microsoft.Extensions.DependencyInjection;
using Orleans.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Orleans.Sagas
{
    public static class ISiloBuilderExtensions
    {
        public static ISiloBuilder UseSagas(this ISiloBuilder builder)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var interfaceType = typeof(IActivity);

            var activityTypes = new List<Type>();

            foreach (var assembly in assemblies)
            {
                var assemblyTypes = assembly.GetTypes();
                var assemblyActivityTypes = assemblyTypes
                    .Where(x => interfaceType.IsAssignableFrom(x))
                    .Where(x => !x.IsInterface)
                    .Where(x => !x.IsAbstract);

                activityTypes.AddRange(assemblyActivityTypes);
            }

            builder.ConfigureServices(services =>
            {
                activityTypes.ForEach(x => services.AddTransient(x));
            });

            return builder;
        }
    }
}
