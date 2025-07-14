using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeLogs;
public class OpeLogActiveByIdSpecification : BaseSpecification<OpeLog>
{
    public OpeLogActiveByIdSpecification(int id)
        : base(i => i.Id == id && i.Borrado == false)
    {

    }
}
