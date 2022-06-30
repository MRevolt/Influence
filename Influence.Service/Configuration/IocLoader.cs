using System.Reflection;
using Influence.Service.Dependencies;
using Microsoft.Extensions.DependencyInjection;

namespace Influence.Service.Configuration
{
    public static class IocLoader
    {
        public static void UseIocLoader(this IServiceCollection serviceCollection)
        {
            var transientType = typeof(ITransientService);
            var scopedType = typeof(IScopedService);
            var singletonType = typeof(ISingletonService);

            var services = Assembly.GetCallingAssembly()
                .GetTypes()
                .Where(p =>
                    transientType.IsAssignableFrom(p) ||
                    scopedType.IsAssignableFrom(p) ||
                    singletonType.IsAssignableFrom(p)
                );

            foreach (var service in services)
            {
                var interfaceOfService = service.GetInterfaces()
                    .FirstOrDefault(x => x != transientType && x != scopedType && x != singletonType);

                if (interfaceOfService == null)
                    continue;

                if (transientType.IsAssignableFrom(service))
                {
                    serviceCollection.AddTransient(interfaceOfService, service);
                }

                else if (scopedType.IsAssignableFrom(service))
                {
                    serviceCollection.AddScoped(interfaceOfService, service);
                }

                else if (singletonType.IsAssignableFrom(service))
                {
                    serviceCollection.AddSingleton(interfaceOfService, service);
                }
            }
        }
    }
}