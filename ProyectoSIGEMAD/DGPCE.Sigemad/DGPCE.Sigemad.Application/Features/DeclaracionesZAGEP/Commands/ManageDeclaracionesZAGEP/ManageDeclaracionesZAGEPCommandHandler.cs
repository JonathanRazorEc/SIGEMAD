using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Contracts.RegistrosActualizacion;
using DGPCE.Sigemad.Application.Dtos.DeclaracionesZAGEP;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.ActuacionesRelevantesDGPCE;
using DGPCE.Sigemad.Application.Specifications.Incendios;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DGPCE.Sigemad.Application.Features.DeclaracionesZAGEP.Commands.ManageDeclaracionesZAGEP;
public class ManageDeclaracionesZAGEPCommandHandler : IRequestHandler<ManageDeclaracionesZAGEPCommand, ManageDeclaracionZAGEPResponse>
{
    private readonly ILogger<ManageDeclaracionesZAGEPCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRegistroActualizacionService _registroActualizacionService;

    public ManageDeclaracionesZAGEPCommandHandler(
        ILogger<ManageDeclaracionesZAGEPCommandHandler> logger,
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

    public async Task<ManageDeclaracionZAGEPResponse> Handle(ManageDeclaracionesZAGEPCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(ManageDeclaracionesZAGEPCommandHandler)} - BEGIN");

        await _registroActualizacionService.ValidarSuceso(request.IdSuceso);
        await ValidateFechas(request);
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            RegistroActualizacion registroActualizacion = await _registroActualizacionService.GetOrCreateRegistroActualizacion<ActuacionRelevanteDGPCE>(
                request.IdRegistroActualizacion, request.IdSuceso, TipoRegistroActualizacionEnum.ActuacionRelevante);

            ActuacionRelevanteDGPCE actuacion = await GetOrCreateActuacionRelevante(request, registroActualizacion);

            var declaracionesOriginales = actuacion.DeclaracionesZAGEP.ToDictionary(d => d.Id, d => _mapper.Map<ManageDeclaracionZAGEPDto>(d));
            MapAndSaveDeclaraciones(request, actuacion);

            var declaracionesParaEliminar = await DeleteLogicalDeclaraciones(request, actuacion, registroActualizacion.Id);

            await SaveActuacion(actuacion);

            await _registroActualizacionService.SaveRegistroActualizacion<
                ActuacionRelevanteDGPCE, DeclaracionZAGEP, ManageDeclaracionZAGEPDto>(
                registroActualizacion,
                actuacion,
                ApartadoRegistroEnum.DeclaracionZAGEP,
                declaracionesParaEliminar, declaracionesOriginales);

            await _unitOfWork.CommitAsync();

            _logger.LogInformation($"{nameof(ManageDeclaracionesZAGEPCommandHandler)} - END");

            return new ManageDeclaracionZAGEPResponse
            {
                IdActuacionRelevante = actuacion.Id,
                IdRegistroActualizacion = registroActualizacion.Id
            };

        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error en la transacción de ManageDeclaracionZAGEPResponse");
            throw;
        }
    }

    private async Task ValidateFechas(ManageDeclaracionesZAGEPCommand request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (request.Detalles == null)
            throw new BadRequestException("Los detalles de la declaracion ZAGEP no pueden estar vacios.");

        var incendioAsociado = await _unitOfWork.Repository<Incendio>()
           .GetByIdWithSpec(new IncendioActiveByIdSpecification(request.IdSuceso))
           ?? throw new BadRequestException($"No se encontró el incendio con ID {request.IdSuceso}.");

        bool fechasValidas = request.Detalles.All(declaracionZAGEP => declaracionZAGEP.FechaSolicitud > DateOnly.FromDateTime(incendioAsociado.FechaInicio.UtcDateTime));
        bool fechasValidasActual = request.Detalles.All(declaracionZAGEP => declaracionZAGEP.FechaSolicitud <= DateOnly.FromDateTime(DateTime.Now));

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

    private async Task<List<int>> DeleteLogicalDeclaraciones(ManageDeclaracionesZAGEPCommand request, ActuacionRelevanteDGPCE actuacion, int idRegistroActualizacion)
    {
        if (actuacion.Id > 0)
        {
            var idsEnRequest = request.Detalles.Where(d => d.Id.HasValue && d.Id > 0).Select(d => d.Id).ToList();
            var declaracionesZagepParaEliminar = actuacion.DeclaracionesZAGEP
                .Where(d => d.Id > 0 && !idsEnRequest.Contains(d.Id))
                .ToList();

            if (declaracionesZagepParaEliminar.Count == 0)
            {
                return new List<int>();
            }

            // Obtener el historial de creación de estas declaraciones
            var idsDeclaracionesZagepParaEliminar = declaracionesZagepParaEliminar.Select(d => d.Id).ToList();
            var historialDirecciones = await _unitOfWork.Repository<DetalleRegistroActualizacion>()
                .GetAsync(d =>
                idsDeclaracionesZagepParaEliminar.Contains(d.IdReferencia) && d.IdApartadoRegistro == (int)ApartadoRegistroEnum.DeclaracionZAGEP);

            foreach (var declaracion in declaracionesZagepParaEliminar)
            {
                var historial = historialDirecciones.FirstOrDefault(d =>
                d.IdReferencia == declaracion.Id &&
                (d.IdEstadoRegistro == EstadoRegistroEnum.Creado || d.IdEstadoRegistro == EstadoRegistroEnum.CreadoYModificado));

                if (historial == null || historial.IdRegistroActualizacion != idRegistroActualizacion)
                {
                    throw new BadRequestException($"La declaracionZAGEP con ID {declaracion.Id} solo puede eliminarse en el registro en que fue creada.");
                }

                _unitOfWork.Repository<DeclaracionZAGEP>().DeleteEntity(declaracion);
            }

            return idsDeclaracionesZagepParaEliminar;
        }

        return new List<int>();
    }


    private void MapAndSaveDeclaraciones(ManageDeclaracionesZAGEPCommand request, ActuacionRelevanteDGPCE actuacion)
    {
        foreach (var declaracionDto in request.Detalles)
        {
            if (declaracionDto.Id.HasValue && declaracionDto.Id > 0)
            {
                var declaracionExistente = actuacion.DeclaracionesZAGEP.FirstOrDefault(d => d.Id == declaracionDto.Id.Value);
                if (declaracionExistente != null)
                {
                    var copiaOriginal = _mapper.Map<ManageDeclaracionZAGEPDto>(declaracionExistente);
                    var copiaNueva = _mapper.Map<ManageDeclaracionZAGEPDto>(declaracionDto);

                    if (!copiaOriginal.Equals(copiaNueva))
                    {
                        _mapper.Map(declaracionDto, declaracionExistente);
                        declaracionExistente.Borrado = false;
                    }
                }
                else
                {
                    actuacion.DeclaracionesZAGEP.Add(_mapper.Map<DeclaracionZAGEP>(declaracionDto));
                }
            }
            else
            {
                actuacion.DeclaracionesZAGEP.Add(_mapper.Map<DeclaracionZAGEP>(declaracionDto));
            }
        }
    }


    private async Task<ActuacionRelevanteDGPCE> GetOrCreateActuacionRelevante(ManageDeclaracionesZAGEPCommand request, RegistroActualizacion registroActualizacion)
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

            // Buscar la Dirección y Coordinación de Emergencia por IdReferencia
            var actuacionRelevante = await _unitOfWork.Repository<ActuacionRelevanteDGPCE>()
                .GetByIdWithSpec(new ActuacionRelevanteDGPCEWithFilteredData(registroActualizacion.IdReferencia, idsActivacionPlanEmergencias, idsDeclaracionesZAGEP, idsActivacionSistemas, idsConvocatoriasCECOD, idsNotificacionesEmergencias, idsMovilizacionMedios, includeEmergenciaNacional));

            if (actuacionRelevante is null || actuacionRelevante.Borrado)
                throw new BadRequestException($"El registro de actualización con Id [{registroActualizacion.Id}] no tiene registro de actuacionRelevanteDGPCE");

            return actuacionRelevante;
        }

        // Validar si ya existe un registro de Dirección y Coordinación de Emergencia para este suceso
        var specSuceso = new ActuacionRelevanteDGPCEWithDeclaracionesZAGEP(new ActuacionRelevanteDGPCESpecificationParams { IdSuceso = request.IdSuceso });
        var declaracionZAGEPExistente = await _unitOfWork.Repository<ActuacionRelevanteDGPCE>().GetByIdWithSpec(specSuceso);

        return declaracionZAGEPExistente ?? new ActuacionRelevanteDGPCE { IdSuceso = request.IdSuceso };
    }
}
