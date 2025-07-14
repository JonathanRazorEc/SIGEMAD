using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeLineasMaritimas;

public class OpeLineasMaritimasForCountingSpecification : BaseSpecification<OpeLineaMaritima>
{
    public OpeLineasMaritimasForCountingSpecification(OpeLineasMaritimasSpecificationParams request)
        : base(opeLineaMaritima =>
        (string.IsNullOrEmpty(request.Nombre) || opeLineaMaritima.Nombre.Contains(request.Nombre)) &&
        (!request.Id.HasValue || opeLineaMaritima.Id == request.Id) &&
        opeLineaMaritima.Borrado != true
        )
    {

    }
}
