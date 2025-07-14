namespace DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios.Pasos;
public class TramitacionMedioDto
{
    public DestinoMedioDto DestinoMedio { get; set; }
    public string TitularMedio { get; set; }
    public DateTime FechaHoraTramitacion { get; set; }
    public string? Descripcion { get; set; }
    public bool? PublicadoCECIS { get; set; }
    public string? Observaciones { get; set; }
}
