using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SportLookup.Backend.WebAPI.Swagger;

public class ReplaceVersionWithExactPathFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        OpenApiPaths apiNewPaths = new();

        var replacedPaths = swaggerDoc.Paths
            .Select(pair => KeyValuePair.Create(pair.Key.Replace("v{version}", swaggerDoc.Info.Version), pair.Value));

        foreach (var (updatedPath, pathItem) in replacedPaths)
            apiNewPaths.Add(updatedPath, pathItem);

        swaggerDoc.Paths = apiNewPaths;
    }
}