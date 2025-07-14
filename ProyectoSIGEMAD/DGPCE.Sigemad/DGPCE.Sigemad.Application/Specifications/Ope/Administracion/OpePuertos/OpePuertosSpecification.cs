using DGPCE.Sigemad.Domain.Modelos;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePuertos;

public class OpePuertosSpecification : BaseSpecification<OpePuerto>
{
    public OpePuertosSpecification(OpePuertosSpecificationParams request)
        : base(opePuerto =>
        (string.IsNullOrEmpty(request.Nombre) || opePuerto.Nombre.Contains(request.Nombre)) &&
        (!request.IdOpeFase.HasValue || opePuerto.IdOpeFase == request.IdOpeFase) &&
        (!request.IdPais.HasValue || opePuerto.IdPais == request.IdPais) &&
        (!request.Id.HasValue || opePuerto.Id == request.Id) &&

         // (!request.FechaValidezDesde.HasValue || DateOnly.FromDateTime(opePuerto.FechaValidezHasta) >= request.FechaValidezDesde) &&
         //    (!request.FechaValidezHasta.HasValue || DateOnly.FromDateTime(opePuerto.FechaValidezHasta) <= request.FechaValidezHasta) &&
         // Filtro por fechas
         /*
            (
            // 1. Se pasan ambas fechas
            (request.FechaValidezDesde.HasValue && request.FechaValidezHasta.HasValue &&
                DateOnly.FromDateTime(opePuerto.FechaValidezDesde) >= request.FechaValidezDesde &&
                (
                    !opePuerto.FechaValidezHasta.HasValue ||
                    DateOnly.FromDateTime(opePuerto.FechaValidezHasta.Value) <= request.FechaValidezHasta
                )
            )
            // 2. Solo fecha desde
            || (request.FechaValidezDesde.HasValue && !request.FechaValidezHasta.HasValue &&
                DateOnly.FromDateTime(opePuerto.FechaValidezDesde) >= request.FechaValidezDesde
            )
            // 3. Solo fecha hasta
            || (!request.FechaValidezDesde.HasValue && request.FechaValidezHasta.HasValue &&
                (
                    !opePuerto.FechaValidezHasta.HasValue ||
                    DateOnly.FromDateTime(opePuerto.FechaValidezHasta.Value) <= request.FechaValidezHasta
                )
            )
            // 4. Ninguna fecha
            || (!request.FechaValidezDesde.HasValue && !request.FechaValidezHasta.HasValue)
         */

         //
         (
            // Si hay al menos una de las dos fechas, se aplica el filtro por solapamiento
            request.FechaValidezDesde.HasValue || request.FechaValidezHasta.HasValue
                ?
                (
                    DateOnly.FromDateTime(opePuerto.FechaValidezDesde) <= (request.FechaValidezHasta ?? DateOnly.MaxValue) &&
                    (
                        !opePuerto.FechaValidezHasta.HasValue ||
                        DateOnly.FromDateTime(opePuerto.FechaValidezHasta.Value) >= (request.FechaValidezDesde ?? DateOnly.MinValue)
                    )
                )
                : true // Si no hay ninguna fecha, se incluyen todos

        //
        ) &&
        //

        opePuerto.Borrado != true
        )
    {
        AddInclude(i => i.OpeFase);

        ApplyPaging(request);

        // Aplicar la ordenación
        if (!string.IsNullOrEmpty(request.Sort?.ToLower()))
        {
            switch (request.Sort)
            {
                case "fechainicioasc":
                    AddOrderBy(i => i.FechaValidezDesde);
                    break;
                case "fechaIniciodesc":
                    AddOrderByDescending(i => i.FechaValidezDesde);
                    break;
                case "denominacionasc":
                    AddOrderBy(i => i.Nombre);
                    break;
                case "denominaciondesc":
                    AddOrderByDescending(i => i.Nombre);
                    break;
                default:
                    AddOrderBy(i => i.Nombre); // Orden por defecto
                    break;
            }
        }
    }
}
