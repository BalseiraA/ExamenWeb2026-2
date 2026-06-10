using MediatR;
using Microsoft.AspNetCore.Mvc;
using TicketSystem.Application.Features.Auth.Login;

namespace TicketSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ISender _sender;

    public AuthController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>Autentica al administrador y devuelve un token JWT.</summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await _sender.Send(command);
        return Ok(result);
    }
}
