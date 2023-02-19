using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SportLookup.Backend.Utils;

public static class ModuleRegistrationExtensions
{
    public static IServiceCollection AddApplicationModule<TModule>(this IServiceCollection services, IConfiguration configuration)
        where TModule : Module
    {

        Activator.CreateInstance<TModule>().Register(services, configuration);

        return services;
    }
}
