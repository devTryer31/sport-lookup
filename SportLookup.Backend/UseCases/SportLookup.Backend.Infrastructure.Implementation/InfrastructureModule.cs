using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SportLookup.Backend.Infrastructure.Implementation.PipelineHandlers;

namespace SportLookup.Backend.Infrastructure.Implementation;

public class InfrastructureModule : Utils.Module
{
    public override void Register(IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineHandler<,>));
    }
}
