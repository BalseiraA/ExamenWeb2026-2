using MediatR;
using TicketSystem.Application.Common.Exceptions;
using TicketSystem.Application.Common.Interfaces;
using TicketSystem.Application.Common.Mappings;
using TicketSystem.Application.DTOs;

namespace TicketSystem.Application.Features.Events.Queries.GetEventById;

public class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQuery, EventDto>
{
    private readonly IEventRepository _eventRepository;

    public GetEventByIdQueryHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<EventDto> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _eventRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("evento", request.Id);

        return entity.ToDto();
    }
}
