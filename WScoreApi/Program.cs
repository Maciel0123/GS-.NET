using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Logs;
using Serilog;
using WScoreInfrastructure.Data;
using WScoreBusiness;
using WScoreApi.Swagger;

var builder = WebApplication.CreateBuilder(args);

// ===============================
// SERILOG
// ===============================
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

// ===============================
// OPEN TELEMETRY
// ===============================
builder.Services.AddOpenTelemetry()
    .WithTracing(t =>
    {
        t.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("WScoreApi"))
         .AddAspNetCoreInstrumentation()
         .AddHttpClientInstrumentation()
         .AddEntityFrameworkCoreInstrumentation()
         .AddConsoleExporter();
    })
    .WithLogging(l =>
    {
        l.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("WScoreApi"))
         .AddConsoleExporter();
    });

// ===============================
// CONTROLLERS
// ===============================
builder.Services.AddControllers();

// ===============================
// API VERSIONING
// ===============================
builder.Services.AddApiVersioning(o =>
{
    o.DefaultApiVersion = new ApiVersion(1, 0);
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.ReportApiVersions = true;
})
.AddApiExplorer(o =>
{
    o.GroupNameFormat = "'v'VVV";
    o.SubstituteApiVersionInUrl = true;
});

// ===============================
// SWAGGER (não carregar no Testing)
// ===============================
builder.Services.AddEndpointsApiExplorer();

if (!builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddSwaggerGen();
    builder.Services.ConfigureOptions<SwaggerOptionsConfig>();
}

// ===============================
// DATABASE (Oracle) — não usar no Testing
// ===============================
if (!builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));
}

// ===============================
// BUSINESS SERVICES
// ===============================
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICheckinService, CheckinService>();

// ===============================
// HEALTHCHECK
// ===============================
builder.Services.AddHealthChecks();

var app = builder.Build();

// ===============================
// SWAGGER (somente DEV, nunca Testing)
// ===============================
if (app.Environment.IsDevelopment() && !app.Environment.IsEnvironment("Testing"))
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var desc in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", desc.GroupName);
        }
    });
}

// ===============================
// HEALTH
// ===============================
app.MapHealthChecks("/health").AllowAnonymous();

// ===============================
// MAP CONTROLLERS
// ===============================
app.MapControllers();

app.Run();

// NECESSÁRIO PARA TESTES
public partial class Program { }
