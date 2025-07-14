using DGPCE.Sigemad.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosEmbarquesDiarios.Commands.CreateOpeDatosEmbarquesDiarios;

public class CreateOpeDatoEmbarqueDiarioCommandValidator : AbstractValidator<CreateOpeDatoEmbarqueDiarioCommand>
{
    public CreateOpeDatoEmbarqueDiarioCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(p => p.Fecha)
            .NotEmpty().WithMessage(localizer["FechObligatorio"]);

        RuleFor(p => p.NumeroRotaciones)
            .NotNull().WithMessage(localizer["NumeroRotacionesObligatorio"])
            .GreaterThanOrEqualTo(0).WithMessage(localizer["NumeroRotacionesDebeSerMayorOIgualCero"]);

        RuleFor(p => p.NumeroPasajeros)
          .NotNull().WithMessage(localizer["NumeroPasajerosObligatorio"])
           .GreaterThanOrEqualTo(0).WithMessage(localizer["NumeroPasajerosDebeSerMayorOIgualCero"]);

        RuleFor(p => p.NumeroTurismos)
           .NotNull().WithMessage(localizer["NumeroTurismosObligatorio"])
           .GreaterThanOrEqualTo(0).WithMessage(localizer["NumeroTurismosDebeSerMayorOIgualCero"]);

        RuleFor(p => p.NumeroAutocares)
         .NotNull().WithMessage(localizer["NumeroAutocaresObligatorio"])
          .GreaterThanOrEqualTo(0).WithMessage(localizer["NumeroAutocaresDebeSerMayorOIgualCero"]);

        RuleFor(p => p.NumeroCamiones)
          .NotNull().WithMessage(localizer["NumeroCamionesObligatorio"])
          .GreaterThanOrEqualTo(0).WithMessage(localizer["NumeroCamionesDebeSerMayorOIgualCero"]);

        RuleFor(p => p.NumeroTotalVehiculos)
           .NotNull().WithMessage(localizer["NumeroTotalVehiculosObligatorio"])
          .GreaterThanOrEqualTo(0).WithMessage(localizer["NumeroTotalVehiculosDebeSerMayorOIgualCero"]);

    }
}
