using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeFronteras;
public class OpeFronteraActiveByIdSpecification : BaseSpecification<OpeFrontera>
{
    public OpeFronteraActiveByIdSpecification(int id)
        : base(i => i.Id == id && i.Borrado == false)
    {

    }
}
