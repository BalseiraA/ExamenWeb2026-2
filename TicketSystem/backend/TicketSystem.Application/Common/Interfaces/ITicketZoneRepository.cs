using TicketSystem.Domain.Entities;

namespace TicketSystem.Application.Common.Interfaces;

public interface ITicketZoneRepository
{
    Task<TicketZone?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    void Update(TicketZone zone);
}
