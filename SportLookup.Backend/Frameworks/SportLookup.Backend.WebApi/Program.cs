using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using SportLookup.Backend.WebAPI;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services, builder.Environment);

var app = builder.Build();


app.UseCors(cfg =>
{
    cfg.WithOrigins("http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
});

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(cfg =>
    {
        using var scope = app.Services.CreateScope();

        var apiVersionsDescrProvider = scope.ServiceProvider.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var description in apiVersionsDescrProvider.ApiVersionDescriptions)
            cfg.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName);
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseApiVersioning();

app.MapControllers();

app.Run();

static void ConfigureServices(IServiceCollection services, IWebHostEnvironment env)
{
    services.AddCors();

    services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", cfg =>
    {
        cfg.Authority = "https://localhost:5001";
        //TODO: Add UI client check.

        if (env.IsDevelopment())
            cfg.RequireHttpsMetadata = false;

        cfg.Audience = "main-sport-lookup-api-resource";
    });
    services.AddAuthorization();

    services.AddControllers();
    services.AddApiVersioning();
    services.AddEndpointsApiExplorer();
    services.AddVersionedApiExplorer(cfg => cfg.GroupNameFormat = "'v'VVV");

    services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

    services.AddSwaggerGen();
}