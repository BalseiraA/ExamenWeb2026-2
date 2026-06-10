using MediatR;
using TicketSystem.Application.Common.Interfaces;
using TicketSystem.Application.Common.Mappings;
using TicketSystem.Application.DTOs;
using TicketSystem.Domain.Entities;
using TicketSystem.Domain.Enums;

namespace TicketSystem.Application.Features.Events.Commands.CreateEvent;

public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, EventDto>
{
    private readonly IEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateEventCommandHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork)
    {
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<EventDto> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var newEvent = new Event
        {
            Name = request.Name,
            Description = request.Description,
            Date = request.Date,
            Venue = request.Venue,
            Status = EventStatus.Active,
            CreatedAt = DateTime.UtcNow,
            Zones = request.Zones.Select(z =>
            {
                var zoneType = Enum.Parse<ZoneType>(z.ZoneType, ignoreCase: true);
                return new TicketZone
                {
                    ZoneType = zoneType,
                    Price = z.Price,
                    TotalCapacity = z.TotalCapacity,
                    AvailableCapacity = z.TotalCapacity
                };
            }).ToList()
        };

        await _eventRepository.AddAsync(newEvent, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return newEvent.ToDto();
    }
}
