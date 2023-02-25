using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SportLookup.Backend.DataAccess.PostgreSQL;
using SportLookup.Backend.Entities.Configuration;
using SportLookup.Backend.Entities.Models.Auth;
using SportLookup.Backend.Infrastructure.Implementation;
using SportLookup.Backend.Infrastructure.Interfaces.DataAccess;
using SportLookup.Backend.UseCases;
using SportLookup.Backend.Utils;
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
    services.AddSingleton(new JWTConfig(configuration.GetSection("Jwt").GetValue<string>("Key")!));

    services.AddCors();
    services.AddDbContext<IDbContext, AppDbContext>(cfg =>
    {
        cfg.UseNpgsql(configuration.GetConnectionString("MainPostgreSQL"));
    });
    services.AddIdentity<AppUser, AppUserRole>(cfg =>
    {
        var passCfg = cfg.Password;
        if (env.IsDevelopment())
        {
            passCfg.RequireNonAlphanumeric = false;
            passCfg.RequireDigit = false;
            passCfg.RequireUppercase = false;
            passCfg.RequireLowercase = false;
        }
    })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

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
    services.AddApiVersioning(cfg => cfg.DefaultApiVersion = new ApiVersion(1, 0));
    services.AddEndpointsApiExplorer();
    services.AddVersionedApiExplorer(cfg => cfg.GroupNameFormat = "'v'VVV");

    services.AddApplicationModule<InfrastructureModule>(configuration);
    services.AddApplicationModule<UseCasesModule>(configuration);

    services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
    //services.AddTransient<IClaimsTransformation, MyClaimsTransformation>();

    services.AddSwaggerGen();
}