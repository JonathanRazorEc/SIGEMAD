using DGPCE.Sigemad.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.OtrasInformaciones.Commands.UpdateOtrasInformaciones;
public class UpdateDetalleOtraInformacionCommandValidator : AbstractValidator<UpdateDetalleOtraInformacionCommand>
{
    public UpdateDetalleOtraInformacionCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(p => p.Id)
                    .NotEmpty().WithMessage(localizer["IdNoVacio"])
                    .NotEqual(0).WithMessage(localizer["IdNoVacio"]);
        RuleFor(p => p.IdSuceso)
                    .NotEmpty().WithMessage(localizer["IdSucesoObligatorio"])
                    .NotEqual(0).WithMessage(localizer["IdSucesoObligatorio"]);
        RuleFor(p => p.IdMedio)
            .NotEmpty().WithMessage(localizer["MedioIdNoVacio"])
            .NotEqual(0).WithMessage(localizer["MedioIdObligatorio"]);
        RuleFor(p => p.FechaHora)
            .NotEmpty().WithMessage(localizer["FechaHoraNoVacio"]);
        RuleFor(p => p.Asunto)
            .NotEmpty().WithMessage(localizer["AsuntoNoVacio"])
            .NotNull().WithMessage(localizer["AsuntoNoNulo"])
            .MaximumLength(500).WithMessage(localizer["AsuntoMaxLegth"]);
    }
}
