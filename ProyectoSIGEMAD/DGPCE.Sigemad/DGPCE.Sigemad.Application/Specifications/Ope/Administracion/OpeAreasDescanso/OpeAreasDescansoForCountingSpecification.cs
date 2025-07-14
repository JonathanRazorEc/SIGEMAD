using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeAreasDescanso;

public class OpeAreasDescansoForCountingSpecification : BaseSpecification<OpeAreaDescanso>
{
    public OpeAreasDescansoForCountingSpecification(OpeAreasDescansoSpecificationParams request)
        : base(opeAreaDescanso =>
        (string.IsNullOrEmpty(request.Nombre) || opeAreaDescanso.Nombre.Contains(request.Nombre)) &&
        (!request.Id.HasValue || opeAreaDescanso.Id == request.Id) &&
        opeAreaDescanso.Borrado != true
        )
    {

    }
}
