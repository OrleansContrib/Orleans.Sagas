using Orleans.ApplicationParts;
using System.Reflection;

namespace Orleans.Sagas
{
    public static class ApplicationPartManagerExtensions
    {
        public static void AddSagaParts(this IApplicationPartManager manager)
        {
            manager.AddFrameworkPart(typeof(SagaGrain).Assembly).WithReferences();
        }
    }
}
