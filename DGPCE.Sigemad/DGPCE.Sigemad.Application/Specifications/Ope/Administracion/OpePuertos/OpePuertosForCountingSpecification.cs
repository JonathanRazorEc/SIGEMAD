using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePuertos;

public class OpePuertosForCountingSpecification : BaseSpecification<OpePuerto>
{
    public OpePuertosForCountingSpecification(OpePuertosSpecificationParams request)
        : base(opePuerto =>
        (string.IsNullOrEmpty(request.Nombre) || opePuerto.Nombre.Contains(request.Nombre)) &&
        (!request.Id.HasValue || opePuerto.Id == request.Id) &&
        (!request.IdOpeFase.HasValue || opePuerto.IdOpeFase == request.IdOpeFase) &&
        (!request.IdPais.HasValue || opePuerto.IdPais == request.IdPais) &&
        // Filtro por fechas
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
        ) &&
        //
        opePuerto.Borrado != true
        )
    {

    }
}
