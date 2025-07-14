using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeAreasDescansoTipos;

public class OpeAreasDescansoTiposSpecification : BaseSpecification<OpeAreaDescansoTipo>
{
    public OpeAreasDescansoTiposSpecification()
       : base(opeAreaDescansoTipo => opeAreaDescansoTipo.Borrado != true)
    {

    }
}
