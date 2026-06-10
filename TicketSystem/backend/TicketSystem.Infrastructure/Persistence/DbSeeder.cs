using Microsoft.EntityFrameworkCore;
using TicketSystem.Domain.Entities;
using TicketSystem.Domain.Enums;

namespace TicketSystem.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.Events.AnyAsync()) return;

        var now = DateTime.UtcNow;

        var events = new List<Event>
        {
            new()
            {
                Name = "Concierto Rock Sinfónico",
                Description = "Una noche de rock acompañado por una orquesta sinfónica en vivo.",
                Date = now.AddMonths(1),
                Venue = "Auditorio Nacional",
                Status = EventStatus.Active,
                CreatedAt = now,
                Zones = new List<TicketZone>
                {
                    NewZone(ZoneType.VIP, 1500m, 100),
                    NewZone(ZoneType.Preferente, 900m, 300),
                    NewZone(ZoneType.General, 450m, 1000)
                }
            },
            new()
            {
                Name = "Festival de Jazz",
                Description = "Festival con los mejores exponentes del jazz nacional e internacional.",
                Date = now.AddMonths(2),
                Venue = "Teatro Metropólitan",
                Status = EventStatus.Active,
                CreatedAt = now,
                Zones = new List<TicketZone>
                {
                    NewZone(ZoneType.VIP, 1200m, 80),
                    NewZone(ZoneType.Preferente, 750m, 200),
                    NewZone(ZoneType.General, 350m, 500)
                }
            },
            new()
            {
                Name = "Obra de Teatro: El Quijote",
                Description = "Adaptación contemporánea del clásico de Cervantes.",
                Date = now.AddMonths(3),
                Venue = "Teatro de la Ciudad",
                Status = EventStatus.Active,
                CreatedAt = now,
                Zones = new List<TicketZone>
                {
                    NewZone(ZoneType.VIP, 800m, 50),
                    NewZone(ZoneType.Preferente, 500m, 150),
                    NewZone(ZoneType.General, 250m, 400)
                }
            }
        };

        await context.Events.AddRangeAsync(events);
        await context.SaveChangesAsync();
    }

    private static TicketZone NewZone(ZoneType type, decimal price, int capacity) => new()
    {
        ZoneType = type,
        Price = price,
        TotalCapacity = capacity,
        AvailableCapacity = capacity
    };
}
