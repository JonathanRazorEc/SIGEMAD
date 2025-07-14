using DGPCE.Sigemad.Application.Dtos.Documentaciones;
using DGPCE.Sigemad.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;


namespace DGPCE.Sigemad.Application.Features.Documentaciones.Commands.ManageDocumentaciones;


public class ManageDocumentacionesCommandListValidator : AbstractValidator<ManageDocumentacionesCommand>
{
    public ManageDocumentacionesCommandListValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x => x.IdSuceso)
         .GreaterThan(0).WithMessage(localizer["IdSucesoObligatorio"]);

        RuleForEach(x => x.DetallesDocumentaciones)
            .SetValidator(new ManageDocumentacionesCommandValidator(localizer))
            .When(d => d.DetallesDocumentaciones.Count > 0);
    }
}

public class ManageDocumentacionesCommandValidator : AbstractValidator<DetalleDocumentacionDto>
{
    public ManageDocumentacionesCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x => x.FechaHora)
            .NotEmpty().WithMessage(localizer["FechaHoraObligatorio"]);

        RuleFor(x => x.FechaHoraSolicitud)
            .NotEmpty().WithMessage(localizer["FechaHoraSolicitud"]);

        RuleFor(x => x.IdTipoDocumento)
            .GreaterThan(0).WithMessage(localizer["IdTipoDocumentoObligatorio"]);

    }
}
