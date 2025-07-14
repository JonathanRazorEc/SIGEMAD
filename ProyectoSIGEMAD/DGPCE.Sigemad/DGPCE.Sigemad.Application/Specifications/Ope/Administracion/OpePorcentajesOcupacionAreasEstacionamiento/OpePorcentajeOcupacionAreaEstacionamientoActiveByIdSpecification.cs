using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePorcentajesOcupacionAreasEstacionamiento;
public class OpePorcentajeOcupacionAreaEstacionamientoActiveByIdSpecification : BaseSpecification<OpePorcentajeOcupacionAreaEstacionamiento>
{
    public OpePorcentajeOcupacionAreaEstacionamientoActiveByIdSpecification(int id)
        : base(i => i.Id == id && i.Borrado == false)
    {

    }
}
