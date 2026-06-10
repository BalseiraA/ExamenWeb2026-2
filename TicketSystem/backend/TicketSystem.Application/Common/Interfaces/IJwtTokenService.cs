namespace TicketSystem.Application.Common.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(string email, string role);
}
