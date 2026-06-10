using TicketSystem.Domain.Entities;

namespace TicketSystem.Application.Common.Interfaces;

public interface ITicketPurchaseRepository
{
    Task AddAsync(TicketPurchase purchase, CancellationToken cancellationToken = default);
    Task<List<TicketPurchase>> GetByEventAsync(int eventId, CancellationToken cancellationToken = default);
    Task<List<TicketPurchase>> GetAllAsync(CancellationToken cancellationToken = default);
}
