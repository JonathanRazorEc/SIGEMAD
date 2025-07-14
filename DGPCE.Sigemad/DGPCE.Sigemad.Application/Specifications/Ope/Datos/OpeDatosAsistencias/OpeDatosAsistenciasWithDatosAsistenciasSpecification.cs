using DGPCE.Sigemad.Domain.Modelos;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;

namespace DGPCE.Sigemad.Application.Specifications.OtrasInformaciones;
public class OpeDatosAsistenciasWithDatosAsistenciasSpecification : BaseSpecification<OpeDatoAsistencia>
{
    public OpeDatosAsistenciasWithDatosAsistenciasSpecification(int id)
        : base(i => i.Id == id && i.Borrado == false)
    {
        AddInclude(i => i.OpeDatosAsistenciasSanitarias.Where(d => !d.Borrado));
        AddInclude(i => i.OpeDatosAsistenciasSociales.Where(d => !d.Borrado));
        AddInclude(i => i.OpeDatosAsistenciasTraducciones.Where(d => !d.Borrado));

        AddInclude("OpeDatosAsistenciasSociales.OpeDatosAsistenciasSocialesTareas");
        AddInclude("OpeDatosAsistenciasSociales.OpeDatosAsistenciasSocialesOrganismos");
        AddInclude("OpeDatosAsistenciasSociales.OpeDatosAsistenciasSocialesUsuarios");
    }
}

