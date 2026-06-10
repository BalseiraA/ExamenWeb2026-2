using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketSystem.Application.Features.Dashboard.Queries.GetSalesDashboard;

namespace TicketSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class DashboardController : ControllerBase
{
    private readonly ISender _sender;

    public DashboardController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>Métricas de ventas para el panel de administración. Requiere rol Admin.</summary>
    [HttpGet("sales")]
    public async Task<IActionResult> GetSales()
    {
        var result = await _sender.Send(new GetSalesDashboardQuery());
        return Ok(result);
    }
}
