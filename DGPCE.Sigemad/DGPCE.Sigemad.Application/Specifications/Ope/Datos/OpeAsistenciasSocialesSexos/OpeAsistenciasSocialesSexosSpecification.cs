using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;

namespace DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeAsistenciasSocialesSexos;

public class OpeAsistenciasSocialesSexosSpecification : BaseSpecification<OpeAsistenciaSocialSexo>
{
    public OpeAsistenciasSocialesSexosSpecification()
       : base(opeAsistenciaSocialSexo => opeAsistenciaSocialSexo.Borrado != true)
    {

    }
}
