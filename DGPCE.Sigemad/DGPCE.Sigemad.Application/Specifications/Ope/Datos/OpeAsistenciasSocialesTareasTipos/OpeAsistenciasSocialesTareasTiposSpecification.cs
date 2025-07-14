using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeAsistenciasSocialesTareasTipos;

public class OpeAsistenciasSocialesTareasTiposSpecification : BaseSpecification<OpeAsistenciaSocialTareaTipo>
{
    public OpeAsistenciasSocialesTareasTiposSpecification()
       : base(opeAsistenciaSocialTareaTipo => opeAsistenciaSocialTareaTipo.Borrado != true)
    {
        AddOrderBy(opeAsistenciaSocialTareaTipo => opeAsistenciaSocialTareaTipo.Nombre);
    }
}
