using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Specifications.ActuacionesRelevantesDGPCE
{
    public class ActuacionRelevanteDGPCEActiveByIdSpecification : BaseSpecification<ActuacionRelevanteDGPCE>
    {
        public ActuacionRelevanteDGPCEActiveByIdSpecification(ActuacionRelevanteDGPCESpecificationParams request)
            : base(ActuacionRelevanteDGPCyE =>
                (!request.Id.HasValue || ActuacionRelevanteDGPCyE.Id == request.Id) &&
                (!request.IdSuceso.HasValue || ActuacionRelevanteDGPCyE.IdSuceso == request.IdSuceso) &&
                (ActuacionRelevanteDGPCyE.Borrado == false))
        {
            if (request.Id.HasValue || request.IdSuceso.HasValue)
            {
                // Relaciones de las tablas "normales"
                AddInclude(d => d.EmergenciaNacional);

                AddInclude(d => d.DeclaracionesZAGEP.Where(dir => !dir.Borrado));

     

                AddInclude(d => d.ConvocatoriasCECOD.Where(dir => !dir.Borrado));

                AddInclude(d => d.NotificacionesEmergencias.Where(dir => !dir.Borrado));
                AddInclude("NotificacionesEmergencias.TipoNotificacion");

                AddInclude(d => d.MovilizacionMedios.Where(dir => !dir.Borrado));
                AddInclude("MovilizacionMedios.Pasos.PasoMovilizacion");
                AddInclude("MovilizacionMedios.Pasos.SolicitudMedio");
                AddInclude("MovilizacionMedios.Pasos.SolicitudMedio.ProcedenciaMedio");
                AddInclude("MovilizacionMedios.Pasos.SolicitudMedio.Archivo");
                AddInclude("MovilizacionMedios.Pasos.TramitacionMedio");
                AddInclude("MovilizacionMedios.Pasos.TramitacionMedio.DestinoMedio");
                AddInclude("MovilizacionMedios.Pasos.CancelacionMedio");
                AddInclude("MovilizacionMedios.Pasos.OfrecimientoMedio");
                AddInclude("MovilizacionMedios.Pasos.AportacionMedio");
                AddInclude("MovilizacionMedios.Pasos.AportacionMedio.Capacidad");
                AddInclude("MovilizacionMedios.Pasos.AportacionMedio.TipoAdministracion");
                AddInclude("MovilizacionMedios.Pasos.DespliegueMedio");
                AddInclude("MovilizacionMedios.Pasos.DespliegueMedio.Capacidad");
                AddInclude("MovilizacionMedios.Pasos.FinIntervencionMedio");
                AddInclude("MovilizacionMedios.Pasos.FinIntervencionMedio.Capacidad");
                AddInclude("MovilizacionMedios.Pasos.LlegadaBaseMedio");
                AddInclude("MovilizacionMedios.Pasos.LlegadaBaseMedio.Capacidad");

                // Relaciones para las tablas con "Auditoria"
                //AddInclude(d => d.AuditoriaEmergenciaNacional);

                //AddInclude(d => d.AuditoriaActivacionPlanEmergencias.Where(dir => !dir.Borrado));
                //AddInclude("AuditoriaActivacionPlanEmergencias.TipoPlan");
                //AddInclude("AuditoriaActivacionPlanEmergencias.PlanEmergencia");
                //AddInclude("AuditoriaActivacionPlanEmergencias.Archivo");

                //AddInclude(d => d.AuditoriaDeclaracionesZAGEP.Where(dir => !dir.Borrado));

                //AddInclude(d => d.AuditoriaActivacionSistemas.Where(dir => !dir.Borrado));
                //AddInclude("AuditoriaActivacionSistemas.TipoSistemaEmergencia");
                //AddInclude("AuditoriaActivacionSistemas.ModoActivacion");

                //AddInclude(d => d.AuditoriaConvocatoriasCECOD.Where(dir => !dir.Borrado));

                //AddInclude(d => d.AuditoriaNotificacionesEmergencias.Where(dir => !dir.Borrado));
                //AddInclude("AuditoriaNotificacionesEmergencias.TipoNotificacion");

                //AddInclude(d => d.AuditoriaMovilizacionMedios.Where(dir => !dir.Borrado));
                //AddInclude("AuditoriaMovilizacionMedios.Pasos.PasoMovilizacion");
                //AddInclude("AuditoriaMovilizacionMedios.Pasos.AuditoriaSolicitudMedio");
                //AddInclude("AuditoriaMovilizacionMedios.Pasos.AuditoriaSolicitudMedio.ProcedenciaMedio");
                //AddInclude("AuditoriaMovilizacionMedios.Pasos.AuditoriaSolicitudMedio.Archivo");
                //AddInclude("AuditoriaMovilizacionMedios.Pasos.AuditoriaTramitacionMedio");
                //AddInclude("AuditoriaMovilizacionMedios.Pasos.AuditoriaTramitacionMedio.DestinoMedio");
                //AddInclude("AuditoriaMovilizacionMedios.Pasos.AuditoriaCancelacionMedio");
                //AddInclude("AuditoriaMovilizacionMedios.Pasos.AuditoriaOfrecimientoMedio");
                //AddInclude("AuditoriaMovilizacionMedios.Pasos.AuditoriaAportacionMedio");
                //AddInclude("AuditoriaMovilizacionMedios.Pasos.AuditoriaAportacionMedio.Capacidad");
                //AddInclude("AuditoriaMovilizacionMedios.Pasos.AuditoriaAportacionMedio.TipoAdministracion");
                //AddInclude("AuditoriaMovilizacionMedios.Pasos.AuditoriaDespliegueMedio");
                //AddInclude("AuditoriaMovilizacionMedios.Pasos.AuditoriaDespliegueMedio.Capacidad");
                //AddInclude("AuditoriaMovilizacionMedios.Pasos.AuditoriaFinIntervencionMedio");
                //AddInclude("AuditoriaMovilizacionMedios.Pasos.AuditoriaFinIntervencionMedio.Capacidad");
                //AddInclude("AuditoriaMovilizacionMedios.Pasos.AuditoriaLlegadaBaseMedio");
                //AddInclude("AuditoriaMovilizacionMedios.Pasos.AuditoriaLlegadaBaseMedio.Capacidad");
            }
        }
    }
}
