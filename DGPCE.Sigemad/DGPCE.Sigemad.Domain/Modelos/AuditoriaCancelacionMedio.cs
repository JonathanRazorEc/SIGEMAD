using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;
public class AuditoriaCancelacionMedio : BaseDomainModel<int>
{
    public AuditoriaEjecucionPaso AuditoriaEjecucionPaso { get; set; }
    public int IdEjecucionPaso { get; set; }


    public string Motivo { get; set; }
    public DateTime FechaHoraCancelacion { get; set; }
}
