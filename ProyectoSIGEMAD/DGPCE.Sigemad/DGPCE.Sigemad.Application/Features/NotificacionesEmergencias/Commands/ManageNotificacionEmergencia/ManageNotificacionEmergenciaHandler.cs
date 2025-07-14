using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Contracts.RegistrosActualizacion;
using DGPCE.Sigemad.Application.Dtos.NotificacionesEmergencias;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.ActuacionesRelevantesDGPCE;
using DGPCE.Sigemad.Application.Specifications.Incendios;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DGPCE.Sigemad.Application.Features.NotificacionesEmergencias.Commands.ManageNotificacionEmergencia;
public class ManageNotificacionEmergenciaHandler : IRequestHandler<ManageNotificacionEmergenciaCommand, ManageNotificacionEmergenciaResponse>
{
    private readonly ILogger<ManageNotificacionEmergenciaHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRegistroActualizacionService _registroActualizacionService;

    public ManageNotificacionEmergenciaHandler(
        ILogger<ManageNotificacionEmergenciaHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IRegistroActualizacionService registroActualizacionService
    )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _registroActualizacionService = registroActualizacionService;
    }

    public async Task<ManageNotificacionEmergenciaResponse> Handle(ManageNotificacionEmergenciaCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(ManageNotificacionEmergenciaHandler)} - BEGIN");

        await _registroActualizacionService.ValidarSuceso(request.IdSuceso);
        await ValidateTipoNotificacion(request);
        await ValidateFechas(request);
        await _unitOfWork.BeginTransactionAsync();


        try
        {
            RegistroActualizacion registroActualizacion = await _registroActualizacionService.GetOrCreateRegistroActualizacion<ActuacionRelevanteDGPCE>(
                request.IdRegistroActualizacion, request.IdSuceso, TipoRegistroActualizacionEnum.ActuacionRelevante);

            ActuacionRelevanteDGPCE actuacion = await GetOrCreateActuacionRelevante(request, registroActualizacion);

            var notificacionesOriginales = actuacion.NotificacionesEmergencias.ToDictionary(d => d.Id, d => _mapper.Map<ManageNotificacionEmergenciaDto>(d));
            MapAndSaveNotificaciones(request, actuacion);

            var notificacionesParaEliminar = await DeleteLogicalNotificaciones(request, actuacion, registroActualizacion.Id);

            await SaveActuacion(actuacion);

            await _registroActualizacionService.SaveRegistroActualizacion<
                ActuacionRelevanteDGPCE, NotificacionEmergencia, ManageNotificacionEmergenciaDto>(
                registroActualizacion,
                actuacion,
                ApartadoRegistroEnum.NotificacionesOficiales,
                notificacionesParaEliminar, notificacionesOriginales);

            await _unitOfWork.CommitAsync();

            _logger.LogInformation($"{nameof(ManageNotificacionEmergenciaResponse)} - END");

            return new ManageNotificacionEmergenciaResponse
            {
                IdActuacionRelevante = actuacion.Id,
                IdRegistroActualizacion = registroActualizacion.Id
            };

        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error en la transacción de ManageConvocatoriaCECODCommandHandler");
            throw;
        }
    }

    private async Task ValidateFechas(ManageNotificacionEmergenciaCommand request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (request.Detalles == null)
            throw new BadRequestException("Los detalles de la notificacion no pueden estar vacios.");

        var incendioAsociado = await _unitOfWork.Repository<Incendio>()
           .GetByIdWithSpec(new IncendioActiveByIdSpecification(request.IdSuceso))
           ?? throw new BadRequestException($"No se encontró el incendio con ID {request.IdSuceso}.");

        bool fechasValidas = request.Detalles.All(notificacion => notificacion.FechaHoraNotificacion > incendioAsociado.FechaInicio);
        bool fechasValidasActual = request.Detalles.All(notificacion => notificacion.FechaHoraNotificacion <= DateTime.Now);

        if (!fechasValidas)
        {
            throw new BadRequestException("Una o mas fechas es menor a la fecha del incendio asociado.");
        }

        if (!fechasValidasActual)
        {
            throw new BadRequestException("Una o mas fechas es mayor a la fecha del sistema.");
        }

    }

    private async Task SaveActuacion(ActuacionRelevanteDGPCE actuacion)
    {
        if (actuacion.Id > 0)
        {
            _unitOfWork.Repository<ActuacionRelevanteDGPCE>().UpdateEntity(actuacion);
        }
        else
        {
            _unitOfWork.Repository<ActuacionRelevanteDGPCE>().AddEntity(actuacion);
        }

        if (await _unitOfWork.Complete() <= 0)
            throw new Exception("No se pudo insertar/actualizar la ActuacionRelevanteDGPCE");
    }


    private async Task<List<int>> DeleteLogicalNotificaciones(ManageNotificacionEmergenciaCommand request, ActuacionRelevanteDGPCE actuacion, int idRegistroActualizacion)
    {
        if (actuacion.Id > 0)
        {
            var idsEnRequest = request.Detalles.Where(d => d.Id.HasValue && d.Id > 0).Select(d => d.Id).ToList();
            var notificacionesEmergenciaParaEliminar = actuacion.NotificacionesEmergencias
                .Where(d => d.Id > 0 && !idsEnRequest.Contains(d.Id))
                .ToList();

            if (notificacionesEmergenciaParaEliminar.Count == 0)
            {
                return new List<int>();
            }

            // Obtener el historial de creación de estas convocatorias
            var idsnotificacionesEmergenciaParaEliminar = notificacionesEmergenciaParaEliminar.Select(d => d.Id).ToList();
            var historialDirecciones = await _unitOfWork.Repository<DetalleRegistroActualizacion>()
                .GetAsync(d =>
                idsnotificacionesEmergenciaParaEliminar.Contains(d.IdReferencia) && d.IdApartadoRegistro == (int)ApartadoRegistroEnum.NotificacionesOficiales);

            foreach (var declaracion in notificacionesEmergenciaParaEliminar)
            {
                var historial = historialDirecciones.FirstOrDefault(d =>
                d.IdReferencia == declaracion.Id &&
                (d.IdEstadoRegistro == EstadoRegistroEnum.Creado || d.IdEstadoRegistro == EstadoRegistroEnum.CreadoYModificado));

                if (historial == null || historial.IdRegistroActualizacion != idRegistroActualizacion)
                {
                    throw new BadRequestException($"La Notificacion Oficial con ID {declaracion.Id} solo puede eliminarse en el registro en que fue creada.");
                }

                _unitOfWork.Repository<NotificacionEmergencia>().DeleteEntity(declaracion);
            }

            return idsnotificacionesEmergenciaParaEliminar;
        }

        return new List<int>();
    }


    private void MapAndSaveNotificaciones(ManageNotificacionEmergenciaCommand request, ActuacionRelevanteDGPCE actuacion)
    {
        foreach (var notificacionDto in request.Detalles)
        {
            if (notificacionDto.Id.HasValue && notificacionDto.Id > 0)
            {
                var notificacionExistente = actuacion.NotificacionesEmergencias.FirstOrDefault(d => d.Id == notificacionDto.Id.Value);
                if (notificacionExistente != null)
                {
                    var copiaOriginal = _mapper.Map<ManageNotificacionEmergenciaDto>(notificacionExistente);
                    var copiaNueva = _mapper.Map<ManageNotificacionEmergenciaDto>(notificacionDto);

                    if (!copiaOriginal.Equals(copiaNueva))
                    {
                        _mapper.Map(notificacionDto, notificacionExistente);
                        notificacionExistente.Borrado = false;
                    }
                }
                else
                {
                    actuacion.NotificacionesEmergencias.Add(_mapper.Map<NotificacionEmergencia>(notificacionDto));
                }
            }
            else
            {
                actuacion.NotificacionesEmergencias.Add(_mapper.Map<NotificacionEmergencia>(notificacionDto));
            }
        }
    }



    private async Task ValidateTipoNotificacion(ManageNotificacionEmergenciaCommand request)
    {
        var idsTipoNotificacion = request.Detalles.Select(c => c.IdTipoNotificacion).Distinct();
        var tipoNotificacionExistentes = await _unitOfWork.Repository<TipoNotificacion>().GetAsync(p => idsTipoNotificacion.Contains(p.Id));

        if (tipoNotificacionExistentes.Count() != idsTipoNotificacion.Count())
        {
            var idsTipoNotificacionesExistentes = tipoNotificacionExistentes.Select(p => p.Id).Cast<int>().ToList();
            var idsTipoNotificacionesInvalidas = idsTipoNotificacion.Except(idsTipoNotificacionesExistentes).ToList();

            if (idsTipoNotificacionesInvalidas.Any())
            {
                _logger.LogWarning($"Los siguientes Id's de tipo notificacion: {string.Join(", ", idsTipoNotificacionesInvalidas)}, no se encontraron");
                throw new NotFoundException(nameof(TipoPlan), string.Join(", ", idsTipoNotificacionesInvalidas));
            }
        }
    }

    private async Task<ActuacionRelevanteDGPCE> GetOrCreateActuacionRelevante(ManageNotificacionEmergenciaCommand request, RegistroActualizacion registroActualizacion)
    {
        if (registroActualizacion.IdReferencia > 0)
        {
            List<int> idsReferencias = new List<int>();
            bool includeEmergenciaNacional = false;
            List<int> idsActivacionPlanEmergencias = new List<int>();
            List<int> idsDeclaracionesZAGEP = new List<int>();
            List<int> idsActivacionSistemas = new List<int>();
            List<int> idsConvocatoriasCECOD = new List<int>();
            List<int> idsNotificacionesEmergencias = new List<int>();
            List<int> idsMovilizacionMedios = new List<int>();

            // Separar IdReferencia según su tipo
            foreach (var detalle in registroActualizacion.DetallesRegistro)
            {
                switch (detalle.IdApartadoRegistro)
                {
                    case (int)ApartadoRegistroEnum.EmergenciaNacional:
                        includeEmergenciaNacional = true;
                        break;
                    case (int)ApartadoRegistroEnum.ActivacionDePlanes:
                        idsActivacionPlanEmergencias.Add(detalle.IdReferencia);
                        break;
                    case (int)ApartadoRegistroEnum.DeclaracionZAGEP:
                        idsDeclaracionesZAGEP.Add(detalle.IdReferencia);
                        break;
                    case (int)ApartadoRegistroEnum.ActivacionDeSistemas:
                        idsActivacionSistemas.Add(detalle.IdReferencia);
                        break;
                    case (int)ApartadoRegistroEnum.ConvocatoriaCECOD:
                        idsConvocatoriasCECOD.Add(detalle.IdReferencia);
                        break;
                    case (int)ApartadoRegistroEnum.NotificacionesOficiales:
                        idsNotificacionesEmergencias.Add(detalle.IdReferencia);
                        break;
                    case (int)ApartadoRegistroEnum.MovilizacionMedios:
                        idsMovilizacionMedios.Add(detalle.IdReferencia);
                        break;
                    default:
                        idsReferencias.Add(detalle.IdReferencia);
                        break;
                }
            }

            // Buscar la notificaciones oficiales por IdReferencia
            var actuacionRelevante = await _unitOfWork.Repository<ActuacionRelevanteDGPCE>()
                .GetByIdWithSpec(new ActuacionRelevanteDGPCEWithFilteredData(registroActualizacion.IdReferencia, idsActivacionPlanEmergencias, idsDeclaracionesZAGEP, idsActivacionSistemas, idsConvocatoriasCECOD, idsNotificacionesEmergencias, idsMovilizacionMedios, includeEmergenciaNacional));

            if (actuacionRelevante is null || actuacionRelevante.Borrado)
                throw new BadRequestException($"El registro de actualización con Id [{registroActualizacion.Id}] no tiene registro de actuacionRelevanteDGPCE");

            return actuacionRelevante;
        }

        // Validar si ya existe un registro de notificaciones oficiales para este suceso
        var specSuceso = new ActuacionRelevanteDGPCEWithNotificacionesOficiales(new ActuacionRelevanteDGPCESpecificationParams { IdSuceso = request.IdSuceso });
        var notificacionOficialExistente = await _unitOfWork.Repository<ActuacionRelevanteDGPCE>().GetByIdWithSpec(specSuceso);

        return notificacionOficialExistente ?? new ActuacionRelevanteDGPCE { IdSuceso = request.IdSuceso };
    }


}
