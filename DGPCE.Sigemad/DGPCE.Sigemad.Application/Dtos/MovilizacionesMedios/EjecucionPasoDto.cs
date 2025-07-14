using DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios.Pasos;

namespace DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios;
public class EjecucionPasoDto
{
    public int Id { get; set; }
    public PasoMovilizacionDto PasoMovilizacion { get; set; }
    public SolicitudMedioDto? SolicitudMedio { get; set; }
    public TramitacionMedioDto? TramitacionMedio { get; set; }
    public CancelacionMedioDto? CancelacionMedio { get; set; }
    public OfrecimientoMedioDto? OfrecimientoMedio { get; set; }
    public AportacionMedioDto? AportacionMedio { get; set; }
    public DespliegueMedioDto? DespliegueMedio { get; set; }
    public FinIntervencionMedioDto? FinIntervencionMedio { get; set; }
    public LlegadaBaseMedioDto? LlegadaBaseMedio { get; set; }
}
