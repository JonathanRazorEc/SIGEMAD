using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.DeclaracionesZAGEP;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.Auditoria.Vms;
using DGPCE.Sigemad.Application.Features.SucesosRelacionados.Vms;
using DGPCE.Sigemad.Application.Specifications.ActuacionesRelevantesDGPCE;
using DGPCE.Sigemad.Application.Specifications.Auditoria;
using DGPCE.Sigemad.Application.Specifications.Evoluciones;
using DGPCE.Sigemad.Application.Specifications.SucesosRelacionados;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.Auditoria.Queries.GetAuditoriaIncendio
{
    public class GetAuditoriaIncendioQueryHandler : IRequestHandler<GetAuditoriaIncendioQuery, AuditoriaIncendioVm>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAuditoriaIncendioQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AuditoriaIncendioVm> Handle(GetAuditoriaIncendioQuery request, CancellationToken cancellationToken)
        {
            // 1. Obtener el registro principal de Auditoria_Incendio
            var specAuditoria = new AuditoriaIncendioByIncendioIdSpec(request.IdIncendio);

            var auditoria = await _unitOfWork.Repository<Auditoria_Incendio>().GetByIdWithSpec(specAuditoria);

            if (auditoria == null)
            {

                return new AuditoriaIncendioVm { };
            }
            else
            {
                var auditoriaSpec = new ActuacionRelevanteAuditoriaSpecification(new ActuacionRelevanteDGPCESpecificationParams
                {
                    IdSuceso = auditoria.IdSuceso
                });

                var movilizacionMediosSpec = new AuditoriaMovilizacionMedioBySucesoSpec(auditoria.IdSuceso);
                var convocatoriaSpec = new AuditoriaConvocatoriaCECODBySucesoSpec(auditoria.IdSuceso);
                var planEmergenciasSpec = new AuditoriaActivacionPlanEmergenciaBySucesoSpec(auditoria.IdSuceso);
                var notificacionseOficialesSpec = new AuditoriaNotificacionesOficialesBySucesoSpec(auditoria.IdSuceso);
                var declaracionesZAGEPSpec = new AuditoriaDeclaracionZAGEPBySucesoSpec(auditoria.IdSuceso);
                var activacionSistemaSpec = new AuditoriaActivacionSistemaBySucesoSpec(auditoria.IdSuceso);
                var emergenciaNacionalSpec = new AuditoriaEmergenciaNacionalBySucesoSpec(auditoria.IdSuceso);
                var sucesoSpec = new SucesoWithTipoSucesoSpec(auditoria.IdSuceso);
                var sucesoRelacionadoSpec = new SucesosRelacionadosActiveByIdSucesoPrincipalSpecification(auditoria.IdSuceso);
                //var evolucionSpec = new EvolucionWithParametrosSpecification(auditoria.IdSuceso);

                var movilizacionMedios = await _unitOfWork.Repository<AuditoriaMovilizacionMedio>().GetAllWithSpec(movilizacionMediosSpec);
                var convocatoriasCecod = await _unitOfWork.Repository<AuditoriaConvocatoriaCECOD>().GetAllWithSpec(convocatoriaSpec);
                var planEmergencias = await _unitOfWork.Repository<AuditoriaActivacionPlanEmergencia>().GetAllWithSpec(planEmergenciasSpec);
                var notificacionesOficiales = await _unitOfWork.Repository<AuditoriaNotificacionEmergencia>().GetAllWithSpec(notificacionseOficialesSpec);
                var activacionSistemas = await _unitOfWork.Repository<AuditoriaActivacionSistema>().GetAllWithSpec(activacionSistemaSpec);
                var declaracionesZAGEP = await _unitOfWork.Repository<AuditoriaDeclaracionZAGEP>().GetAllWithSpec(declaracionesZAGEPSpec);
                var emergenciaNacional = await _unitOfWork.Repository<AuditoriaEmergenciaNacional>().GetByIdWithSpec(emergenciaNacionalSpec);
                var suceso = await _unitOfWork.Repository<Suceso>().GetByIdWithSpec(sucesoSpec);
                var sucesoRelacionado = await _unitOfWork.Repository<SucesoRelacionado>().GetByIdWithSpec(sucesoRelacionadoSpec);
               // var evolucion = await _unitOfWork.Repository<Evolucion>().GetByIdWithSpec(evolucionSpec);

               // var ultimoEstadoIncendio = evolucion?.Parametros?.OrderByDescending(p => p.FechaCreacion).FirstOrDefault()?.EstadoIncendio?.Descripcion;



                var vistaPrimaria = new VistaPrimariaVm
                {
                    Id = auditoria.Id,
                    Inicio = auditoria.FechaInicio,
                    //EstadoIncendio = ultimoEstadoIncendio,
                    Denominacion = auditoria.Denominacion,
                    Provincia = auditoria.Provincia?.Descripcion,
                    NotaGeneral = auditoria.NotaGeneral,
                    SituacionOperativa = auditoria.Parametro?.IdSituacionEquivalente,
                    Municipio = auditoria.Municipio?.Descripcion,
                    //SuperficieAfectadaHa = auditoria.Parametro?.SuperficieAfectadaHectarea,
                    Seguimiento = auditoria.EstadoSuceso.Descripcion,
                    EstadoSuceso = auditoria.EstadoSuceso?.Descripcion,
                    ClaseSuceso = auditoria.ClaseSuceso?.Descripcion
                };

                var movilizacionMediosExtraOrdinariosVms = movilizacionMedios
                  .SelectMany(movilizacion => movilizacion.Pasos
                    .Where(paso => paso.AuditoriaSolicitudMedio != null)
                    .Select(paso => new AuditoriaMovilizacionMediosExtraordinariosVm
                    {
                        Solicitante = paso.AuditoriaSolicitudMedio.AutoridadSolicitante,
                        IdPasoMovilizacion = paso.IdPasoMovilizacion,
                        UltActualizacion = paso.FechaModificacion
                    })
                  )
                  .ToList();

                var convocatoriasCECODVms = convocatoriasCecod
                  .Select(c => new AuditoriaConvocatoriasCECODVm
                  {
                      FechaInicio = c.FechaInicio,
                      FechaFin = c.FechaFin,
                      Lugar = c.Lugar,
                      Convocados = c.Convocados
                  })
                  .ToList();

                var activacionPlanesEmergenciaVms = planEmergencias
                  .Select(a => new AuditoriaActivacionPlanesEmergenciaVm
                  {
                      TipoPlan = a.TipoPlan.Descripcion,
                      PlanEmergencia = a.PlanEmergencia?.Descripcion,
                      FechaInicio = a.FechaInicio,
                      FechaFinal = a.FechaFin,
                      Autoridad = a.Autoridad
                  })
                  .ToList();

                var notificacionesOficialesVm = notificacionesOficiales
                  .Select(n => new AuditoriaNotificacionesOficialesVm
                  {
                      TipoNotificacion = n.TipoNotificacion.Descripcion,
                      Fecha = n.FechaHoraNotificacion,
                      OrganosSNPCNotificados = n.OrganosNotificados,
                      OrganismoInternacional = n.OrganismoInternacional
                  })
                  .ToList();

                var activacionSistemaVms = activacionSistemas
                  .Select(a => new AuditoriaActivacionSistemaVm
                  {
                      //TipoActivacion = a.IdTipoSistemaEmergencia,
                      TipoActivacion = a.TipoSistemaEmergencia.Descripcion,
                      FechaActivacion = a.FechaHoraActivacion,
                      FechaActualizacion = a.FechaHoraActualizacion
                  })
                  .ToList();

                var auditoriaDeclaracionZAGEPVms = declaracionesZAGEP
                  .Select(d => new AuditoriaDeclaracionZAGEPVm
                  {
                      Fecha = d.FechaSolicitud,
                      Denominacion = d.Denominacion
                  })
                  .ToList();

                var declaracionEmergenciaInteresNacionalVm = emergenciaNacional != null ?
                  new AuditoriaDeclaracionEmergenciaInteresNacionalVm
                  {
                      AutoridadSolicitante = emergenciaNacional.Autoridad,
                      DescripcionDeLaSolicitud = emergenciaNacional.DescripcionSolicitud,
                      FechaSolicitud = emergenciaNacional.FechaHoraSolicitud,
                      FechaDeclaracion = emergenciaNacional.FechaHoraDeclaracion
                  } :
                  null;

                var sucesosRelacionadosVm = sucesoRelacionado?.DetalleSucesoRelacionados
                    .Where(detalle =>
                    detalle.SucesoAsociado != null &&
                    detalle.SucesoAsociado.TipoSuceso != null &&
                    detalle.SucesoAsociado.Incendios != null &&
                    detalle.SucesoAsociado.Incendios.Any())
                    .Select(detalle => new AuditoriaSucesosRelacionadosVm
                    {
                    TipoSuceso = detalle.SucesoAsociado.TipoSuceso.Descripcion,
                    FechaCreacion = detalle.SucesoAsociado.FechaCreacion,
                    Denominacion = detalle.SucesoAsociado.Incendios[0].Denominacion,
                    EstadoSuceso = detalle.SucesoAsociado.Incendios[0].EstadoSuceso?.Descripcion
                    })
                    .ToList();


                return new AuditoriaIncendioVm
                {
                    VistaPrimaria = vistaPrimaria,
                    MovilizacionMediosExtraOrdinarios = movilizacionMediosExtraOrdinariosVms,
                    ConvocatoriasCECOD = convocatoriasCECODVms,
                    ActivacionPlanEmergencias = activacionPlanesEmergenciaVms,
                    NotificacionesOficiales = notificacionesOficialesVm,
                    ActivacionSistemas = activacionSistemaVms,
                    DeclaracionZAGEP = auditoriaDeclaracionZAGEPVms,
                    DeclaracionEmergenciaInteresNacional = declaracionEmergenciaInteresNacionalVm,
                    //SucesoRelacionado = sucesosRelacionados
                };

                //return new AuditoriaIncendioVm {  };
            }

        }
    }
}