using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using WScoreInfrastructure.Data;

namespace WScoreTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        builder.ConfigureServices(services =>
        {
            // Remove DbContext existente
            var dbCtxDescriptors = services
                .Where(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>))
                .ToList();
            foreach (var d in dbCtxDescriptors) services.Remove(d);

            // Remove provedores Oracle (se tiver algo registrado)
            var oracleRegs = services
                .Where(s => s.ImplementationType?.Namespace?.Contains("Oracle") == true)
                .ToList();
            foreach (var s in oracleRegs) services.Remove(s);

            // Adiciona InMemory para testes
            services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("TestDb"));

            // Garante DB limpo
            using var scope = services.BuildServiceProvider().CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        });
    }
}
