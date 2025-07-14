using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeEstadosOcupacion;

public class OpeEstadosOcupacionSpecification : BaseSpecification<OpeEstadoOcupacion>
{
    public OpeEstadosOcupacionSpecification()
       : base(opeEstadosOcupacion => opeEstadosOcupacion.Borrado != true)
    {

    }
}
