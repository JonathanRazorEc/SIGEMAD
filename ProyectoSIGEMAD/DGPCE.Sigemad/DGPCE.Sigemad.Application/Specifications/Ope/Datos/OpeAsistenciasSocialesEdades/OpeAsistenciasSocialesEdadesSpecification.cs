using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeAsistenciasSocialesEdades;

public class OpeAsistenciasSocialesEdadesSpecification : BaseSpecification<OpeAsistenciaSocialEdad>
{
    public OpeAsistenciasSocialesEdadesSpecification()
       : base(opeAsistenciaSocialEdad => opeAsistenciaSocialEdad.Borrado != true)
    {
        AddOrderBy(opeAsistenciaSocialEdad => opeAsistenciaSocialEdad.Id);
    }
}
