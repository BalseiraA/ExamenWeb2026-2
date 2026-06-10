using MediatR;
using TicketSystem.Application.DTOs;

namespace TicketSystem.Application.Features.Purchases.Queries.GetPurchasesByEvent;

public record GetPurchasesByEventQuery(int EventId) : IRequest<List<PurchaseDto>>;
