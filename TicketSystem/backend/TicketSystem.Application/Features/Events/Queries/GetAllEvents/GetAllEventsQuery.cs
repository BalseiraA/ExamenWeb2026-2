using MediatR;
using TicketSystem.Application.DTOs;

namespace TicketSystem.Application.Features.Events.Queries.GetAllEvents;

public record GetAllEventsQuery(
    string? Search = null,
    string? Status = null,
    DateTime? DateFrom = null,
    DateTime? DateTo = null) : IRequest<List<EventDto>>;
