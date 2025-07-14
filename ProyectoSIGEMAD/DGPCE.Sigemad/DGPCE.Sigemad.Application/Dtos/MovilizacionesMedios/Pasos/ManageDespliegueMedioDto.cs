namespace DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios.Pasos;
public class ManageDespliegueMedioDto : DatosPasoBase
{
    public int IdCapacidad { get; set; }
    public string? MedioNoCatalogado { get; set; }
    public DateTime FechaHoraDespliegue { get; set; }
    public DateTime? FechaHoraInicioIntervencion { get; set; }
    public string? Observaciones { get; set; }
}
