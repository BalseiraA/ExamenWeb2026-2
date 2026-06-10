using MediatR;
using TicketSystem.Application.DTOs;

namespace TicketSystem.Application.Features.Dashboard.Queries.GetSalesDashboard;

public record GetSalesDashboardQuery : IRequest<SalesDashboardDto>;
