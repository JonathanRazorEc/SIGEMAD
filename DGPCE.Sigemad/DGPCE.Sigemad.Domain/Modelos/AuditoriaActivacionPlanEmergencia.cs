using DGPCE.Sigemad.Domain.Common;


namespace DGPCE.Sigemad.Domain.Modelos;
public class AuditoriaActivacionPlanEmergencia : BaseDomainModel<int>
{
    public int IdActuacionRelevanteDGPCE { get; set; }
    public virtual ActuacionRelevanteDGPCE ActuacionRelevanteDGPCE { get; set; } = null!;

    public int? IdTipoPlan { get; set; }
    public virtual TipoPlan TipoPlan { get; set; } = null!;

    public int? IdPlanEmergencia { get; set; }
    public virtual PlanEmergencia PlanEmergencia { get; set; } = null!;

    public string? TipoPlanPersonalizado { get; set; } = null!;
    public string? PlanEmergenciaPersonalizado { get; set; } = null!;
    public DateOnly FechaInicio { get; set; }
    public DateOnly? FechaFin { get; set; }
    public string Autoridad { get; set; }
    public string? Observaciones { get; set; }

    public Guid? IdArchivo { get; set; }
    public Archivo? Archivo { get; set; }
}
