using TicketSystem.Domain.Entities;

namespace TicketSystem.Application.Common.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
}
