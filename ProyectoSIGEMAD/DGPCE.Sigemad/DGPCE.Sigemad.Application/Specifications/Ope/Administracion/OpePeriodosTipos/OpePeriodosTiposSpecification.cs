using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePeriodosTipos;

public class OpePeriodosTiposSpecification : BaseSpecification<OpePeriodoTipo>
{
    public OpePeriodosTiposSpecification()
       : base(opePeriodoTipo => opePeriodoTipo.Borrado != true)
    {

    }
}
