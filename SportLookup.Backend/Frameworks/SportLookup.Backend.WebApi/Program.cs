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
        cfg.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });
}


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

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
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(cfg =>
    {
        string docXMLFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        cfg.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, docXMLFilename));
    });
}