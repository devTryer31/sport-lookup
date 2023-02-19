using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SportLookup.Backend.Utils;

public abstract class Module
{
    public abstract void Register(IServiceCollection services, IConfiguration configuration);
}