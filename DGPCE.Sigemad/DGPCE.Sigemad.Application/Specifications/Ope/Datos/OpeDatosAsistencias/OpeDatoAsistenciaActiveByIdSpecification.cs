using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeDatosAsistencias;
public class OpeDatoAsistenciaActiveByIdSpecification : BaseSpecification<OpeDatoAsistencia>
{
    public OpeDatoAsistenciaActiveByIdSpecification(int id)
        : base(i => i.Id == id && i.Borrado == false)
    {

    }
}
