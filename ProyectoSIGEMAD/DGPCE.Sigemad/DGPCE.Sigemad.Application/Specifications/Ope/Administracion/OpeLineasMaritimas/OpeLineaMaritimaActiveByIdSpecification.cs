using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeLineasMaritimas;
public class OpeLineaMaritimaActiveByIdSpecification : BaseSpecification<OpeLineaMaritima>
{
    public OpeLineaMaritimaActiveByIdSpecification(int id)
        : base(i => i.Id == id && i.Borrado == false)
    {

    }
}
