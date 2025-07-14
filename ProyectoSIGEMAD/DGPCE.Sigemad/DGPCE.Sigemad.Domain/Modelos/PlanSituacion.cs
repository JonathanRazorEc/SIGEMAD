namespace DGPCE.Sigemad.Domain.Modelos;
public class PlanSituacion
{
    public int Id { get; set; }

    public int? IdPlanEmergencia { get; set; }
    public int? IdFaseEmergencia { get; set; }

    public int Orden { get; set; }
    public string? Descripcion { get; set; }

    public string? Nivel { get; set; }

    public string? Situacion { get; set; }
    public string? SituacionEquivalente { get; set; }

    public virtual PlanEmergencia? PlanEmergencia { get; set; }

    public virtual FaseEmergencia? FaseEmergencia { get; set; }
}
