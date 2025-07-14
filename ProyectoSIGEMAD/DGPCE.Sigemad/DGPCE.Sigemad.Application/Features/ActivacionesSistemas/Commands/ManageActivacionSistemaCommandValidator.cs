using DGPCE.Sigemad.Application.Dtos.ActivacionSistema;
using DGPCE.Sigemad.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.ActivacionesSistemas.Commands;
public class ManageActivacionSistemaCommandValidator : AbstractValidator<ManageActivacionSistemaCommand>
{
    public ManageActivacionSistemaCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x => x.IdSuceso)
            .GreaterThan(0).WithMessage(localizer["IdSucesoObligatorio"]);

        RuleForEach(x => x.Detalles).SetValidator(new ActivacionSistemaDtoValidator(localizer))
                .When(d => d.Detalles != null && d.Detalles.Count > 0);
    }
}

public class ActivacionSistemaDtoValidator : AbstractValidator<ManageActivacionSistemaDto>
{
    public ActivacionSistemaDtoValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        
        RuleFor(p => p.FechaActivacion)
       .NotEmpty().WithMessage(localizer["FechaHoraActivacionObligatorio"]);

        RuleFor(x => x.IdTipoSistemaEmergencia)
         .GreaterThan(0).WithMessage(localizer["IdTipoSistemaEmergencia"]);

        RuleFor(x => x.Autoridad)
            .MaximumLength(510).WithMessage(localizer["AutoridadMaxLength"]);

        RuleFor(x => x.Codigo)
            .MaximumLength(15).WithMessage(localizer["CodigoMaxLength"]);

    }
}