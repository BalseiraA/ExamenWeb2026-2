using MediatR;
using Microsoft.AspNetCore.Mvc;
using TicketSystem.Application.Features.Purchases.Commands.PurchaseTickets;
using TicketSystem.Application.Features.Purchases.Queries.GetPurchasesByEvent;

namespace TicketSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PurchasesController : ControllerBase
{
    private readonly ISender _sender;

    public PurchasesController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>Registra una compra de boletos. Público.</summary>
    [HttpPost]
    public async Task<IActionResult> Purchase([FromBody] PurchaseTicketsCommand command)
    {
        var result = await _sender.Send(command);
        return Ok(result);
    }

    /// <summary>Lista las compras de un evento. Público.</summary>
    [HttpGet("event/{eventId:int}")]
    public async Task<IActionResult> GetByEvent(int eventId)
    {
        var result = await _sender.Send(new GetPurchasesByEventQuery(eventId));
        return Ok(result);
    }
}
