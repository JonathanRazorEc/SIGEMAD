using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeAsistenciasSocialesTipos;

public class OpeAsistenciasSocialesTiposSpecification : BaseSpecification<OpeAsistenciaSocialTipo>
{
    public OpeAsistenciasSocialesTiposSpecification()
       : base(opeAsistenciaSocialTipo => opeAsistenciaSocialTipo.Borrado != true)
    {
        AddOrderBy(opeAsistenciaSocialTipo => opeAsistenciaSocialTipo.Nombre);
    }
}
