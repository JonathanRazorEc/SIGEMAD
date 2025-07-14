using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Features.Auditoria.Vms
{
    public class AuditoriaIncendioVm
    {
        public VistaPrimariaVm VistaPrimaria { get; set; }
        public List<AuditoriaMovilizacionMediosExtraordinariosVm> MovilizacionMediosExtraOrdinarios { get; set; }
        public List<AuditoriaConvocatoriasCECODVm> ConvocatoriasCECOD { get; set; }
        public List<AuditoriaActivacionSistemaVm> ActivacionSistemas { get; set; }
        public List<AuditoriaDeclaracionZAGEPVm> DeclaracionZAGEP { get; set; }
        public AuditoriaDeclaracionEmergenciaInteresNacionalVm DeclaracionEmergenciaInteresNacional { get; set; }
        public List<AuditoriaActivacionPlanesEmergenciaVm> ActivacionPlanEmergencias { get; set; }
        public List<AuditoriaNotificacionesOficialesVm> NotificacionesOficiales { get; set; }
        public List<AuditoriaSucesosRelacionadosVm> SucesoRelacionado { get; set; }
    }

}
