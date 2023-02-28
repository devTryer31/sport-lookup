using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace SportLookup.Backend.WebAPI.Swagger;

internal class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions> //TODO: move to Infrastructure.Implementation
{
    private readonly IApiVersionDescriptionProvider _apiDescriptionProvider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider apiDescriptionProvider)
        => _apiDescriptionProvider = apiDescriptionProvider;

    public void Configure(SwaggerGenOptions options)
    {
        string docXMLFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, docXMLFilename));

        foreach (var description in _apiDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, new OpenApiInfo
            {
                Contact = new OpenApiContact { Email = "mietvalera@gmail.com", Name = "Valeriy" },
                Title = "Sport LookUp API",
                Description = "API for Sport LookUp web app service",
                License = new OpenApiLicense { Name = "GNU" },
                Version = $"v{description.ApiVersion.MajorVersion}",
            });

            string authName = $"Auth token for api version {description.ApiVersion}";
            options.AddSecurityDefinition(authName, new OpenApiSecurityScheme
            {
                Scheme = "Bearer",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Name = "Authorization",
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id=authName
                        }
                    },
                    Array.Empty<string>()
                }
            });
        }
    }
}