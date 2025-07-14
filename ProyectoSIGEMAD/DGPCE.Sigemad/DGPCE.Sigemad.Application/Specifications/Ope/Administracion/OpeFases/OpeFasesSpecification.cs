using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeFases;

public class OpeFasesSpecification : BaseSpecification<OpeFase>
{
    public OpeFasesSpecification()
       : base(opeFase => opeFase.Borrado != true)
    {

    }
}
