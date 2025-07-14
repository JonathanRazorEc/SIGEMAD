using DGPCE.Sigemad.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.OtrasInformaciones.Commands.CreateOtrasInformaciones;
public class CreateOtraInformacionCommandValidator : AbstractValidator<CreateOtraInformacionCommand>
{
    public CreateOtraInformacionCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
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
