using DGPCE.Sigemad.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Domain.Modelos;
public class AuditoriaNotificacionEmergencia : BaseDomainModel<int>
{
    public int IdActuacionRelevanteDGPCE { get; set; }
    public virtual ActuacionRelevanteDGPCE ActuacionRelevanteDGPCE { get; set; } = null!;

    public int IdTipoNotificacion { get; set; }
    public virtual TipoNotificacion TipoNotificacion { get; set; } = null!;
    
    public DateTime FechaHoraNotificacion { get; set; }

    public string OrganosNotificados { get; set; }

    public string? UCPM { get; set; }

    public string? OrganismoInternacional { get; set; }

    public string? OtrosPaises { get; set; }

    public string? Observaciones { get; set; }

   

}
