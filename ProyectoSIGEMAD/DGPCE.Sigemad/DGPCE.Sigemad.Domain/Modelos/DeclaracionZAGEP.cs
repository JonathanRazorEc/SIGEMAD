using DGPCE.Sigemad.Domain.Common;


namespace DGPCE.Sigemad.Domain.Modelos;
public class DeclaracionZAGEP : BaseDomainModel<int>
{
    public int IdActuacionRelevanteDGPCE { get; set; }
    public DateOnly FechaSolicitud { get; set; }
    public string Denominacion { get; set; }
    public string? Observaciones { get; set; }

    public virtual ActuacionRelevanteDGPCE ActuacionRelevanteDGPCE { get; set; } = null!;
}
