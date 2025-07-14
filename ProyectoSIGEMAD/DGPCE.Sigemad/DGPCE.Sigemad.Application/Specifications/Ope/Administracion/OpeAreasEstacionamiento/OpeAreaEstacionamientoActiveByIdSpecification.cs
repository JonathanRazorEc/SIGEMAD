using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeAreasEstacionamiento;
public class OpeAreaEstacionamientoActiveByIdSpecification : BaseSpecification<OpeAreaEstacionamiento>
{
    public OpeAreaEstacionamientoActiveByIdSpecification(int id)
        : base(i => i.Id == id && i.Borrado == false)
    {

    }
}
