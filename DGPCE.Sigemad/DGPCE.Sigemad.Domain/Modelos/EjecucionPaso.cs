using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;
public class EjecucionPaso : BaseDomainModel<int>
{
    public MovilizacionMedio MovilizacionMedio { get; set; }
    public int IdMovilizacionMedio { get; set; }

    public PasoMovilizacion PasoMovilizacion { get; set; }
    public int IdPasoMovilizacion { get; set; }

    // Propiedades de navegación para múltiples pasos del mismo tipo
    public SolicitudMedio? SolicitudMedio { get; set; }
    public TramitacionMedio? TramitacionMedio { get; set; }
    public CancelacionMedio? CancelacionMedio { get; set; }
    public OfrecimientoMedio? OfrecimientoMedio { get; set; }
    public AportacionMedio? AportacionMedio { get; set; }
    public DespliegueMedio? DespliegueMedio { get; set; }
    public FinIntervencionMedio? FinIntervencionMedio { get; set; }
    public LlegadaBaseMedio? LlegadaBaseMedio { get; set; }
}
