using MediatR;

namespace TicketSystem.Application.Features.Auth.Login;

public record LoginCommand(string Email, string Password) : IRequest<LoginResponse>;

public record LoginResponse(string Token, string Email, string Role);
