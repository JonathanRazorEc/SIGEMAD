using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.ActuacionesRelevantes;
using DGPCE.Sigemad.Application.Dtos.Evoluciones;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.Evoluciones.Queries.GetEvolucion;
using DGPCE.Sigemad.Application.Specifications.ActuacionesRelevantesDGPCE;
using DGPCE.Sigemad.Application.Specifications.Evoluciones;
using DGPCE.Sigemad.Application.Specifications.RegistrosActualizaciones;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.ActuacionesRelevantes.Queries;
public class GetActuacionRelevanteQueryHandler : IRequestHandler<GetActuacionRelevanteQuery, ActuacionRelevanteDGPCEDto>
{
    private readonly ILogger<GetActuacionRelevanteQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;


    public GetActuacionRelevanteQueryHandler(
    ILogger<GetActuacionRelevanteQueryHandler> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ActuacionRelevanteDGPCEDto> Handle(GetActuacionRelevanteQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(GetActuacionRelevanteQueryHandler)} - BEGIN");

        ActuacionRelevanteDGPCE actuacion;
        List<int> idsActivacionPlanEmergencias = new List<int>();
        List<int> idsDeclaracionesZAGEP = new List<int>();
        List<int> idsActivacionSistemas = new List<int>();
        List<int> idsConvocatoriasCECOD = new List<int>();
        List<int> idsNotificacionesEmergencias = new List<int>();
        List<int> idsMovilizacionMedios = new List<int>();

        List<int> idsActivacionPlanEmergenciasEliminables = new List<int>();
        List<int> idsDeclaracionesZAGEPEliminables = new List<int>();
        List<int> idsActivacionSistemasEliminables = new List<int>();
        List<int> idsConvocatoriasCECODEliminables = new List<int>();
        List<int> idsNotificacionesEmergenciasEliminables = new List<int>();
        List<int> idsMovilizacionMediosEliminables = new List<int>();

        bool includeEmergenciaNacional = false;

        if (request.IdRegistroActualizacion.HasValue)
        {
            _logger.LogInformation($"Buscando ActuacionRelevante para IdRegistroActualizacion: {request.IdRegistroActualizacion}");
            // Obtener RegistroActualizacion con IdReferencia (Evolucion.Id)
            var registroSpec = new RegistroActualizacionSpecificationParams
            {
                Id = request.IdRegistroActualizacion.Value
            };
            var registroActualizacion = await _unitOfWork.Repository<RegistroActualizacion>()
                .GetByIdWithSpec(new RegistroActualizacionSpecification(registroSpec));

            if (registroActualizacion == null)
            {
                _logger.LogWarning($"No se encontró RegistroActualizacion con Id: {request.IdRegistroActualizacion}");
                throw new NotFoundException(nameof(RegistroActualizacion), request.IdRegistroActualizacion);
            }

            foreach (var detalle in registroActualizacion.DetallesRegistro)
            {
                switch (detalle.IdApartadoRegistro)
                {
                    case (int)ApartadoRegistroEnum.EmergenciaNacional:
                        includeEmergenciaNacional = true;
                        break;
                    case (int)ApartadoRegistroEnum.ActivacionDePlanes:
                        idsActivacionPlanEmergencias.Add(detalle.IdReferencia);
                        if (IsEliminable(detalle.IdEstadoRegistro)) idsActivacionPlanEmergenciasEliminables.Add(detalle.IdReferencia);
                        break;

                    case (int)ApartadoRegistroEnum.DeclaracionZAGEP:
                        idsDeclaracionesZAGEP.Add(detalle.IdReferencia);
                        if (IsEliminable(detalle.IdEstadoRegistro)) idsDeclaracionesZAGEPEliminables.Add(detalle.IdReferencia);
                        break;

                    case (int)ApartadoRegistroEnum.ActivacionDeSistemas:
                        idsActivacionSistemas.Add(detalle.IdReferencia);
                        if (IsEliminable(detalle.IdEstadoRegistro)) idsActivacionSistemasEliminables.Add(detalle.IdReferencia);
                        break;

                    case (int)ApartadoRegistroEnum.ConvocatoriaCECOD:
                        idsConvocatoriasCECOD.Add(detalle.IdReferencia);
                        if (IsEliminable(detalle.IdEstadoRegistro)) idsConvocatoriasCECODEliminables.Add(detalle.IdReferencia);
                        break;

                    case (int)ApartadoRegistroEnum.NotificacionesOficiales:
                        idsNotificacionesEmergencias.Add(detalle.IdReferencia);
                        if (IsEliminable(detalle.IdEstadoRegistro)) idsNotificacionesEmergenciasEliminables.Add(detalle.IdReferencia);
                        break;

                    case (int)ApartadoRegistroEnum.MovilizacionMedios:
                        idsMovilizacionMedios.Add(detalle.IdReferencia);
                        if (IsEliminable(detalle.IdEstadoRegistro)) idsMovilizacionMediosEliminables.Add(detalle.IdReferencia);
                        break;
                }
            }

            // Buscar la Evolución por IdReferencia
            actuacion = await _unitOfWork.Repository<ActuacionRelevanteDGPCE>()
         .GetByIdWithSpec(new ActuacionRelevanteDGPCEWithFilteredData(registroActualizacion.IdReferencia, idsActivacionPlanEmergencias, idsDeclaracionesZAGEP, idsActivacionSistemas, idsConvocatoriasCECOD, idsNotificacionesEmergencias, idsMovilizacionMedios, includeEmergenciaNacional));
        }
        else
        {
            _logger.LogInformation($"Buscando actuacion para IdSuceso: {request.IdSuceso}");
            // Buscar Evolución por IdSuceso
            actuacion = await _unitOfWork.Repository<ActuacionRelevanteDGPCE>()
                .GetByIdWithSpec(new ActuacionRelevanteDGPCEActiveByIdSpecification(new ActuacionRelevanteDGPCESpecificationParams { IdSuceso = request.IdSuceso }));
        }

        if (actuacion == null)
        {
            _logger.LogWarning($"No se encontró Actuacion para IdSuceso: {request.IdSuceso}");
            throw new NotFoundException(nameof(Evolucion), request.IdSuceso);
        }

        // Mapear actuacion a ActuacionRelevanteDGPCEDto
        var actuacionDto = _mapper.Map<ActuacionRelevanteDGPCEDto>(actuacion);


        actuacionDto.ActivacionPlanEmergencias.ForEach(activacionPlanEmergencias => activacionPlanEmergencias.EsEliminable = idsActivacionPlanEmergenciasEliminables.Contains(activacionPlanEmergencias.Id));
        actuacionDto.DeclaracionesZAGEP.ForEach(declaracionesZAGEP => declaracionesZAGEP.EsEliminable = idsDeclaracionesZAGEPEliminables.Contains(declaracionesZAGEP.Id));
        actuacionDto.ActivacionSistemas.ForEach(activacionSistemas => activacionSistemas.EsEliminable = idsActivacionSistemasEliminables.Contains(activacionSistemas.Id));
        actuacionDto.ConvocatoriasCECOD.ForEach(convocatoriasCECOD => convocatoriasCECOD.EsEliminable = idsConvocatoriasCECODEliminables.Contains(convocatoriasCECOD.Id));
        actuacionDto.NotificacionesEmergencias.ForEach(notificacionesEmergencias => notificacionesEmergencias.EsEliminable = idsNotificacionesEmergenciasEliminables.Contains(notificacionesEmergencias.Id));
        actuacionDto.MovilizacionMedios.ForEach(movilizacionMedios => movilizacionMedios.EsEliminable = idsMovilizacionMediosEliminables.Contains(movilizacionMedios.Id.Value));


        _logger.LogInformation($"{nameof(GetActuacionRelevanteQueryHandler)} - END");

        return actuacionDto;
    }

    private bool IsEliminable(EstadoRegistroEnum estado)
    {
        return estado == EstadoRegistroEnum.Creado ||
            estado == EstadoRegistroEnum.CreadoYModificado;
    }


}
