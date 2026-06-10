using FluentValidation;

namespace TicketSystem.Application.Features.Events.Commands.EditEvent;

public class EditEventCommandValidator : AbstractValidator<EditEventCommand>
{
    public EditEventCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(150);

        RuleFor(x => x.Description)
            .MaximumLength(1000);

        RuleFor(x => x.Venue)
            .NotEmpty().WithMessage("El lugar es obligatorio.")
            .MaximumLength(200);

        RuleForEach(x => x.Zones).ChildRules(zone =>
        {
            zone.RuleFor(z => z.Price)
                .GreaterThanOrEqualTo(0.01m).WithMessage("El precio de la zona debe ser de al menos $0.01.");

            zone.RuleFor(z => z.TotalCapacity)
                .GreaterThan(0).WithMessage("La capacidad de la zona debe ser mayor que cero.");
        });
    }
}
