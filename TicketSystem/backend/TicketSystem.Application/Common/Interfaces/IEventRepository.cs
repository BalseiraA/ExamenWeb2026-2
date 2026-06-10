using TicketSystem.Domain.Entities;
using TicketSystem.Domain.Enums;

namespace TicketSystem.Application.Common.Interfaces;

public interface IEventRepository
{
    Task<List<Event>> GetAllAsync(string? search, EventStatus? status, DateTime? dateFrom, DateTime? dateTo, CancellationToken cancellationToken = default);
    Task<Event?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task AddAsync(Event entity, CancellationToken cancellationToken = default);
    void Update(Event entity);
}
