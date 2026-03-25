using EventService.Application.DTOs;
using FluentValidation;

namespace EventService.Application.Validators;

public class CreateEventRequestValidator : AbstractValidator<CreateEventRequest>
{
    public CreateEventRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre del evento es obligatorio.")
            .MaximumLength(200);

        RuleFor(x => x.Date)
            .GreaterThan(DateTime.UtcNow).WithMessage("La fecha del evento debe ser futura.");

        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("El lugar del evento es obligatorio.")
            .MaximumLength(300);

        RuleFor(x => x.Zones)
            .NotEmpty().WithMessage("Debe incluir al menos una zona.");

        RuleForEach(x => x.Zones).ChildRules(zone =>
        {
            zone.RuleFor(z => z.Name)
                .NotEmpty().WithMessage("El nombre de la zona es obligatorio.");

            zone.RuleFor(z => z.Price)
                .GreaterThanOrEqualTo(0).WithMessage("El precio debe ser mayor o igual a 0.");

            zone.RuleFor(z => z.Capacity)
                .GreaterThan(0).WithMessage("La capacidad debe ser mayor a 0.");
        });
    }
}
