using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeAsistenciasSocialesOrganismosTipos;

public class OpeAsistenciasSocialesOrganismosTiposSpecification : BaseSpecification<OpeAsistenciaSocialOrganismoTipo>
{
    public OpeAsistenciasSocialesOrganismosTiposSpecification()
       : base(opeAsistenciaSocialOrganismoTipo => opeAsistenciaSocialOrganismoTipo.Borrado != true)
    {
        AddOrderBy(opeAsistenciaSocialOrganismoTipo => opeAsistenciaSocialOrganismoTipo.Nombre);
    }
}
