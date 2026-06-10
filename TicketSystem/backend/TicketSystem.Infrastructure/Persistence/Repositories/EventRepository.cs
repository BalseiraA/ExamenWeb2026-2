using Microsoft.EntityFrameworkCore;
using TicketSystem.Application.Common.Interfaces;
using TicketSystem.Domain.Entities;
using TicketSystem.Domain.Enums;

namespace TicketSystem.Infrastructure.Persistence.Repositories;

public class EventRepository : IEventRepository
{
    private readonly AppDbContext _context;

    public EventRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Event>> GetAllAsync(string? search, EventStatus? status, DateTime? dateFrom, DateTime? dateTo, CancellationToken cancellationToken = default)
    {
        var query = _context.Events
            .Include(e => e.Zones)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(e =>
                e.Name.ToLower().Contains(term) ||
                e.Venue.ToLower().Contains(term));
        }

        if (status.HasValue)
            query = query.Where(e => e.Status == status.Value);

        if (dateFrom.HasValue)
            query = query.Where(e => e.Date >= dateFrom.Value);

        if (dateTo.HasValue)
            query = query.Where(e => e.Date <= dateTo.Value);

        return await query
            .OrderBy(e => e.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<Event?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Events
            .Include(e => e.Zones)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task AddAsync(Event entity, CancellationToken cancellationToken = default)
    {
        await _context.Events.AddAsync(entity, cancellationToken);
    }

    public void Update(Event entity)
    {
        _context.Events.Update(entity);
    }
}
