using MediatR;
using TicketSystem.Application.Common.Interfaces;
using TicketSystem.Application.Common.Mappings;
using TicketSystem.Application.DTOs;

namespace TicketSystem.Application.Features.Purchases.Queries.GetPurchasesByEvent;

public class GetPurchasesByEventQueryHandler : IRequestHandler<GetPurchasesByEventQuery, List<PurchaseDto>>
{
    private readonly ITicketPurchaseRepository _purchaseRepository;

    public GetPurchasesByEventQueryHandler(ITicketPurchaseRepository purchaseRepository)
    {
        _purchaseRepository = purchaseRepository;
    }

    public async Task<List<PurchaseDto>> Handle(GetPurchasesByEventQuery request, CancellationToken cancellationToken)
    {
        var purchases = await _purchaseRepository.GetByEventAsync(request.EventId, cancellationToken);
        return purchases.Select(p => p.ToDto()).ToList();
    }
}
