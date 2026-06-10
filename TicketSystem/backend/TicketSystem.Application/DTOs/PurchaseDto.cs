namespace TicketSystem.Application.DTOs;

public record PurchaseDto(
    int Id,
    int EventId,
    int ZoneId,
    string ZoneType,
    string BuyerName,
    string BuyerEmail,
    int Quantity,
    decimal TotalAmount,
    DateTime PurchasedAt);
