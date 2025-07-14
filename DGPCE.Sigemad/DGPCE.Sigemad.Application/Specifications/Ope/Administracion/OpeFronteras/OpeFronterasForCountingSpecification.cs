using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeFronteras;

public class OpeFronterasForCountingSpecification : BaseSpecification<OpeFrontera>
{
    public OpeFronterasForCountingSpecification(OpeFronterasSpecificationParams request)
        : base(opeFrontera =>
        (string.IsNullOrEmpty(request.Nombre) || opeFrontera.Nombre.Contains(request.Nombre)) &&
        (!request.Id.HasValue || opeFrontera.Id == request.Id) &&
        opeFrontera.Borrado != true
        )
    {

    }
}
