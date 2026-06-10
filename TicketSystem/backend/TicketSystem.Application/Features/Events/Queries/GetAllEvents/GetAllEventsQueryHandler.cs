using MediatR;
using TicketSystem.Application.Common.Interfaces;
using TicketSystem.Application.Common.Mappings;
using TicketSystem.Application.DTOs;
using TicketSystem.Domain.Enums;

namespace TicketSystem.Application.Features.Events.Queries.GetAllEvents;

public class GetAllEventsQueryHandler : IRequestHandler<GetAllEventsQuery, List<EventDto>>
{
    private readonly IEventRepository _eventRepository;

    public GetAllEventsQueryHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<List<EventDto>> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
    {
        EventStatus? status = null;
        if (!string.IsNullOrWhiteSpace(request.Status) &&
            Enum.TryParse<EventStatus>(request.Status, ignoreCase: true, out var parsed))
        {
            status = parsed;
        }

        var events = await _eventRepository.GetAllAsync(
            request.Search,
            status,
            request.DateFrom,
            request.DateTo,
            cancellationToken);

        return events.Select(e => e.ToDto()).ToList();
    }
}
