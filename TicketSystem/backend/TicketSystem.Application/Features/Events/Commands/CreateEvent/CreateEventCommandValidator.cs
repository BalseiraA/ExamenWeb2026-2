using FluentValidation;
using TicketSystem.Domain.Enums;

namespace TicketSystem.Application.Features.Events.Commands.CreateEvent;

public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(150);

        RuleFor(x => x.Description)
            .MaximumLength(1000);

        RuleFor(x => x.Venue)
            .NotEmpty().WithMessage("El lugar es obligatorio.")
            .MaximumLength(200);

        RuleFor(x => x.Date)
            .GreaterThan(DateTime.UtcNow).WithMessage("La fecha del evento debe ser futura.");

        RuleFor(x => x.Zones)
            .NotEmpty().WithMessage("Debe configurar al menos una zona.");

        RuleForEach(x => x.Zones).ChildRules(zone =>
        {
            zone.RuleFor(z => z.ZoneType)
                .Must(t => Enum.TryParse<ZoneType>(t, ignoreCase: true, out _))
                .WithMessage("El tipo de zona debe ser VIP, Preferente o General.");

            zone.RuleFor(z => z.Price)
                .GreaterThanOrEqualTo(0.01m).WithMessage("El precio de la zona debe ser de al menos $0.01.");

            zone.RuleFor(z => z.TotalCapacity)
                .GreaterThan(0).WithMessage("La capacidad de la zona debe ser mayor que cero.");
        });

        RuleFor(x => x.Zones)
            .Must(zones => zones
                .Select(z => z.ZoneType?.ToLowerInvariant())
                .Distinct()
                .Count() == zones.Count)
            .WithMessage("No se pueden repetir tipos de zona en un mismo evento.")
            .When(x => x.Zones is not null && x.Zones.Count > 0);
    }
}
