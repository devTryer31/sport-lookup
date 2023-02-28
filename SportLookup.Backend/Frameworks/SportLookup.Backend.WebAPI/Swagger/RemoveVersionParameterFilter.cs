using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SportLookup.Backend.WebAPI.Swagger;

public class RemoveVersionParameterFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var versionParam = operation.Parameters.FirstOrDefault(p => p.Name == "version");
        operation.Parameters.Remove(versionParam);
    }
}
