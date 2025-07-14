using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePeriodos;
public class OpePeriodoActiveByIdSpecification : BaseSpecification<OpePeriodo>
{
    public OpePeriodoActiveByIdSpecification(int id)
        : base(i => i.Id == id && i.Borrado == false)
    {

    }
}
