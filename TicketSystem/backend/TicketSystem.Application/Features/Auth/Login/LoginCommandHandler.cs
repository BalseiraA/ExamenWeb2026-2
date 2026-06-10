using MediatR;
using TicketSystem.Application.Common.Interfaces;

namespace TicketSystem.Application.Features.Auth.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    // Usuario administrador hardcodeado (sistema sin registro de usuarios).
    private const string AdminEmail = "admin@tickets.com";
    private const string AdminPassword = "Admin123!";
    private const string AdminRole = "Admin";

    private readonly IJwtTokenService _jwtTokenService;

    public LoginCommandHandler(IJwtTokenService jwtTokenService)
    {
        _jwtTokenService = jwtTokenService;
    }

    public Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var isValid =
            string.Equals(request.Email, AdminEmail, StringComparison.OrdinalIgnoreCase) &&
            request.Password == AdminPassword;

        if (!isValid)
            throw new UnauthorizedAccessException("Credenciales inválidas.");

        var token = _jwtTokenService.GenerateToken(AdminEmail, AdminRole);
        return Task.FromResult(new LoginResponse(token, AdminEmail, AdminRole));
    }
}
