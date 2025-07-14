using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeAsistenciasSanitariasTipos;

public class OpeAsistenciasSanitariasTiposSpecification : BaseSpecification<OpeAsistenciaSanitariaTipo>
{
    public OpeAsistenciasSanitariasTiposSpecification()
       : base(opeAsistenciaSanitariaTipo => opeAsistenciaSanitariaTipo.Borrado != true)
    {
        AddOrderBy(opeAsistenciaSanitariaTipo => opeAsistenciaSanitariaTipo.Nombre);
    }
}
