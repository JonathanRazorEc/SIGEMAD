
namespace DGPCE.Sigemad.Application.Features.ActivacionesPlanesEmergencia.Vms;
public class ActivacionPlanEmergenciaVm 
{
    public int IdTipoPlan { get; set; }
    public string NombrePlan { get; set; } = null!;
    public string AutoridadQueLoActiva { get; set; } = null!;
    public string? RutaDocumentoActivacion { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public string? Observaciones { get; set; }

}
