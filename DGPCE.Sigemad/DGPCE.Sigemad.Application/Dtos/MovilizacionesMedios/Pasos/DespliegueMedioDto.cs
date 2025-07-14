using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios.Pasos;
public class DespliegueMedioDto
{
    public Capacidad Capacidad { get; set; }
    public int IdCapacidad { get; set; }


    public string? MedioNoCatalogado { get; set; }
    public DateTime FechaHoraDespliegue { get; set; }
    public DateTime? FechaHoraInicioIntervencion { get; set; }
    public string? Observaciones { get; set; }
}
