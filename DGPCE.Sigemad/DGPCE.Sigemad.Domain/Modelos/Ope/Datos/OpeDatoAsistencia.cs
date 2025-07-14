using DGPCE.Sigemad.Domain.Common;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;

namespace DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
public class OpeDatoAsistencia : BaseDomainModel<int>
{
    public int IdOpePuerto { get; set; }
    public DateTime Fecha { get; set; }
    
    public virtual OpePuerto OpePuerto { get; set; }


    public List<OpeDatoAsistenciaSanitaria>? OpeDatosAsistenciasSanitarias { get; set; }
    public List<OpeDatoAsistenciaSocial>? OpeDatosAsistenciasSociales { get; set; }
    public List<OpeDatoAsistenciaTraduccion>? OpeDatosAsistenciasTraducciones { get; set; }
}
