using MediatR;
using TicketSystem.Application.Common.Exceptions;
using TicketSystem.Application.Common.Interfaces;
using TicketSystem.Application.Common.Mappings;
using TicketSystem.Application.DTOs;
using TicketSystem.Domain.Entities;
using TicketSystem.Domain.Enums;

namespace TicketSystem.Application.Features.Purchases.Commands.PurchaseTickets;

public class PurchaseTicketsCommandHandler : IRequestHandler<PurchaseTicketsCommand, PurchaseDto>
{
    private readonly IEventRepository _eventRepository;
    private readonly ITicketPurchaseRepository _purchaseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PurchaseTicketsCommandHandler(
        IEventRepository eventRepository,
        ITicketPurchaseRepository purchaseRepository,
        IUnitOfWork unitOfWork)
    {
        _eventRepository = eventRepository;
        _purchaseRepository = purchaseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PurchaseDto> Handle(PurchaseTicketsCommand request, CancellationToken cancellationToken)
    {
        var ev = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken)
            ?? throw new NotFoundException("evento", request.EventId);

        if (ev.Status == EventStatus.Cancelled)
            throw new BusinessRuleException("No se pueden comprar boletos para un evento cancelado.");

        var zone = ev.Zones.FirstOrDefault(z => z.Id == request.ZoneId)
            ?? throw new NotFoundException($"La zona con id '{request.ZoneId}' no pertenece al evento.");

        if (zone.AvailableCapacity < request.Quantity)
            throw new BusinessRuleException(
                $"No hay disponibilidad suficiente en la zona {zone.ZoneType}. Disponibles: {zone.AvailableCapacity}.");

        var total = zone.Price * request.Quantity;

        // Descuenta la capacidad disponible de la zona (entidad rastreada por EF).
        zone.AvailableCapacity -= request.Quantity;

        var purchase = new TicketPurchase
        {
            EventId = ev.Id,
            ZoneId = zone.Id,
            BuyerName = request.BuyerName,
            BuyerEmail = request.BuyerEmail,
            Quantity = request.Quantity,
            TotalAmount = total,
            PurchasedAt = DateTime.UtcNow
        };

        await _purchaseRepository.AddAsync(purchase, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Adjunta la zona para exponer su tipo en el DTO de respuesta.
        purchase.Zone = zone;
        return purchase.ToDto();
    }
}
