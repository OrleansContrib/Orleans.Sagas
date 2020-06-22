using Microsoft.Extensions.DependencyInjection;
using Orleans.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Orleans.Sagas
{
    public static class ISiloBuilderExtensions
    {
        public static ISiloBuilder UseSagas(this ISiloBuilder builder, params Assembly[] activityAssemblies)
        {
            var assemblies = activityAssemblies is null
                ? AppDomain.CurrentDomain.GetAssemblies()
                : AppDomain.CurrentDomain.GetAssemblies().Concat(activityAssemblies);

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
