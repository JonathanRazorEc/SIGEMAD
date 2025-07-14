using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Files;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Contracts.RegistrosActualizacion;
using DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios;
using DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios.Pasos;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.EmergenciasNacionales.Commands.ManageEmergenciasNacionales;
using DGPCE.Sigemad.Application.Specifications.ActuacionesRelevantesDGPCE;
using DGPCE.Sigemad.Application.Specifications.Incendios;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.MovilizacionMedios.Commands.ManageMovilizacionMedios;
public class ManageMovilizacionMediosCommandHandler : IRequestHandler<ManageMovilizacionMediosCommand, ManageMovilizacionMediosResponse>
{
    private readonly ILogger<ManageMovilizacionMediosCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;
    private readonly IRegistroActualizacionService _registroActualizacionService;

    private const string ARCHIVOS_PATH = "movilizacion-medios";

    public ManageMovilizacionMediosCommandHandler(
        ILogger<ManageMovilizacionMediosCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IFileService fileService,
        IRegistroActualizacionService registroActualizacionService
    )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fileService = fileService;
        _registroActualizacionService = registroActualizacionService;
    }

    public async Task<ManageMovilizacionMediosResponse> Handle(ManageMovilizacionMediosCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(ManageMovilizacionMediosCommandHandler)} - BEGIN");

        await ValidateIdsAsync(request);
        await ValidarFlujoPasos(request);
        await ValidateFechas(request);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            RegistroActualizacion registroActualizacion = await _registroActualizacionService.GetOrCreateRegistroActualizacion<ActuacionRelevanteDGPCE>(
                request.IdRegistroActualizacion, request.IdSuceso, TipoRegistroActualizacionEnum.ActuacionRelevante);

            ActuacionRelevanteDGPCE actuacionRelevante = await GetOrCreateActuacionRelevanteAsync(request, registroActualizacion);

            Dictionary<int, MovilizacionMedioDto> movilizacionesOriginales = actuacionRelevante.MovilizacionMedios.ToDictionary(m => m.Id, m => _mapper.Map<MovilizacionMedioDto>(m));
            await MapAndManageMovilizaciones(request, actuacionRelevante);

            List<int> idsMovilizacionesParaEliminar = await DeleteLogicalMovilizaciones(request, actuacionRelevante, registroActualizacion.Id);

            await SaveActuacion(actuacionRelevante);

            await _registroActualizacionService.SaveRegistroActualizacion<
                ActuacionRelevanteDGPCE, MovilizacionMedio, MovilizacionMedioDto>(
                registroActualizacion,
                actuacionRelevante,
                ApartadoRegistroEnum.MovilizacionMedios,
                idsMovilizacionesParaEliminar, movilizacionesOriginales);

            await _unitOfWork.CommitAsync();

            _logger.LogInformation($"{nameof(ManageMovilizacionMediosCommandHandler)} - END");
            return new ManageMovilizacionMediosResponse
            {
                IdActuacionRelevante = actuacionRelevante.Id,
                IdRegistroActualizacion = registroActualizacion.Id
            };

        }
        catch (Exception ex)
        {

            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error en la transacción de CreateOrUpdateDireccionCommandHandler");
            throw;
        }
    }

    private async Task ValidateFechas(ManageMovilizacionMediosCommand request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (request.Movilizaciones == null)
            throw new BadRequestException("Loas movilizaciones no pueden estar vacias.");

        var incendioAsociado = await _unitOfWork.Repository<Incendio>()
           .GetByIdWithSpec(new IncendioActiveByIdSpecification(request.IdSuceso))
           ?? throw new BadRequestException($"No se encontró el incendio con ID {request.IdSuceso}.");

        foreach (var movilizacion in request.Movilizaciones)
        {
            
            var solicitud = movilizacion.Pasos.OfType<ManageSolicitudMedioDto>().FirstOrDefault();
            var ofrecimiento = movilizacion.Pasos.OfType<ManageOfrecimientoMedioDto>().FirstOrDefault();
            var aportacion = movilizacion.Pasos.OfType<ManageAportacionMedioDto>().FirstOrDefault();
            var despliegue = movilizacion.Pasos.OfType<ManageDespliegueMedioDto>().FirstOrDefault();
            var cancelacion = movilizacion.Pasos.OfType<ManageCancelacionMedioDto>().FirstOrDefault(); // si existe
            var finIntervencion = movilizacion.Pasos.OfType<ManageFinIntervencionMedioDto>().FirstOrDefault();
            var llegadaBase = movilizacion.Pasos.OfType<ManageLlegadaBaseMedioDto>().FirstOrDefault();
            var tramitacion = movilizacion.Pasos.OfType<ManageTramitacionMedioDto>().FirstOrDefault();

            //Solicitud
            if (solicitud != null)
            {
                if(solicitud.FechaHoraSolicitud < incendioAsociado.FechaInicio)
                    throw new BadRequestException("La fecha de solicitud no puede ser menor que la fecha del incendio.");
               

                if (solicitud.FechaHoraSolicitud > DateTime.Now)
                    throw new BadRequestException("La fecha de solicitud no puede ser mayor que la fecha del sistema.");
            }          

            if(tramitacion != null)
            {
                if(solicitud != null)
                {
                    if (tramitacion.FechaHoraTramitacion < solicitud.FechaHoraSolicitud)
                        throw new BadRequestException("La fecha de tramitación no puede ser menor que la fecha de solicitud.");

                    if (tramitacion.FechaHoraTramitacion > DateTime.Now)
                        throw new BadRequestException("La fecha de tramitación no puede ser mayor que la fecha del sistema.");
                }

            }
           

            // Ofrecimiento
            if (ofrecimiento != null)
            {
                if (ofrecimiento.FechaHoraOfrecimiento < incendioAsociado.FechaInicio)
                    throw new BadRequestException("La fecha de ofrecimiento no puede ser menor que la fecha del incendio.");

                if (ofrecimiento.FechaHoraOfrecimiento > DateTime.Now)
                    throw new BadRequestException("La fecha de ofrecimiento no puede ser mayor que la fecha del sistema.");

                if (ofrecimiento.FechaHoraDisponibilidad.HasValue)
                {
                    if (ofrecimiento.FechaHoraDisponibilidad < ofrecimiento.FechaHoraOfrecimiento)
                        throw new BadRequestException("La fecha de disponibilidad no puede ser menor que la fecha de ofrecimiento.");

                    if (ofrecimiento.FechaHoraDisponibilidad > DateTime.Now)
                        throw new BadRequestException("La fecha de disponibilidad no puede ser mayor que la fecha del sistema.");

                }            

                
            }

            //Cancelación

            if (cancelacion != null && solicitud != null)
            {
                if (cancelacion.FechaHoraCancelacion < solicitud.FechaHoraSolicitud)
                    throw new BadRequestException("La fecha de cancelación no puede ser menor que la fecha de solicitud.");

                if (cancelacion.FechaHoraCancelacion > DateTime.Now)
                    throw new BadRequestException("La fecha de cancelación no puede ser mayor que la fecha del sistema.");
            }          
            

            // Aportación
            if (aportacion != null)
            {
                if (ofrecimiento != null)
                {
                    if (aportacion.FechaHoraAportacion < ofrecimiento.FechaHoraOfrecimiento)
                        throw new BadRequestException("La fecha de aportación no puede ser menor que la fecha de ofrecimiento.");

                    if (aportacion.FechaHoraAportacion > DateTime.Now)
                        throw new BadRequestException("La fecha de aportación no puede ser mayor que la fecha del sistema.");
                }
                else
                {
                    if (aportacion.FechaHoraAportacion < incendioAsociado.FechaInicio)
                        throw new BadRequestException("La fecha de aportación no puede ser menor que la fecha del incendio.");

                    if (aportacion.FechaHoraAportacion > DateTime.Now)
                        throw new BadRequestException("La fecha de aportación no puede ser mayor que la fecha del sistema.");
                }
            }

            // Despliegue
            if (despliegue != null)
            {

                if (aportacion != null)
                {
                    if (despliegue.FechaHoraDespliegue < aportacion.FechaHoraAportacion)
                        throw new BadRequestException("La fecha de despliegue no puede ser menor que la fecha de aportación.");

                    if (despliegue.FechaHoraDespliegue > DateTime.Now)
                        throw new BadRequestException("La fecha de despliegue no puede ser mayor que la fecha del sistema.");
                }

                if (despliegue.FechaHoraInicioIntervencion.HasValue)
                {
                    if (despliegue.FechaHoraInicioIntervencion < despliegue.FechaHoraDespliegue)
                        throw new BadRequestException("La fecha de inicio de intervención no puede ser menor que la fecha de despliegue.");

                    if (despliegue.FechaHoraInicioIntervencion > DateTime.Now)
                        throw new BadRequestException("La fecha de inicio de intervención no puede ser mayor que la fecha del sistema.");
                }              

                
            }

            // Fin de intervención

            if (finIntervencion != null && finIntervencion.FechaHoraInicioIntervencion.HasValue &&
                despliegue?.FechaHoraInicioIntervencion.HasValue == true)
            {
                if (finIntervencion.FechaHoraInicioIntervencion < despliegue.FechaHoraInicioIntervencion)
                {
                    throw new BadRequestException("La fecha de fin de intervención no puede ser menor que la fecha de inicio de intervención.");
                }

                if (finIntervencion.FechaHoraInicioIntervencion > DateTime.Now)
                {
                    throw new BadRequestException("La fecha de fin de intervención no puede ser mauor que la fecha del sistema.");
                }
            }

            // Llegada a base
            if (llegadaBase?.FechaHoraLlegada.HasValue == true &&
                finIntervencion?.FechaHoraInicioIntervencion.HasValue == true)
            {
                if (llegadaBase.FechaHoraLlegada < finIntervencion.FechaHoraInicioIntervencion)
                {
                    throw new BadRequestException("La fecha de llegada no puede ser menor que la fecha de fin de intervención.");
                }

                if (llegadaBase.FechaHoraLlegada > DateTime.Now)
                {
                    throw new BadRequestException("La fecha de llegada no puede ser mayor que la fecha del sistema.");
                }
            }

        }
    }

    private async Task MapAndManageMovilizaciones(ManageMovilizacionMediosCommand request, ActuacionRelevanteDGPCE actuacionRelevante)
    {
        foreach (var movilizacionDto in request.Movilizaciones)
        {
            if (movilizacionDto.Id.HasValue && movilizacionDto.Id > 0)
            {
                var movilizacionExistente = actuacionRelevante.MovilizacionMedios.FirstOrDefault(c => c.Id == movilizacionDto.Id.Value);

                if (movilizacionExistente != null)
                {
                    movilizacionExistente.Borrado = false;

                    foreach (var pasoDto in movilizacionDto.Pasos)
                    {
                        var ejecucionPasoExistente = movilizacionExistente.Pasos
                            .FirstOrDefault(p => p.Id == pasoDto.Id);

                        if (ejecucionPasoExistente != null)
                        {
                            // Si el paso existe, actualizarlo
                            ejecucionPasoExistente.Borrado = false;
                            await ActualizarPasoEspecifico(ejecucionPasoExistente, pasoDto);
                        }
                        else
                        {
                            // 🔹 Si el paso no existe, crearlo
                            var nuevoEjecucionPaso = new EjecucionPaso
                            {
                                IdPasoMovilizacion = (int)pasoDto.TipoPaso,
                            };

                            await AgregarPasoEspecifico(nuevoEjecucionPaso, pasoDto);

                            movilizacionExistente.Pasos.Add(nuevoEjecucionPaso);
                        }
                    }

                    await EliminarPasosNoEnRequest(movilizacionExistente, movilizacionDto);
                }
                else
                {
                    var nuevaMovilizacion = await CreateMovilizacion(movilizacionDto);
                    actuacionRelevante.MovilizacionMedios.Add(nuevaMovilizacion);
                }
            }
            else
            {
                var nuevaMovilizacion = await CreateMovilizacion(movilizacionDto);
                actuacionRelevante.MovilizacionMedios.Add(nuevaMovilizacion);
            }
        }
    }

    private async Task<List<int>> DeleteLogicalMovilizaciones(ManageMovilizacionMediosCommand request, ActuacionRelevanteDGPCE actuacion, int idRegistroActualizacion)
    {
        if (actuacion.Id > 0)
        {
            var idsEnRequest = request.Movilizaciones.Where(d => d.Id.HasValue && d.Id > 0).Select(d => d.Id).ToList();
            var movilizacionesParaEliminar = actuacion.MovilizacionMedios
                .Where(d => d.Id > 0 && d.Borrado == false && !idsEnRequest.Contains(d.Id))
                .ToList();

            if (movilizacionesParaEliminar.Count == 0)
            {
                return new List<int>();
            }

            // Obtener el historial de creación de estas convocatorias
            var idsmovilizacionesParaEliminar = movilizacionesParaEliminar.Select(d => d.Id).ToList();
            var historialDirecciones = await _unitOfWork.Repository<DetalleRegistroActualizacion>()
                .GetAsync(d =>
                idsmovilizacionesParaEliminar.Contains(d.IdReferencia) && d.IdApartadoRegistro == (int)ApartadoRegistroEnum.MovilizacionMedios);

            foreach (var movilizacion in movilizacionesParaEliminar)
            {
                var historial = historialDirecciones.FirstOrDefault(d =>
                d.IdReferencia == movilizacion.Id &&
                (d.IdEstadoRegistro == EstadoRegistroEnum.Creado || d.IdEstadoRegistro == EstadoRegistroEnum.CreadoYModificado));

                if (historial == null || historial.IdRegistroActualizacion != idRegistroActualizacion)
                {
                    throw new BadRequestException($"La movilizacion con ID {movilizacion.Id} solo puede eliminarse en el registro en que fue creada.");
                }

                foreach (var pasoAEliminar in movilizacion.Pasos)
                {
                    _unitOfWork.Repository<EjecucionPaso>().DeleteEntity(pasoAEliminar);
                }

                _unitOfWork.Repository<MovilizacionMedio>().DeleteEntity(movilizacion);
            }

            return idsmovilizacionesParaEliminar;
        }

        return new List<int>();
    }


    private async Task<MovilizacionMedio> CreateMovilizacion(MovilizacionMedioDto movilizacionDto)
    {
        var nuevaMovilizacion = new MovilizacionMedio
        {
            Solicitante = movilizacionDto.Solicitante,
            Pasos = new List<EjecucionPaso>(),
        };

        foreach (var pasoDto in movilizacionDto.Pasos)
        {
            var ejecucionPaso = new EjecucionPaso
            {
                IdPasoMovilizacion = (int)pasoDto.TipoPaso,
            };

            await AgregarPasoEspecifico(ejecucionPaso, pasoDto);
            nuevaMovilizacion.Pasos.Add(ejecucionPaso);
        }

        return nuevaMovilizacion;
    }

    private async Task ActualizarPasoEspecifico(EjecucionPaso ejecucionPaso, DatosPasoBase pasoDto)
    {
        switch (pasoDto)
        {
            case ManageSolicitudMedioDto solicitud:
                if (ejecucionPaso.SolicitudMedio != null)
                {
                    _mapper.Map(solicitud, ejecucionPaso.SolicitudMedio);
                    ejecucionPaso.SolicitudMedio.Archivo = await MapArchivo(solicitud, ejecucionPaso.SolicitudMedio.Archivo);
                    ejecucionPaso.SolicitudMedio.Borrado = false;
                }
                break;

            case ManageTramitacionMedioDto tramitacion:
                if (ejecucionPaso.TramitacionMedio != null)
                {
                    _mapper.Map(tramitacion, ejecucionPaso.TramitacionMedio);
                    ejecucionPaso.TramitacionMedio.Borrado = false;
                }
                break;

            case ManageAportacionMedioDto aportacion:
                if (ejecucionPaso.AportacionMedio != null)
                {
                    _mapper.Map(aportacion, ejecucionPaso.AportacionMedio);
                    ejecucionPaso.AportacionMedio.Borrado = false;
                }
                break;

            case ManageCancelacionMedioDto cancelacion:
                if (ejecucionPaso.CancelacionMedio != null)
                {
                    _mapper.Map(cancelacion, ejecucionPaso.CancelacionMedio);
                    ejecucionPaso.CancelacionMedio.Borrado = false;
                }
                break;

            case ManageOfrecimientoMedioDto ofrecimiento:
                if (ejecucionPaso.OfrecimientoMedio != null)
                {
                    _mapper.Map(ofrecimiento, ejecucionPaso.OfrecimientoMedio);
                    ejecucionPaso.OfrecimientoMedio.Borrado = false;
                }
                break;

            case ManageDespliegueMedioDto despliegue:
                if (ejecucionPaso.DespliegueMedio != null)
                {
                    _mapper.Map(despliegue, ejecucionPaso.DespliegueMedio);
                    ejecucionPaso.DespliegueMedio.Borrado = false;
                }
                break;

            case ManageFinIntervencionMedioDto finIntervencion:
                if (ejecucionPaso.FinIntervencionMedio != null)
                {
                    _mapper.Map(finIntervencion, ejecucionPaso.FinIntervencionMedio);
                    ejecucionPaso.FinIntervencionMedio.Borrado = false;
                }
                break;

            case ManageLlegadaBaseMedioDto llegadaBase:
                if (ejecucionPaso.LlegadaBaseMedio != null)
                {
                    _mapper.Map(llegadaBase, ejecucionPaso.LlegadaBaseMedio);
                    ejecucionPaso.LlegadaBaseMedio.Borrado = false;
                }
                break;
        }
    }


    private async Task AgregarPasoEspecifico(EjecucionPaso ejecucionPaso, DatosPasoBase pasoDto)
    {
        switch (pasoDto)
        {
            case ManageSolicitudMedioDto solicitud:
                ejecucionPaso.SolicitudMedio = new SolicitudMedio
                {
                    IdProcedenciaMedio = solicitud.IdProcedenciaMedio,
                    AutoridadSolicitante = solicitud.AutoridadSolicitante,
                    FechaHoraSolicitud = solicitud.FechaHoraSolicitud,
                    Descripcion = solicitud.Descripcion,
                    Observaciones = solicitud.Observaciones,
                    Archivo = await MapArchivo(solicitud, null)
                };
                break;
            case ManageTramitacionMedioDto tramitacion:
                ejecucionPaso.TramitacionMedio = new TramitacionMedio
                {
                    IdDestinoMedio = tramitacion.IdDestinoMedio,
                    TitularMedio = tramitacion.TitularMedio,
                    FechaHoraTramitacion = tramitacion.FechaHoraTramitacion,
                    Descripcion = tramitacion.Descripcion,
                    Observaciones = tramitacion.Observaciones
                };
                break;
            case ManageCancelacionMedioDto cancelacion:
                ejecucionPaso.CancelacionMedio = new CancelacionMedio
                {
                    Motivo = cancelacion.Motivo,
                    FechaHoraCancelacion = cancelacion.FechaHoraCancelacion,
                };
                break;
            case ManageDespliegueMedioDto despliegue:
                ejecucionPaso.DespliegueMedio = new DespliegueMedio
                {
                    IdCapacidad = despliegue.IdCapacidad,
                    MedioNoCatalogado = despliegue.MedioNoCatalogado,
                    FechaHoraDespliegue = despliegue.FechaHoraDespliegue,
                    FechaHoraInicioIntervencion = despliegue.FechaHoraInicioIntervencion,
                    Observaciones = despliegue.Observaciones
                };
                break;
            case ManageOfrecimientoMedioDto ofrecimiento:
                ejecucionPaso.OfrecimientoMedio = new OfrecimientoMedio
                {
                    TitularMedio = ofrecimiento.TitularMedio,
                    GestionCECOD = ofrecimiento.GestionCECOD,
                    FechaHoraOfrecimiento = ofrecimiento.FechaHoraOfrecimiento,
                    FechaHoraDisponibilidad = ofrecimiento.FechaHoraDisponibilidad,
                    Descripcion = ofrecimiento.Descripcion,
                    Observaciones = ofrecimiento.Observaciones
                };
                break;
            case ManageFinIntervencionMedioDto finIntervencion:
                ejecucionPaso.FinIntervencionMedio = new FinIntervencionMedio
                {
                    IdCapacidad = finIntervencion.IdCapacidad,
                    MedioNoCatalogado = finIntervencion.MedioNoCatalogado,
                    FechaHoraInicioIntervencion = finIntervencion.FechaHoraInicioIntervencion,
                    Observaciones = finIntervencion.Observaciones
                };
                break;
            case ManageAportacionMedioDto aportacion:
                ejecucionPaso.AportacionMedio = new AportacionMedio
                {
                    IdTipoAdministracion = aportacion.IdTipoAdministracion,
                    IdCapacidad = aportacion.IdCapacidad,
                    TitularMedio = aportacion.TitularMedio,
                    MedioNoCatalogado = aportacion.MedioNoCatalogado,
                    FechaHoraAportacion = aportacion.FechaHoraAportacion,
                    Descripcion = aportacion.Descripcion,

                };
                break;
            case ManageLlegadaBaseMedioDto llegadaBase:
                ejecucionPaso.LlegadaBaseMedio = new LlegadaBaseMedio
                {
                    IdCapacidad = llegadaBase.IdCapacidad,
                    MedioNoCatalogado = llegadaBase.MedioNoCatalogado,
                    FechaHoraLlegada = llegadaBase.FechaHoraLlegada,
                    Observaciones = llegadaBase.Observaciones
                };
                break;
        };
    }

    private async Task EliminarPasosNoEnRequest(MovilizacionMedio movilizacionExistente, MovilizacionMedioDto movilizacionDto)
    {
        var idsPasosEnRequest = movilizacionDto.Pasos
            .Where(p => p.Id.HasValue && p.Id > 0)
            .Select(p => p.Id)
            .ToList();

        var pasosParaEliminar = movilizacionExistente.Pasos
            .Where(p => p.Id > 0 && p.Borrado == false && !idsPasosEnRequest.Contains(p.Id))
            .ToList();

        if (pasosParaEliminar.Any())
        {
            var ultimoPaso = movilizacionExistente.Pasos
                .OrderByDescending(p => p.FechaCreacion)
                .FirstOrDefault();

            foreach (var paso in pasosParaEliminar)
            {
                // 🔹 Si el paso que se intenta eliminar NO es el último, detener el proceso
                if (paso != ultimoPaso)
                {
                    throw new BadRequestException($"No se puede eliminar el paso con ID {paso.Id} porque no es el último paso registrado en la movilización con ID {movilizacionExistente.Id}.");
                }

                _unitOfWork.Repository<EjecucionPaso>().DeleteEntity(paso);
            }
        }

        await Task.CompletedTask;
    }

    private async Task<Archivo?> MapArchivo(ManageSolicitudMedioDto manageSolicitudMedioDto, Archivo? archivoExistente)
    {
        if (manageSolicitudMedioDto.Archivo != null)
        {
            var fileEntity = new Archivo
            {
                NombreOriginal = manageSolicitudMedioDto.Archivo?.FileName ?? string.Empty,
                NombreUnico = $"{Path.GetFileNameWithoutExtension(manageSolicitudMedioDto.Archivo?.FileName ?? string.Empty)}_{Guid.NewGuid()}{manageSolicitudMedioDto.Archivo?.Extension ?? string.Empty}",
                Tipo = manageSolicitudMedioDto.Archivo?.ContentType ?? string.Empty,
                Extension = manageSolicitudMedioDto.Archivo?.Extension ?? string.Empty,
                PesoEnBytes = manageSolicitudMedioDto.Archivo?.Length ?? 0,
            };

            fileEntity.RutaDeAlmacenamiento = await _fileService.SaveFileAsync(manageSolicitudMedioDto.Archivo?.Content ?? new byte[0], fileEntity.NombreUnico, ARCHIVOS_PATH);
            fileEntity.FechaCreacion = DateTime.Now;
            return fileEntity;
        }

        return archivoExistente;
    }

    private async Task ValidarFlujoPasos(ManageMovilizacionMediosCommand request)
    {
        // Obtener todo el flujo de la base de datos
        var flujoPasos = await _unitOfWork.Repository<FlujoPasoMovilizacion>().GetAllNoTrackingAsync();

        // Obtener el primer paso configurado (IdPasoActual = NULL)
        var primerPasoPermitido = flujoPasos.FirstOrDefault(f => f.IdPasoActual == null);
        if (primerPasoPermitido == null)
        {
            throw new BadRequestException("No se ha configurado un paso inicial en la base de datos.");
        }

        foreach (var movilizacion in request.Movilizaciones)
        {
            if (!movilizacion.Pasos.Any())
            {
                throw new BadRequestException($"La movilización con ID {movilizacion.Id} no contiene pasos.");
            }

            // Validar que el primer paso coincida con el configurado en la base de datos
            var primerPasoMovilizacion = movilizacion.Pasos.First();
            if ((int)primerPasoMovilizacion.TipoPaso != primerPasoPermitido.IdPasoSiguiente)
            {
                throw new BadRequestException(
                    $"El primer paso de la movilización con ID {movilizacion.Id} debe ser el paso {primerPasoPermitido.IdPasoSiguiente}."
                );
            }

            // Validar el flujo completo de pasos
            for (int i = 0; i < movilizacion.Pasos.Count - 1; i++)
            {
                var pasoActual = movilizacion.Pasos[i];
                var pasoSiguiente = movilizacion.Pasos[i + 1];

                var esPermitido = flujoPasos.Any(f =>
                    f.IdPasoActual == (int)pasoActual.TipoPaso &&
                    f.IdPasoSiguiente == (int)pasoSiguiente.TipoPaso);

                if (!esPermitido)
                {
                    throw new BadRequestException(
                        $"El paso {pasoActual.TipoPaso} no permite continuar al paso {pasoSiguiente.TipoPaso} en la movilización con ID {movilizacion.Id}."
                    );
                }
            }
        }

    }

    private async Task<ActuacionRelevanteDGPCE> GetOrCreateActuacionRelevanteAsync(ManageMovilizacionMediosCommand request, RegistroActualizacion registroActualizacion)
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

        // Validar si ya existe un registro de Movilizacion de Medios para este suceso
        var specSuceso = new ActuacionRelevanteDGPCEWithMovilizacionMedios(new ActuacionRelevanteDGPCESpecificationParams { IdSuceso = request.IdSuceso });
        var movilizacionMedioExistente = await _unitOfWork.Repository<ActuacionRelevanteDGPCE>().GetByIdWithSpec(specSuceso);

        return movilizacionMedioExistente ?? new ActuacionRelevanteDGPCE { IdSuceso = request.IdSuceso };
    }

    private async Task ValidateIdsAsync(ManageMovilizacionMediosCommand request)
    {
        await ValidateIdProcedenciaMedioAsync(request);
        await ValidateIdDestinoMedioAsync(request);
        await ValidateIdTipoAdministracionMedioAsync(request);
        await ValidateIdCapacidadMedioAsync(request);
    }

    private async Task ValidateIdProcedenciaMedioAsync(ManageMovilizacionMediosCommand request)
    {
        // Extraer todos los IdProcedenciaMedio desde los pasos que son del tipo SolicitudMedioRequest
        var idsProcedenciaMedio = request.Movilizaciones
            .SelectMany(m => m.Pasos)
            .OfType<ManageSolicitudMedioDto>() // Filtra solo los pasos del tipo SolicitudMedioRequest
            .Select(p => p.IdProcedenciaMedio)
            .Distinct();

        // Obtener los Ids válidos desde la base de datos
        var procedenciaMedioExistentes = await _unitOfWork.Repository<ProcedenciaMedio>()
            .GetAsync(p => idsProcedenciaMedio.Contains(p.Id));

        if (procedenciaMedioExistentes.Count() != idsProcedenciaMedio.Count())
        {
            // Identificar los Ids inválidos
            var idsProcedenciaMedioExistentes = procedenciaMedioExistentes.Select(p => p.Id).ToList();
            var idsProcedenciaMedioInvalidos = idsProcedenciaMedio.Except(idsProcedenciaMedioExistentes).ToList();

            if (idsProcedenciaMedioInvalidos.Any())
            {
                _logger.LogWarning($"Los siguientes Id's de Procedencia Medio no se encontraron: {string.Join(", ", idsProcedenciaMedioInvalidos)}");
                throw new NotFoundException(nameof(ProcedenciaMedio), string.Join(", ", idsProcedenciaMedioInvalidos));
            }
        }
    }

    private async Task ValidateIdDestinoMedioAsync(ManageMovilizacionMediosCommand request)
    {
        // Extraer todos los idsDestinoMedio desde los pasos que son del tipo ManageTramitacionMedioDto
        var idsDestinoMedio = request.Movilizaciones
            .SelectMany(m => m.Pasos)
            .OfType<ManageTramitacionMedioDto>()
            .Select(p => p.IdDestinoMedio)
            .Distinct();

        // Obtener los Ids válidos desde la base de datos
        var destinoMedioExistentes = await _unitOfWork.Repository<DestinoMedio>()
            .GetAsync(p => idsDestinoMedio.Contains(p.Id));

        if (destinoMedioExistentes.Count() != idsDestinoMedio.Count())
        {
            // Identificar los Ids inválidos
            var idsDestinoMedioExistentes = destinoMedioExistentes.Select(p => p.Id).ToList();
            var idsDestinoMedioInvalidos = idsDestinoMedio.Except(idsDestinoMedioExistentes).ToList();

            if (idsDestinoMedioInvalidos.Any())
            {
                _logger.LogWarning($"Los siguientes Id's de Procedencia Medio no se encontraron: {string.Join(", ", idsDestinoMedioInvalidos)}");
                throw new NotFoundException(nameof(ProcedenciaMedio), string.Join(", ", idsDestinoMedioInvalidos));
            }
        }
    }

    private async Task ValidateIdTipoAdministracionMedioAsync(ManageMovilizacionMediosCommand request)
    {
        var idsTipoAdministracion = request.Movilizaciones
            .SelectMany(m => m.Pasos)
            .OfType<ManageAportacionMedioDto>()
            .Select(p => p.IdTipoAdministracion)
            .Distinct();

        // Obtener los Ids válidos desde la base de datos
        var tipoAdministracionExistentes = await _unitOfWork.Repository<TipoAdministracion>()
            .GetAsync(p => idsTipoAdministracion.Contains(p.Id));

        if (tipoAdministracionExistentes.Count() != idsTipoAdministracion.Count())
        {
            // Identificar los Ids inválidos
            var idsTipoAdministracionExistentes = tipoAdministracionExistentes.Select(p => p.Id).ToList();
            var idsTipoAdministracionInvalidos = idsTipoAdministracion.Except(idsTipoAdministracionExistentes).ToList();

            if (idsTipoAdministracionInvalidos.Any())
            {
                _logger.LogWarning($"Los siguientes Id's de Procedencia Medio no se encontraron: {string.Join(", ", idsTipoAdministracionInvalidos)}");
                throw new NotFoundException(nameof(TipoAdministracion), string.Join(", ", idsTipoAdministracionInvalidos));
            }
        }
    }

    private async Task ValidateIdCapacidadMedioAsync(ManageMovilizacionMediosCommand request)
    {
        var idsCapacidad = request.Movilizaciones
            .SelectMany(m => m.Pasos)
            .Where(p => p is ManageAportacionMedioDto ||
                        p is ManageDespliegueMedioDto ||
                        p is ManageFinIntervencionMedioDto ||
                        p is ManageLlegadaBaseMedioDto)
            .Select(p => p switch
            {
                ManageAportacionMedioDto aportacion => (int?)aportacion.IdCapacidad,
                ManageDespliegueMedioDto despliegue => (int?)despliegue.IdCapacidad,
                ManageFinIntervencionMedioDto finIntervencion => (int?)finIntervencion.IdCapacidad,
                ManageLlegadaBaseMedioDto llegadaBase => (int?)llegadaBase.IdCapacidad,
                _ => null // Esto asegura que todos los casos devuelvan un int?
            })
            .Where(id => id.HasValue) // Filtra los valores nulos
            .Select(id => id.Value) // Convierte los Nullable<int> a int
            .Distinct();

        // Obtener los Ids válidos desde la base de datos
        var capacidadExistentes = await _unitOfWork.Repository<Capacidad>()
            .GetAsync(p => idsCapacidad.Contains(p.Id));

        if (capacidadExistentes.Count() != idsCapacidad.Count())
        {
            // Identificar los Ids inválidos
            var idsCapacidadExistentes = capacidadExistentes.Select(p => p.Id).ToList();
            var idsCapacidadInvalidos = idsCapacidad.Except(idsCapacidadExistentes).ToList();

            if (idsCapacidadInvalidos.Any())
            {
                _logger.LogWarning($"Los siguientes Id's de Procedencia Medio no se encontraron: {string.Join(", ", idsCapacidadInvalidos)}");
                throw new NotFoundException(nameof(Capacidad), string.Join(", ", idsCapacidadInvalidos));
            }
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

}
