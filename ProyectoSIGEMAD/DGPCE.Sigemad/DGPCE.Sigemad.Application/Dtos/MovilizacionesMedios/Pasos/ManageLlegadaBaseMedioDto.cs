namespace DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios.Pasos;
public class ManageLlegadaBaseMedioDto : DatosPasoBase
{
    public int IdCapacidad { get; set; }
    public string? MedioNoCatalogado { get; set; }
    public DateTime? FechaHoraLlegada { get; set; }
    public string? Observaciones { get; set; }
}
