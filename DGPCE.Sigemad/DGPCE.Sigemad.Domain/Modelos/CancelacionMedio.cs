using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;
public class CancelacionMedio : BaseDomainModel<int>
{
    public EjecucionPaso EjecucionPaso { get; set; }
    public int IdEjecucionPaso { get; set; }


    public string Motivo { get; set; }
    public DateTime FechaHoraCancelacion { get; set; }
}
