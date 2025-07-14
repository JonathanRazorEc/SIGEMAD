using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;
public class AuditoriaEjecucionPaso : BaseDomainModel<int>
{
    public AuditoriaMovilizacionMedio AuditoriaMovilizacionMedio { get; set; }
    public int IdMovilizacionMedio { get; set; }

    public PasoMovilizacion PasoMovilizacion { get; set; }
    public int IdPasoMovilizacion { get; set; }

    // Propiedades de navegación para múltiples pasos del mismo tipo
    public AuditoriaSolicitudMedio? AuditoriaSolicitudMedio { get; set; }
    public AuditoriaTramitacionMedio? AuditoriaTramitacionMedio { get; set; }
    public AuditoriaCancelacionMedio? AuditoriaCancelacionMedio { get; set; }
    public AuditoriaOfrecimientoMedio? AuditoriaOfrecimientoMedio { get; set; }
    public AuditoriaAportacionMedio? AuditoriaAportacionMedio { get; set; }
    public AuditoriaDespliegueMedio? AuditoriaDespliegueMedio { get; set; }
    public AuditoriaFinIntervencionMedio? AuditoriaFinIntervencionMedio { get; set; }
    public AuditoriaLlegadaBaseMedio? AuditoriaLlegadaBaseMedio { get; set; }
}
