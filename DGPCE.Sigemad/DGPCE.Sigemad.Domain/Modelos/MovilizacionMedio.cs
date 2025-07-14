using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;
public class MovilizacionMedio : BaseDomainModel<int>
{
    public string Solicitante { get; set; }

    public ActuacionRelevanteDGPCE ActuacionRelevanteDGPCE { get; set; }
    public int IdActuacionRelevanteDGPCE { get; set; }
    public List<EjecucionPaso> Pasos { get; set; } = new();
}
