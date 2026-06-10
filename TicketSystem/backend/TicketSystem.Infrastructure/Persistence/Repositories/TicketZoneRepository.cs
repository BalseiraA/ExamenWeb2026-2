using Microsoft.EntityFrameworkCore;
using TicketSystem.Application.Common.Interfaces;
using TicketSystem.Domain.Entities;

namespace TicketSystem.Infrastructure.Persistence.Repositories;

public class TicketZoneRepository : ITicketZoneRepository
{
    private readonly AppDbContext _context;

    public TicketZoneRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TicketZone?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.TicketZones
            .FirstOrDefaultAsync(z => z.Id == id, cancellationToken);
    }

    public void Update(TicketZone zone)
    {
        _context.TicketZones.Update(zone);
    }
}
