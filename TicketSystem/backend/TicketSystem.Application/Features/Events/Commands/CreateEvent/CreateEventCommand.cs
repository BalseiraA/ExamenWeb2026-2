using MediatR;
using TicketSystem.Application.DTOs;

namespace TicketSystem.Application.Features.Events.Commands.CreateEvent;

public record CreateEventCommand(
    string Name,
    string Description,
    DateTime Date,
    string Venue,
    List<CreateZoneDto> Zones) : IRequest<EventDto>;

public record CreateZoneDto(
    string ZoneType,
    decimal Price,
    int TotalCapacity);
