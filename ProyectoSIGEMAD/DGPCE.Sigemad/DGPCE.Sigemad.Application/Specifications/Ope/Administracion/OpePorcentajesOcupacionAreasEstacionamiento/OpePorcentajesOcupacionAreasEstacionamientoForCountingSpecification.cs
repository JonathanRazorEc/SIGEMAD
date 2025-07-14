using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePorcentajesOcupacionAreasEstacionamiento;

public class OpePorcentajesOcupacionAreasEstacionamientoForCountingSpecification : BaseSpecification<OpePorcentajeOcupacionAreaEstacionamiento>
{
    public OpePorcentajesOcupacionAreasEstacionamientoForCountingSpecification(OpePorcentajesOcupacionAreasEstacionamientoSpecificationParams request)
        : base(opePorcentajeOcupacionAreaEstacionamiento =>
        (!request.Id.HasValue || opePorcentajeOcupacionAreaEstacionamiento.Id == request.Id) &&
        opePorcentajeOcupacionAreaEstacionamiento.Borrado != true
        )
    {

    }
}
