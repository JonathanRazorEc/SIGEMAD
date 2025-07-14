using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeAreasEstacionamiento;

public class OpeAreasEstacionamientoForCountingSpecification : BaseSpecification<OpeAreaEstacionamiento>
{
    public OpeAreasEstacionamientoForCountingSpecification(OpeAreasEstacionamientoSpecificationParams request)
        : base(opeAreaEstacionamiento =>
        (string.IsNullOrEmpty(request.Nombre) || opeAreaEstacionamiento.Nombre.Contains(request.Nombre)) &&
        (!request.Id.HasValue || opeAreaEstacionamiento.Id == request.Id) &&
        opeAreaEstacionamiento.Borrado != true
        )
    {

    }
}
