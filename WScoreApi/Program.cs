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

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

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

builder.Services.AddControllers();

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

builder.Services.AddEndpointsApiExplorer();

if (!builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddSwaggerGen();
    builder.Services.ConfigureOptions<SwaggerOptionsConfig>();
}

if (!builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));
}

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICheckinService, CheckinService>();

builder.Services.AddHealthChecks();

var app = builder.Build();

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

app.MapHealthChecks("/health").AllowAnonymous();

app.MapControllers();

app.Run();

public partial class Program { }
