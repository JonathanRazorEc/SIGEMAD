
namespace DGPCE.Sigemad.Application.Features.Auditoria.Vms
{
    public class AuditoriaActivacionSistemaVm
    {


        //Auditoria_ActivacionSistema.[IdTipoSistemaEmergencia][int]
        //public int TipoActivacion { get; set; }

        public string? TipoActivacion { get; set; }

        //Auditoria_ActivacionSistema.[FechaHoraActivacion] 
        public DateTime? FechaActivacion
        { get; set; }

        //Auditoria_ActivacionSistema.[FechaHoraActualizacion] [datetime2](7) NULL,
        public DateTime? FechaActualizacion { get; set; }


    }

}
