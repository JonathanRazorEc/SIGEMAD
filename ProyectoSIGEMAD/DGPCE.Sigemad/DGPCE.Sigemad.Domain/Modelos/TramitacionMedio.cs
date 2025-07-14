using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;
public class TramitacionMedio : BaseDomainModel<int>
{
    public EjecucionPaso EjecucionPaso { get; set; }
    public int IdEjecucionPaso { get; set; }


    public DestinoMedio DestinoMedio { get; set; }
    public int IdDestinoMedio { get; set; }


    public string TitularMedio { get; set; }
    public DateTime FechaHoraTramitacion { get; set; }
    public string? Descripcion { get; set; }
    public bool? PublicadoCECIS { get; set; }
    public string? Observaciones { get; set; }
}
