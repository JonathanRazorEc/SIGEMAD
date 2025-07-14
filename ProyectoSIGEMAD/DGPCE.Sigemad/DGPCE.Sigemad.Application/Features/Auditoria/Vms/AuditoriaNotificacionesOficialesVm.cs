
namespace DGPCE.Sigemad.Application.Features.Auditoria.Vms
{
    public class AuditoriaNotificacionesOficialesVm
    {
        //Auditoria_notificacionEmergencia.[IdTipoNotificacion] [int] NOT NULL, 
        public string TipoNotificacion { get; set; }

        //Auditoria_notificacionEmergencia.[FechaHoraNotificacion] [datetime2]
        public DateTime Fecha { get; set; }

        //Auditoria_notificacionEmergencia.[OrganosNotificados] [nvarchar]
        public string OrganosSNPCNotificados { get; set; }

        //Auditoria_notificacionEmergencia.[OrganismoInternacional][nvarchar] (255)
        public string? OrganismoInternacional { get; set; }

    }

}
