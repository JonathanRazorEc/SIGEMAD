using DGPCE.Sigemad.Domain.Common;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosAsistencias.Vms
{
    public class OpeDatoAsistenciaVm : BaseDomainModel<int>
    {
        public int IdOpePuerto { get; set; }
        public DateTime Fecha { get; set; }
       
        public virtual OpePuerto OpePuerto { get; set; } = null!;

        public List<OpeDatoAsistenciaSanitaria> OpeDatosAsistenciasSanitarias { get; set; } = null!;
        public List<OpeDatoAsistenciaSocial> OpeDatosAsistenciasSociales { get; set; } = null!;
        public List<OpeDatoAsistenciaTraduccion> OpeDatosAsistenciasTraducciones { get; set; } = null!;
    }
}
