using TicketSystem.Application.DTOs;
using TicketSystem.Domain.Entities;

namespace TicketSystem.Application.Common.Mappings;

public static class EventMappings
{
    public static EventDto ToDto(this Event entity) => new(
        entity.Id,
        entity.Name,
        entity.Description,
        entity.Date,
        entity.Venue,
        entity.Status.ToString(),
        entity.CreatedAt,
        entity.Zones
            .OrderBy(z => z.ZoneType)
            .Select(z => z.ToDto())
            .ToList());

    public static ZoneDto ToDto(this TicketZone zone) => new(
        zone.Id,
        zone.ZoneType.ToString(),
        zone.Price,
        zone.TotalCapacity,
        zone.AvailableCapacity);

    public static PurchaseDto ToDto(this TicketPurchase purchase) => new(
        purchase.Id,
        purchase.EventId,
        purchase.ZoneId,
        purchase.Zone?.ZoneType.ToString() ?? string.Empty,
        purchase.BuyerName,
        purchase.BuyerEmail,
        purchase.Quantity,
        purchase.TotalAmount,
        purchase.PurchasedAt);
}
