using DGPCE.Sigemad.Application.Dtos.Parametros.Dtos;
using DGPCE.Sigemad.Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.Parametros.Commands;

public class ManageParametroCommandValidator : AbstractValidator<ManageParametroCommand>
{
    public ManageParametroCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x => x.IdSuceso)
            .GreaterThan(0).WithMessage(localizer["IdSucesoObligatorio"]);

        RuleForEach(x => x.Parametro)
         .SetValidator(new CreateParametroCommandValidator(localizer));

    }
}

public class CreateParametroCommandValidator : AbstractValidator<CreateOrUpdateParametroDto>
{
    public CreateParametroCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        //RuleFor(x => x.IdEstadoIncendio)
          //  .GreaterThan(0).WithMessage(localizer["IdEstadoIncendioObligatorio"]);

        RuleFor(x => x.FechaHoraActualizacion)
            .NotNull().WithMessage(localizer["FechaHoraActualizacionObligatorio"]);



    }
}
