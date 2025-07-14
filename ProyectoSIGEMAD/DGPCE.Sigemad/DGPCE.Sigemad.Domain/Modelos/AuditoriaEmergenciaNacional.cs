
using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;
public class AuditoriaEmergenciaNacional : BaseDomainModel<int>
{
    public string Autoridad { get; set; }
    public string DescripcionSolicitud { get; set; }
    public DateTime FechaHoraSolicitud { get; set; }
    public DateTime? FechaHoraDeclaracion { get; set; }
    public string? DescripcionDeclaracion { get; set; }
    public DateTime? FechaHoraDireccion { get; set; }
    public string? Observaciones { get; set; }

    public int IdEmergencia { get; set; } // en lugar de IdActuacionRelevanteDGPCE
    public virtual ActuacionRelevanteDGPCE ActuacionRelevanteDGPCE { get; set; }


}
