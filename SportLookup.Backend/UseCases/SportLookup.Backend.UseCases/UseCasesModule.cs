using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SportLookup.Backend.UseCases;

public class UseCasesModule : Utils.Module
{
    public override void Register(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<UseCasesModule>());
        services.AddValidatorsFromAssemblyContaining<UseCasesModule>();

        var profilesTypes = typeof(UseCasesModule)
            .Assembly
            .GetExportedTypes()
            .Where(t => t.BaseType == typeof(Profile))
            .Select(t => Activator.CreateInstance(t) as Profile);

        services.AddAutoMapper(cfg => cfg.AddProfiles(profilesTypes));
    }
}
