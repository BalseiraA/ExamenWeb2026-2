using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TicketSystem.Infrastructure.Persistence;

/// <summary>
/// Permite a las herramientas de EF Core (dotnet ef migrations) crear el
/// contexto en tiempo de diseño sin necesidad de arrancar la API.
/// </summary>
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Data Source=ticketsystem.db")
            .Options;

        return new AppDbContext(options);
    }
}
