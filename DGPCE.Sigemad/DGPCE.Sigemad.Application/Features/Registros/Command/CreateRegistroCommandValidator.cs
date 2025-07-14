using DGPCE.Sigemad.Application.Features.Registros.Command.CreateRegistros;
using DGPCE.Sigemad.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;


namespace DGPCE.Sigemad.Application.Features.Registros.Command;
public class CreateRegistroCommandValidator : AbstractValidator<CreateRegistroCommand>
{
    public CreateRegistroCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x => x.FechaHoraEvolucion)
            .NotNull().WithMessage(localizer["FechaHoraObligatorio"]);

        RuleFor(x => x.IdEntradaSalida)
            .GreaterThan(0).WithMessage(localizer["IdEntradaSalidaObligatorio"]);

        RuleFor(x => x.IdMedio)
            .GreaterThan(0).WithMessage(localizer["IdMedioObligatorio"]);

        RuleFor(x => x.RegistroProcedenciasDestinos)
            .NotEmpty().WithMessage(localizer["RegistroProcedenciasDestinosObligatorio"]);
    }
}