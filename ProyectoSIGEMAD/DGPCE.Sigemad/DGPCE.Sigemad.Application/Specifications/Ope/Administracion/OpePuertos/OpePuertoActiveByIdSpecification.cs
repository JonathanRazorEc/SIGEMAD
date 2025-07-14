using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePuertos;
public class OpePuertoActiveByIdSpecification : BaseSpecification<OpePuerto>
{
    public OpePuertoActiveByIdSpecification(int id)
        : base(i => i.Id == id && i.Borrado == false)
    {

    }
}
