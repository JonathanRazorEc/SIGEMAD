namespace DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios.Pasos;
public class ManageOfrecimientoMedioDto : DatosPasoBase
{
    public string TitularMedio { get; set; }
    public bool? GestionCECOD { get; set; }
    public DateTime FechaHoraOfrecimiento { get; set; }
    public string? Descripcion { get; set; }
    public DateTime? FechaHoraDisponibilidad { get; set; }
    public string? Observaciones { get; set; }

}
