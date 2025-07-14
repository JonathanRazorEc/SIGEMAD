namespace DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios.Pasos;
public class ManageAportacionMedioDto : DatosPasoBase
{
    public int IdCapacidad { get; set; }
    public int IdTipoAdministracion { get; set; }
    public string? MedioNoCatalogado { get; set; }
    public string? TitularMedio { get; set; }
    public DateTime FechaHoraAportacion { get; set; }
    public string? Descripcion { get; set; }
}

