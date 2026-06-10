namespace TicketSystem.Application.DTOs;

public record SalesDashboardDto(
    int TotalEvents,
    int TotalTicketsSold,
    decimal TotalSales,
    List<EventRevenueDto> RevenueByEvent,
    List<ZoneSalesDto> TicketsByZone);

public record EventRevenueDto(
    int EventId,
    string EventName,
    int TicketsSold,
    decimal Revenue);

public record ZoneSalesDto(
    string ZoneType,
    int TicketsSold,
    decimal Revenue);
