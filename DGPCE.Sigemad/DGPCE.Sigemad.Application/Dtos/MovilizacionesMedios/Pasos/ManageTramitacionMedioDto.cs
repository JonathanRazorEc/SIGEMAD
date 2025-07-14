namespace DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios.Pasos;
public class ManageTramitacionMedioDto : DatosPasoBase
{
    public int IdDestinoMedio { get; set; }
    public string TitularMedio { get; set; }
    public DateTime FechaHoraTramitacion { get; set; }
    public string? Descripcion { get; set; }
    public string? Observaciones { get; set; }
    public bool? PublicadoCECIS { get; set; }
}
