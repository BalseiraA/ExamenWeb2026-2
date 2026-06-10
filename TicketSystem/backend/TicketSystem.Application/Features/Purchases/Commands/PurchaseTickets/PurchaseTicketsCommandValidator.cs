using FluentValidation;

namespace TicketSystem.Application.Features.Purchases.Commands.PurchaseTickets;

public class PurchaseTicketsCommandValidator : AbstractValidator<PurchaseTicketsCommand>
{
    public PurchaseTicketsCommandValidator()
    {
        RuleFor(x => x.EventId).GreaterThan(0);
        RuleFor(x => x.ZoneId).GreaterThan(0);

        RuleFor(x => x.BuyerName)
            .NotEmpty().WithMessage("El nombre del comprador es obligatorio.")
            .MaximumLength(150);

        RuleFor(x => x.BuyerEmail)
            .NotEmpty().WithMessage("El email del comprador es obligatorio.")
            .EmailAddress().WithMessage("El email del comprador no tiene un formato válido.");

        RuleFor(x => x.Quantity)
            .InclusiveBetween(1, 10).WithMessage("La cantidad debe ser entre 1 y 10 boletos por compra.");
    }
}
