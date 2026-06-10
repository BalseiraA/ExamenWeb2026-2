using MediatR;

namespace TicketSystem.Application.Features.Events.Commands.CancelEvent;

public record CancelEventCommand(int Id) : IRequest;
