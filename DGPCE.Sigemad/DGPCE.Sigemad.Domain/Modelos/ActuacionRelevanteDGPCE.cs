using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;
public class ActuacionRelevanteDGPCE : BaseDomainModel<int>
{
    public int IdSuceso { get; set; }
    public Suceso Suceso { get; set; } = null!;
    public virtual EmergenciaNacional? EmergenciaNacional { get; set; }
    public virtual List<DeclaracionZAGEP> DeclaracionesZAGEP { get; set; } = new();
    public virtual List<ConvocatoriaCECOD> ConvocatoriasCECOD { get; set; } = new();
    public virtual List<NotificacionEmergencia> NotificacionesEmergencias { get; set; } = new();
    public virtual List<MovilizacionMedio> MovilizacionMedios { get; set; } = new();

    public virtual List<AuditoriaActivacionPlanEmergencia> AuditoriaActivacionPlanEmergencias { get; set; } = new();
    public virtual List<AuditoriaDeclaracionZAGEP> AuditoriaDeclaracionesZAGEP { get; set; } = new();

    public virtual AuditoriaEmergenciaNacional? AuditoriaEmergenciaNacional { get; set; }
    public virtual List<AuditoriaActivacionSistema> AuditoriaActivacionSistemas { get; set; } = new();
    public virtual List<AuditoriaConvocatoriaCECOD> AuditoriaConvocatoriasCECOD { get; set; } = new();
    public virtual List<AuditoriaNotificacionEmergencia> AuditoriaNotificacionesEmergencias { get; set; } = new();
    public virtual List<AuditoriaMovilizacionMedio> AuditoriaMovilizacionMedios { get; set; } = new();

}
