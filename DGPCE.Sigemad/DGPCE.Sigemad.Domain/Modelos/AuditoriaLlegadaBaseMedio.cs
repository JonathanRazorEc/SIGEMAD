using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;
public class AuditoriaLlegadaBaseMedio : BaseDomainModel<int>
{
    public AuditoriaEjecucionPaso AuditoriaEjecucionPaso { get; set; }
    public int IdEjecucionPaso { get; set; }


    public Capacidad Capacidad { get; set; }
    public int IdCapacidad { get; set; }

    public string? MedioNoCatalogado { get; set; }
    public DateTime? FechaHoraLlegada { get; set; }
    public string? Observaciones { get; set; }
}
