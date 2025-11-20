using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using WScoreInfrastructure.Data;
using WScoreBusiness;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Versionamento (Asp.Versioning 8.1.0)
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
})
.AddMvc() // necessário na versão 8+ caso use controllers MVC
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// BD Oracle (somente fora do ambiente de teste)
if (!builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));
}

// Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICheckinService, CheckinService>();

// Swagger (normal)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger no DEV
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();

public partial class Program { }
