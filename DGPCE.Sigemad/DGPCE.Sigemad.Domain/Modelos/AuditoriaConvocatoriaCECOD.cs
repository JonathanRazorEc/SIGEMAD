using DGPCE.Sigemad.Domain.Common;


namespace DGPCE.Sigemad.Domain.Modelos;
public class AuditoriaConvocatoriaCECOD : BaseDomainModel<int>
{

    public int IdActuacionRelevanteDGPCE { get; set; }
    public DateOnly FechaInicio { get; set; }
    public DateOnly? FechaFin { get; set; }
    public string Lugar { get; set; }
    public string Convocados { get; set; }
    public string? Participantes { get; set; }
    public string? Observaciones { get; set; }

    public virtual ActuacionRelevanteDGPCE ActuacionRelevanteDGPCE { get; set; } = null!;

}
