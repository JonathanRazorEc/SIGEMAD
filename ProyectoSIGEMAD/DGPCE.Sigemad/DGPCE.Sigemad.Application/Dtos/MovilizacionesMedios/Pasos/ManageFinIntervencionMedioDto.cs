namespace DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios.Pasos;
public class ManageFinIntervencionMedioDto : DatosPasoBase
{
    public int IdCapacidad { get; set; }
    public string? MedioNoCatalogado { get; set; }
    public DateTime? FechaHoraInicioIntervencion { get; set; }
    public string? Observaciones { get; set; }
}
