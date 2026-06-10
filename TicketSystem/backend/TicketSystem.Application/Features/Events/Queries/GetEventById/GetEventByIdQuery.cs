using MediatR;
using TicketSystem.Application.DTOs;

namespace TicketSystem.Application.Features.Events.Queries.GetEventById;

public record GetEventByIdQuery(int Id) : IRequest<EventDto>;
