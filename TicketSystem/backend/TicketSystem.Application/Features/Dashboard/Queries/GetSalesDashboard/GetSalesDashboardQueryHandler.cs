using MediatR;
using TicketSystem.Application.Common.Interfaces;
using TicketSystem.Application.DTOs;

namespace TicketSystem.Application.Features.Dashboard.Queries.GetSalesDashboard;

public class GetSalesDashboardQueryHandler : IRequestHandler<GetSalesDashboardQuery, SalesDashboardDto>
{
    private readonly ITicketPurchaseRepository _purchaseRepository;
    private readonly IEventRepository _eventRepository;

    public GetSalesDashboardQueryHandler(
        ITicketPurchaseRepository purchaseRepository,
        IEventRepository eventRepository)
    {
        _purchaseRepository = purchaseRepository;
        _eventRepository = eventRepository;
    }

    public async Task<SalesDashboardDto> Handle(GetSalesDashboardQuery request, CancellationToken cancellationToken)
    {
        var purchases = await _purchaseRepository.GetAllAsync(cancellationToken);
        var events = await _eventRepository.GetAllAsync(null, null, null, null, cancellationToken);

        var totalTicketsSold = purchases.Sum(p => p.Quantity);
        var totalSales = purchases.Sum(p => p.TotalAmount);

        var revenueByEvent = events
            .Select(e =>
            {
                var eventPurchases = purchases.Where(p => p.EventId == e.Id).ToList();
                return new EventRevenueDto(
                    e.Id,
                    e.Name,
                    eventPurchases.Sum(p => p.Quantity),
                    eventPurchases.Sum(p => p.TotalAmount));
            })
            .OrderByDescending(e => e.Revenue)
            .ToList();

        var ticketsByZone = purchases
            .Where(p => p.Zone is not null)
            .GroupBy(p => p.Zone!.ZoneType)
            .Select(g => new ZoneSalesDto(
                g.Key.ToString(),
                g.Sum(p => p.Quantity),
                g.Sum(p => p.TotalAmount)))
            .OrderBy(z => z.ZoneType)
            .ToList();

        return new SalesDashboardDto(
            events.Count,
            totalTicketsSold,
            totalSales,
            revenueByEvent,
            ticketsByZone);
    }
}
