using DGPCE.Sigemad.Application.Helpers;
using DGPCE.Sigemad.Application.Resources;
using DGPCE.Sigemad.Domain.Enums;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosAsistencias.Commands.UpdateOpeDatosAsistencias;

public class UpdateOpeDatoAsistenciaCommandValidator : AbstractValidator<UpdateOpeDatoAsistenciaCommand>
{
    public UpdateOpeDatoAsistenciaCommandValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(p => p.Id)
            .NotEmpty().WithMessage(localizer["IdNoVacio"])
            .NotNull().WithMessage(localizer["IdObligatorio"]);

        RuleFor(p => p.Fecha)
            .NotEmpty().WithMessage(localizer["FechaObligatorio"]);

        // VALIDACIÓN DE LISTAS (sanitarias, sociales y traducciones)
        RuleFor(p => p.OpeDatosAsistenciasSanitarias)
            .Must(lista => lista == null || lista.Select(x => x.IdOpeAsistenciaSanitariaTipo).Distinct().Count() == lista.Count)
            .WithMessage(localizer["TiposAsistenciaSanitariaRepetidos"]);

        RuleFor(p => p.OpeDatosAsistenciasSociales)
           .Must(lista => lista == null || lista.Select(x => x.IdOpeAsistenciaSocialTipo).Distinct().Count() == lista.Count)
           .WithMessage(localizer["TiposAsistenciaSocialRepetidos"]);

        // VALIDACIÓN SUBLISTAS DE ASISTENCIAS SOCIALES (tareas, organismos)
        RuleForEach(p => p.OpeDatosAsistenciasSociales).ChildRules(social =>
        {
            social.RuleFor(x => x.OpeDatosAsistenciasSocialesTareas)
                .Must(tareas => tareas == null || tareas.Select(t => t.IdOpeAsistenciaSocialTareaTipo).Distinct().Count() == tareas.Count)
                .WithMessage(localizer["TiposAsistenciaSocialTareasRepetidos"]);

            social.RuleFor(x => x.OpeDatosAsistenciasSocialesOrganismos)
                .Must(organismos => organismos == null || organismos.Select(o => o.IdOpeAsistenciaSocialOrganismoTipo).Distinct().Count() == organismos.Count)
                .WithMessage(localizer["TiposAsistenciaSocialOrganismosRepetidos"]);            
        });
        // VALIDACIÓN SUBLISTAS DE ASISTENCIAS SOCIALES (tareas, organismos)

        RuleFor(p => p.OpeDatosAsistenciasTraducciones)
        .Must(lista => lista == null || lista.Count <= 1)
        .WithMessage(localizer["SoloUnaAsistenciaTraduccion"]);
        //  FIN VALIDACIÓN DE LISTAS (sanitarias, sociales y traducciones)
    }
}
