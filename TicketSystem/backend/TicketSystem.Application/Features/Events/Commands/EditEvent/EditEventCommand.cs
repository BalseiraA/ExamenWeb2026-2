using MediatR;
using TicketSystem.Application.DTOs;

namespace TicketSystem.Application.Features.Events.Commands.EditEvent;

public record EditEventCommand(
    int Id,
    string Name,
    string Description,
    DateTime Date,
    string Venue,
    List<EditZoneDto> Zones) : IRequest<EventDto>;

public record EditZoneDto(
    int Id,
    decimal Price,
    int TotalCapacity);
