namespace DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios.Pasos;
public class ManageCancelacionMedioDto : DatosPasoBase
{
    public string Motivo { get; set; }
    public DateTime FechaHoraCancelacion { get; set; }
}
