using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios.Pasos;
public class AportacionMedioDto
{
    public Capacidad Capacidad { get; set; }
    public TipoAdministracion TipoAdministracion { get; set; }
    public int IdTipoAdministracion { get; set; }


    public string? MedioNoCatalogado { get; set; }
    public string? TitularMedio { get; set; }
    public DateTime FechaHoraAportacion { get; set; }
    public string? Descripcion { get; set; }
}
