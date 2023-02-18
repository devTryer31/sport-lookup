using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SportLookup.Backend.DataAccess.PostgreSQL;
using SportLookup.Backend.Infrastructure.Interfaces.DataAccess;
using SportLookup.Backend.WebAPI;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services, builder.Environment, builder.Configuration);

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

static void ConfigureServices(IServiceCollection services, IWebHostEnvironment env, IConfiguration configuration)
{
    services.AddCors();
    services.AddDbContext<IDbContext, AppDbContext>(cfg =>
    {
        cfg.UseNpgsql(configuration.GetConnectionString("MainPostgreSQL"));
    });

    services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", cfg =>
    {
        cfg.Authority = "https://localhost:5001";
        //TODO: Add UI client check.

        if (env.IsDevelopment())
            cfg.RequireHttpsMetadata = false;

        cfg.Audience = "main-sport-lookup-api-resource";
    });
    services.AddAuthorization(cfg =>
    {
        cfg.AddPolicy("MyPolicy", cfg =>
        {
            cfg.RequireClaim("myNewClaim", "123");
        });
    });

    services.AddControllers();
    services.AddApiVersioning(cfg => cfg.DefaultApiVersion = new ApiVersion(1,0));
    services.AddEndpointsApiExplorer();
    services.AddVersionedApiExplorer(cfg => cfg.GroupNameFormat = "'v'VVV");

    services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
    //services.AddTransient<IClaimsTransformation, MyClaimsTransformation>();

    services.AddSwaggerGen();
}