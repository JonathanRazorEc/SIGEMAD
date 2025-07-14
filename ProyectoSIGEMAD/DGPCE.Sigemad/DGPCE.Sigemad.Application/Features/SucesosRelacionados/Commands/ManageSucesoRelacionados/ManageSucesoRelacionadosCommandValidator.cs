using DGPCE.Sigemad.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.SucesosRelacionados.Commands.ManageSucesoRelacionados;

public class ManageSucesoRelacionadosCommandValidator : AbstractValidator<ManageSucesoRelacionadosCommand>
{
    public ManageSucesoRelacionadosCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x => x.IdSuceso)
            .GreaterThan(0).WithMessage(localizer["IdSucesoObligatorio"]);
    }
}