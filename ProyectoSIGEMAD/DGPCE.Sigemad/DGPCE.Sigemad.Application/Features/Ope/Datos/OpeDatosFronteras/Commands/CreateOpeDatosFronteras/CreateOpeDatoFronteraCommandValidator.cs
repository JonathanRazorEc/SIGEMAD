using DGPCE.Sigemad.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronteras.Commands.CreateOpeDatosFronteras;

public class CreateOpeDatoFronteraCommandValidator : AbstractValidator<CreateOpeDatoFronteraCommand>
{
    public CreateOpeDatoFronteraCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(p => p.Fecha)
            .NotEmpty().WithMessage(localizer["FechaObligatorio"])
            .LessThanOrEqualTo(DateTime.Today).WithMessage(localizer["FechaNoPuedeSerSuperiorAHoy"]);

        RuleFor(p => p.IdOpeDatoFronteraIntervaloHorario)
            .NotNull().WithMessage(localizer["OpeDatoFronteraIntervaloHorarioObligatorio"])
            .GreaterThan(0).WithMessage(localizer["OpeDatoFronteraIntervaloHorarioInvalido"]);

        RuleFor(p => p.NumeroVehiculos)
           .NotEmpty().WithMessage(localizer["NumeroVehiculosObligatorio"]);

        RuleFor(p => p.Afluencia)
            .NotEmpty().WithMessage(localizer["AfluenciaObligatorio"]);

    }
}
