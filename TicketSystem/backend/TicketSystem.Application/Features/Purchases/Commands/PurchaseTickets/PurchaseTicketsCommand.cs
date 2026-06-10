using MediatR;
using TicketSystem.Application.DTOs;

namespace TicketSystem.Application.Features.Purchases.Commands.PurchaseTickets;

public record PurchaseTicketsCommand(
    int EventId,
    int ZoneId,
    string BuyerName,
    string BuyerEmail,
    int Quantity) : IRequest<PurchaseDto>;
