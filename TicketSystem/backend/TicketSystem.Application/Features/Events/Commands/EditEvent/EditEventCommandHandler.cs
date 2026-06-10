using MediatR;
using TicketSystem.Application.Common.Exceptions;
using TicketSystem.Application.Common.Interfaces;
using TicketSystem.Application.Common.Mappings;
using TicketSystem.Application.DTOs;

namespace TicketSystem.Application.Features.Events.Commands.EditEvent;

public class EditEventCommandHandler : IRequestHandler<EditEventCommand, EventDto>
{
    private readonly IEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EditEventCommandHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork)
    {
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<EventDto> Handle(EditEventCommand request, CancellationToken cancellationToken)
    {
        var existing = await _eventRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("evento", request.Id);

        existing.Name = request.Name;
        existing.Description = request.Description;
        existing.Date = request.Date;
        existing.Venue = request.Venue;

        foreach (var zoneUpdate in request.Zones)
        {
            var zone = existing.Zones.FirstOrDefault(z => z.Id == zoneUpdate.Id)
                ?? throw new NotFoundException($"La zona con id '{zoneUpdate.Id}' no pertenece al evento.");

            // Boletos ya vendidos = capacidad total previa - capacidad disponible previa.
            var sold = zone.TotalCapacity - zone.AvailableCapacity;

            if (zoneUpdate.TotalCapacity < sold)
                throw new BusinessRuleException(
                    $"La capacidad de la zona {zone.ZoneType} no puede ser menor a los {sold} boletos ya vendidos.");

            zone.Price = zoneUpdate.Price;
            zone.TotalCapacity = zoneUpdate.TotalCapacity;
            zone.AvailableCapacity = zoneUpdate.TotalCapacity - sold;
        }

        _eventRepository.Update(existing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return existing.ToDto();
    }
}
