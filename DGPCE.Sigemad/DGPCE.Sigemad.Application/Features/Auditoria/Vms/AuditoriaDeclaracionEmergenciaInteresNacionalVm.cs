
using DGPCE.Sigemad.Application.Helpers;

namespace DGPCE.Sigemad.Application.Features.Auditoria.Vms
{
    public class AuditoriaDeclaracionEmergenciaInteresNacionalVm
    {
        //Auditoria_EmergenciaNacional.[Autoridad][nvarchar] (
        public string AutoridadSolicitante { get; set; }

        //Auditoria_EmergenciaNacional.[DescripcionDeclaracion]
        public string? DescripcionDeLaSolicitud { get; set; }

        //Auditoria_EmergenciaNacional.[FechaHoraSolicitud][datetime2] (7) NOT NULL,
        public DateTime FechaSolicitud { get; set; }

        //Auditoria_EmergenciaNacional.[FechaHoraDeclaracion][datetime2] (7)  NULL, 

        public DateTime? FechaDeclaracion { get; set; }
    }

}
