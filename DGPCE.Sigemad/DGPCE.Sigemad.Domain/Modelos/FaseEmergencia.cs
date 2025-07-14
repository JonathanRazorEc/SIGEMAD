namespace DGPCE.Sigemad.Domain.Modelos;
public class FaseEmergencia
{
    public int Id { get; set; }

    public int? IdPlanEmergencia { get; set; }

    public int Orden { get; set; }
    public string? Descripcion { get; set; }


    public virtual PlanEmergencia? PlanEmergencia { get; set; }

}
