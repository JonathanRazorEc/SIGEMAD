namespace DGPCE.Sigemad.API.Models.ActivacionesPlanes;

public class ManageActivacionPlanRequest
{
    public int? IdRegistroActualizacion { get; set; }
    public int IdSuceso { get; set; }
    public List<ActivacionPlanRequest> ActivacionPlanes { get; set; } = new();
}
