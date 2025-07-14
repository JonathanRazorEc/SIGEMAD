using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;
public class AuditoriaOfrecimientoMedio : BaseDomainModel<int>
{
    public AuditoriaEjecucionPaso AuditoriaEjecucionPaso { get; set; }
    public int IdEjecucionPaso { get; set; }


    public string TitularMedio { get; set; }
    public bool? GestionCECOD { get; set; }
    public DateTime FechaHoraOfrecimiento { get; set; }
    public string? Descripcion { get; set; }
    public DateTime? FechaHoraDisponibilidad { get; set; }
    public string? Observaciones { get; set; }
}
