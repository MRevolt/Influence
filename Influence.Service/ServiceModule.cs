using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IocLoader = Influence.Service.Configuration.IocLoader;

namespace Influence.Service
{
    public static class ServiceModule
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Configure(IConfiguration configuration, IServiceCollection services)
        {
            IocLoader.UseIocLoader(services);
        }
    }
}
