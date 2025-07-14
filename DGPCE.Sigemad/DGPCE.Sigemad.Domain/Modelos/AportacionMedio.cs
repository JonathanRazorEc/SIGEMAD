using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;
public class AportacionMedio : BaseDomainModel<int>
{
    public EjecucionPaso EjecucionPaso { get; set; }
    public int IdEjecucionPaso { get; set; }


    public Capacidad Capacidad { get; set; }
    public int IdCapacidad { get; set; }


    public TipoAdministracion TipoAdministracion { get; set; }
    public int IdTipoAdministracion { get; set; }


    public string? MedioNoCatalogado { get; set; }
    public string? TitularMedio { get; set; }
    public DateTime FechaHoraAportacion { get; set; }
    public string? Descripcion { get; set; }
}
