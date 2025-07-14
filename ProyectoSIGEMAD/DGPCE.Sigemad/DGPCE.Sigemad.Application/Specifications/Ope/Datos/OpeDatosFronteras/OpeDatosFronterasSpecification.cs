using System;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosEmbarquesDiarios.Vms;
using DGPCE.Sigemad.Domain.Modelos;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeDatosFronteras;

public class OpeDatosFronterasSpecification : BaseSpecification<OpeDatoFrontera>
{
    public OpeDatosFronterasSpecification(OpeDatosFronterasSpecificationParams request)
        : base(opeDatoFrontera =>
        (!request.Id.HasValue || opeDatoFrontera.Id == request.Id) &&
        //(!request.IdOpeFrontera.HasValue || opeDatoFrontera.IdOpeFrontera == request.IdOpeFrontera) &&
         (request.IdsOpeFronteras == null || request.IdsOpeFronteras.Contains(opeDatoFrontera.IdOpeFrontera)) &&
       //(!request.Fecha.HasValue || DateOnly.FromDateTime(opeDatoFrontera.Fecha) == request.Fecha.Value) &&
       //
       (!request.FechaInicio.HasValue || DateOnly.FromDateTime(opeDatoFrontera.Fecha) >= request.FechaInicio) &&
        (!request.FechaFin.HasValue || DateOnly.FromDateTime(opeDatoFrontera.Fecha) <= request.FechaFin) &&
        //

        //
         (!request.IdOpeOpeDatoFronteraIntervaloHorario.HasValue || opeDatoFrontera.IdOpeDatoFronteraIntervaloHorario == request.IdOpeOpeDatoFronteraIntervaloHorario) &&
        //

        opeDatoFrontera.Borrado != true

        )
    {
        AddInclude(i => i.OpeFrontera);
        AddInclude(i => i.OpeDatoFronteraIntervaloHorario);

        ApplyPaging(request);

        // Aplicar la ordenación
        if (!string.IsNullOrEmpty(request.Sort?.ToLower()))
        {
            switch (request.Sort)
            {
                case "fechainicioasc":
                    AddOrderBy(i => i.Fecha);
                    break;
                case "fechaIniciodesc":
                    AddOrderByDescending(i => i.Fecha);
                    break;
                case "denominacionasc":
                    //AddOrderBy(i => i.Nombre);
                    break;
                case "denominaciondesc":
                    //AddOrderByDescending(i => i.Nombre);
                    break;
                default:
                    //AddOrderByDescending(i => i.Fecha); // Orden por defecto
                    AddOrderByDescending(i => i.Fecha);
                    AddOrderByDescending(i =>
                        i.IntervaloHorarioPersonalizado
                            ? i.InicioIntervaloHorarioPersonalizado ?? TimeSpan.Zero
                            : i.OpeDatoFronteraIntervaloHorario.Inicio
                    );

                    break;
            }
        }
    }
}
