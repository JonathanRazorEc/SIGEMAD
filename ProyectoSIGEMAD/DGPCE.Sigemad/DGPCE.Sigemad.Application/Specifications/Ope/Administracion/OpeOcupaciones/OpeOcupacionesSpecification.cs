using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeOcupaciones;

public class OpeOcupacionesSpecification : BaseSpecification<OpeOcupacion>
{
    public OpeOcupacionesSpecification()
       : base(opeOcupaciones => opeOcupaciones.Borrado != true)
    {

    }
}
