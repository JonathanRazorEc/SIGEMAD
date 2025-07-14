using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeDatosAsistencias;

public class OpeDatosAsistenciasForCountingSpecification : BaseSpecification<OpeDatoAsistencia>
{
    public OpeDatosAsistenciasForCountingSpecification(OpeDatosAsistenciasSpecificationParams request)
        : base(opeDatoAsistencia =>
        (!request.Id.HasValue || opeDatoAsistencia.Id == request.Id) &&
        opeDatoAsistencia.Borrado != true
        )
    {

    }
}
