using DGPCE.Sigemad.Application.Dtos.Impactos;
using DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.CreateListaImpactoEvolucion;
using DGPCE.Sigemad.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.ManageImpactos;

public class ManageImpactosCommandValidator : AbstractValidator<ManageImpactosCommand>
{
    public ManageImpactosCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x => x.IdSuceso).GreaterThan(0).WithMessage(localizer["IdSucesoObligatorio"]);

        //RuleForEach(x => x.Impactos)
        //    .SetValidator(new ImpactoDtoValidator())
        //    .When(d => d.Impactos.Count > 0);
    }

    public class ImpactoDtoValidator : AbstractValidator<ManageImpactoDto>
    {
        public ImpactoDtoValidator()
        {
            RuleFor(x => x.IdImpactoClasificado).GreaterThan(0).WithMessage("El ID de Impacto Clasificado es obligatorio.");
        }
    }
}
