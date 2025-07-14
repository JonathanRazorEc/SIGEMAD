using DGPCE.Sigemad.Domain.Common;


namespace DGPCE.Sigemad.Domain.Modelos;
public class ActivacionPlanEmergencia : BaseDomainModel<int>
{
    public int IdRegistro { get; set; }
    public virtual Registro Registro { get; set; } = null!;

    public int? IdTipoPlan { get; set; }
    public virtual TipoPlan TipoPlan { get; set; } = null!;

    public int? IdPlanEmergencia { get; set; }
    public virtual PlanEmergencia PlanEmergencia { get; set; } = null!;

    public string? TipoPlanPersonalizado { get; set; } = null!;
    public string? PlanEmergenciaPersonalizado { get; set; } = null!;
    public DateTimeOffset FechaHoraInicio { get; set; }
    public DateTimeOffset? FechaHoraFin { get; set; }
    public string Autoridad { get; set; }
    public string? Observaciones { get; set; }

    public Guid? IdArchivo { get; set; }
    public Archivo? Archivo { get; set; }
}
