using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios.Pasos;
public class LlegadaBaseMedioDto
{
    public Capacidad Capacidad { get; set; }
    public int IdCapacidad { get; set; }

    public string? MedioNoCatalogado { get; set; }
    public DateTime? FechaHoraLlegada { get; set; }
    public string? Observaciones { get; set; }
}
