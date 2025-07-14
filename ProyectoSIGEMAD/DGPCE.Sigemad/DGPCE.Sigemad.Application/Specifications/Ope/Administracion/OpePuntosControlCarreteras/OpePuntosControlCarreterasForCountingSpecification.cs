using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePuntosControlCarreteras;

public class OpePuntosControlCarreterasForCountingSpecification : BaseSpecification<OpePuntoControlCarretera>
{
    public OpePuntosControlCarreterasForCountingSpecification(OpePuntosControlCarreterasSpecificationParams request)
        : base(opePuntoControlCarretera =>
        (string.IsNullOrEmpty(request.Nombre) || opePuntoControlCarretera.Nombre.Contains(request.Nombre)) &&
        (!request.Id.HasValue || opePuntoControlCarretera.Id == request.Id) &&
        opePuntoControlCarretera.Borrado != true
        )
    {

    }
}
