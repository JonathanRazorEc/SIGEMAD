using DGPCE.Sigemad.Application.Helpers;
using DGPCE.Sigemad.Application.Resources;
using DGPCE.Sigemad.Domain.Enums;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronteras.Commands.UpdateOpeDatosFronteras;

public class UpdateOpeDatoFronteraCommandValidator : AbstractValidator<UpdateOpeDatoFronteraCommand>
{
    public UpdateOpeDatoFronteraCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(p => p.Id)
            .NotEmpty().WithMessage(localizer["IdNoVacio"])
            .NotNull().WithMessage(localizer["IdObligatorio"]);

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
