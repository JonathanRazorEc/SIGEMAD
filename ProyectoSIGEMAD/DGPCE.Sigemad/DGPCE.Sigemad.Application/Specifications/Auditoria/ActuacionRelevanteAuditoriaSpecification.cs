using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.ActuacionesRelevantesDGPCE
{
    public class ActuacionRelevanteAuditoriaSpecification : BaseSpecification<ActuacionRelevanteDGPCE>
    {
        public ActuacionRelevanteAuditoriaSpecification(ActuacionRelevanteDGPCESpecificationParams request)
             : base(ActuacionRelevanteDGPCyE =>
                 (!request.Id.HasValue || ActuacionRelevanteDGPCyE.Id == request.Id) &&
                 (!request.IdSuceso.HasValue || ActuacionRelevanteDGPCyE.IdSuceso == request.IdSuceso) &&
                 (ActuacionRelevanteDGPCyE.Borrado == false))
        {
            AddInclude(d => d.AuditoriaEmergenciaNacional);

            AddInclude(d => d.AuditoriaActivacionPlanEmergencias.Where(a => !a.Borrado));
            AddInclude("AuditoriaActivacionPlanEmergencias.TipoPlan");
            AddInclude("AuditoriaActivacionPlanEmergencias.PlanEmergencia");
            AddInclude("AuditoriaActivacionPlanEmergencias.Archivo");

            AddInclude(d => d.AuditoriaDeclaracionesZAGEP.Where(a => !a.Borrado));

            AddInclude(d => d.AuditoriaActivacionSistemas.Where(a => !a.Borrado));
            AddInclude("AuditoriaActivacionSistemas.TipoSistemaEmergencia");
            AddInclude("AuditoriaActivacionSistemas.ModoActivacion");

            //AddInclude(d => d.AuditoriaConvocatoriasCECOD.Where(a => !a.Borrado));

            AddInclude(d => d.AuditoriaNotificacionesEmergencias.Where(a => !a.Borrado));
            AddInclude("AuditoriaNotificacionesEmergencias.TipoNotificacion");

            AddInclude(d => d.AuditoriaMovilizacionMedios.Where(a => !a.Borrado));
            AddInclude("AuditoriaMovilizacionMedios.Pasos.PasoMovilizacion");
            AddInclude("AuditoriaMovilizacionMedios.Pasos.AuditoriaSolicitudMedio");
            AddInclude("AuditoriaMovilizacionMedios.Pasos.AuditoriaSolicitudMedio.ProcedenciaMedio");
            AddInclude("AuditoriaMovilizacionMedios.Pasos.AuditoriaSolicitudMedio.Archivo");
            AddInclude("AuditoriaMovilizacionMedios.Pasos.AuditoriaTramitacionMedio");
            AddInclude("AuditoriaMovilizacionMedios.Pasos.AuditoriaTramitacionMedio.DestinoMedio");
            AddInclude("AuditoriaMovilizacionMedios.Pasos.AuditoriaCancelacionMedio");
            AddInclude("AuditoriaMovilizacionMedios.Pasos.AuditoriaOfrecimientoMedio");
            AddInclude("AuditoriaMovilizacionMedios.Pasos.AuditoriaAportacionMedio");
            AddInclude("AuditoriaMovilizacionMedios.Pasos.AuditoriaAportacionMedio.Capacidad");
            AddInclude("AuditoriaMovilizacionMedios.Pasos.AuditoriaAportacionMedio.TipoAdministracion");
            AddInclude("AuditoriaMovilizacionMedios.Pasos.AuditoriaDespliegueMedio");
            AddInclude("AuditoriaMovilizacionMedios.Pasos.AuditoriaDespliegueMedio.Capacidad");
            AddInclude("AuditoriaMovilizacionMedios.Pasos.AuditoriaFinIntervencionMedio");
            AddInclude("AuditoriaMovilizacionMedios.Pasos.AuditoriaFinIntervencionMedio.Capacidad");
            AddInclude("AuditoriaMovilizacionMedios.Pasos.AuditoriaLlegadaBaseMedio");
            AddInclude("AuditoriaMovilizacionMedios.Pasos.AuditoriaLlegadaBaseMedio.Capacidad");
        }
    }
}
