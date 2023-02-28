using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SportLookup.Backend.DataAccess.PostgreSQL;
using SportLookup.Backend.Entities.Configuration;
using SportLookup.Backend.Entities.Models.Auth;
using SportLookup.Backend.Infrastructure.Implementation;
using SportLookup.Backend.Infrastructure.Interfaces.DataAccess;
using SportLookup.Backend.UseCases;
using SportLookup.Backend.Utils;
using SportLookup.Backend.WebAPI.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
[assembly: ApiController]

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

app.UseApiVersioning();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

static void ConfigureServices(IServiceCollection services, IWebHostEnvironment env, IConfiguration configuration)
{
    var jwtConfigSection = configuration.GetSection("Jwt");
    var jwtConfiguration = new JWTConfig(
        key: jwtConfigSection.GetValue<string>("Key")!,
        apiAudience: jwtConfigSection.GetValue<string>("ApiAudience")!
    );
    services.AddSingleton(jwtConfiguration);

    services.AddCors();
    services.AddDbContext<IDbContext, AppDbContext>(cfg =>
    {
        cfg.UseNpgsql(configuration.GetConnectionString("MainPostgreSQL"));
    });
    services.AddIdentity<AppUser, AppUserRole>(cfg =>
    {
        cfg.SignIn.RequireConfirmedAccount = true;
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

    services.AddAuthentication(cfg =>
    {
        cfg.DefaultAuthenticateScheme = "Bearer";
        cfg.DefaultChallengeScheme = "Bearer";
    })
        .AddJwtBearer("Bearer", cfg =>
        {
            if (env.IsDevelopment())
                cfg.RequireHttpsMetadata = false;

            cfg.SaveToken = true;

            cfg.TokenValidationParameters = new()
            {
                ValidateAudience = true,
                ValidAudience = jwtConfiguration.ApiAudience,
                ValidateIssuer = true,
                ValidIssuer = "https://localhost:7053",
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(jwtConfiguration.Secret)
            };
        });

    services.AddControllers();
    services.AddApiVersioning(cfg =>
    {
        //cfg.ApiVersionReader = Reader
        cfg.AssumeDefaultVersionWhenUnspecified = true;
        cfg.ApiVersionSelector = new ConstantApiVersionSelector(new ApiVersion(1, 0));
        cfg.DefaultApiVersion = new ApiVersion(1, 0);
    });
    services.AddEndpointsApiExplorer();
    services.AddVersionedApiExplorer(cfg => cfg.GroupNameFormat = "'v'VVV");

    services.AddApplicationModule<InfrastructureModule>(configuration);
    services.AddApplicationModule<UseCasesModule>(configuration);

    services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
    //services.AddTransient<IClaimsTransformation, MyClaimsTransformation>();

    services.AddSwaggerGen(cfg =>
    {
        cfg.DocumentFilter<ReplaceVersionWithExactPathFilter>();
        cfg.OperationFilter<RemoveVersionParameterFilter>();
    });
}