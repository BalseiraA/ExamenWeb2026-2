using Microsoft.EntityFrameworkCore;
using TicketSystem.Application.Common.Interfaces;
using TicketSystem.Domain.Entities;

namespace TicketSystem.Infrastructure.Persistence.Repositories;

public class TicketPurchaseRepository : ITicketPurchaseRepository
{
    private readonly AppDbContext _context;

    public TicketPurchaseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(TicketPurchase purchase, CancellationToken cancellationToken = default)
    {
        await _context.TicketPurchases.AddAsync(purchase, cancellationToken);
    }

    public async Task<List<TicketPurchase>> GetByEventAsync(int eventId, CancellationToken cancellationToken = default)
    {
        return await _context.TicketPurchases
            .Include(p => p.Zone)
            .Where(p => p.EventId == eventId)
            .OrderByDescending(p => p.PurchasedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<TicketPurchase>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.TicketPurchases
            .Include(p => p.Zone)
            .ToListAsync(cancellationToken);
    }
}
