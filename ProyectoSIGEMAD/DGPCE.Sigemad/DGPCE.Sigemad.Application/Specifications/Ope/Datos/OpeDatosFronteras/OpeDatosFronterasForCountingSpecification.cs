using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosEmbarquesDiarios.Vms;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeDatosFronteras;

public class OpeDatosFronterasForCountingSpecification : BaseSpecification<OpeDatoFrontera>
{
    public OpeDatosFronterasForCountingSpecification(OpeDatosFronterasSpecificationParams request)
        : base(opeDatoFrontera =>
        (!request.Id.HasValue || opeDatoFrontera.Id == request.Id) &&
          //(!request.IdOpeFrontera.HasValue || opeDatoFrontera.IdOpeFrontera == request.IdOpeFrontera) &&
          (request.IdsOpeFronteras == null || request.IdsOpeFronteras.Contains(opeDatoFrontera.IdOpeFrontera)) &&
        //(!request.Fecha.HasValue || DateOnly.FromDateTime(opeDatoFrontera.Fecha) == request.Fecha.Value) &&
        //
        (!request.FechaInicio.HasValue || DateOnly.FromDateTime(opeDatoFrontera.Fecha) >= request.FechaInicio) &&
        (!request.FechaFin.HasValue || DateOnly.FromDateTime(opeDatoFrontera.Fecha) <= request.FechaFin) &&
        //
        (!request.IdOpeOpeDatoFronteraIntervaloHorario.HasValue || opeDatoFrontera.IdOpeDatoFronteraIntervaloHorario == request.IdOpeOpeDatoFronteraIntervaloHorario) &&
        opeDatoFrontera.Borrado != true
        )
    {

    }
}
