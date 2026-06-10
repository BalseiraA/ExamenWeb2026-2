using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketSystem.Application.Features.Events.Commands.CancelEvent;
using TicketSystem.Application.Features.Events.Commands.CreateEvent;
using TicketSystem.Application.Features.Events.Commands.EditEvent;
using TicketSystem.Application.Features.Events.Queries.GetAllEvents;
using TicketSystem.Application.Features.Events.Queries.GetEventById;

namespace TicketSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly ISender _sender;

    public EventsController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>Lista eventos con filtros opcionales. Público.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search,
        [FromQuery] string? status,
        [FromQuery] DateTime? dateFrom,
        [FromQuery] DateTime? dateTo)
    {
        var result = await _sender.Send(new GetAllEventsQuery(search, status, dateFrom, dateTo));
        return Ok(result);
    }

    /// <summary>Obtiene el detalle de un evento. Público.</summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _sender.Send(new GetEventByIdQuery(id));
        return Ok(result);
    }

    /// <summary>Crea un evento. Requiere rol Admin.</summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateEventCommand command)
    {
        var result = await _sender.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Edita un evento existente. Requiere rol Admin.</summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id, [FromBody] EditEventCommand command)
    {
        if (id != command.Id)
            return BadRequest("El id de la ruta no coincide con el del cuerpo.");

        var result = await _sender.Send(command);
        return Ok(result);
    }

    /// <summary>Cancela un evento. Requiere rol Admin.</summary>
    [HttpDelete("{id:int}/cancel")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Cancel(int id)
    {
        await _sender.Send(new CancelEventCommand(id));
        return NoContent();
    }
}
