using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeAsistenciasSocialesNacionalidades;

public class OpeAsistenciasSocialesNacionalidadesSpecification : BaseSpecification<OpeAsistenciaSocialNacionalidad>
{
    public OpeAsistenciasSocialesNacionalidadesSpecification()
       : base(opeAsistenciaSocialNacionalidad => opeAsistenciaSocialNacionalidad.Borrado != true)
    {
        AddOrderBy(opeAsistenciaSocialNacionalidad => opeAsistenciaSocialNacionalidad.Nombre);
    }
}
