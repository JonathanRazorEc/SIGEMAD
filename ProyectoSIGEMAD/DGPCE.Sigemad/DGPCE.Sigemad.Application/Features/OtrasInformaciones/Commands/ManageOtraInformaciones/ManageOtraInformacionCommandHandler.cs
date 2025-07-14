using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Contracts.RegistrosActualizacion;
using DGPCE.Sigemad.Application.Dtos.OtraInformaciones;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Commands.Manage;
using DGPCE.Sigemad.Application.Specifications.Incendios;
using DGPCE.Sigemad.Application.Specifications.OtrasInformaciones;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.OtrasInformaciones.Commands.ManageOtraInformaciones;
public class ManageOtraInformacionCommandHandler : IRequestHandler<ManageOtraInformacionCommand, ManageOtraInformacionResponse>
{
    private readonly ILogger<ManageOtraInformacionCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRegistroActualizacionService _registroActualizacionService;

    public ManageOtraInformacionCommandHandler(
        ILogger<ManageOtraInformacionCommandHandler> logger,
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

    public async Task<ManageOtraInformacionResponse> Handle(ManageOtraInformacionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(ManageOtraInformacionCommandHandler)} - BEGIN");

        await _registroActualizacionService.ValidarSuceso(request.IdSuceso);
        await ValidateIdsAsync(request);
        await ValidateFechas(request);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            RegistroActualizacion registroActualizacion = await _registroActualizacionService.GetOrCreateRegistroActualizacion<OtraInformacion>(
                request.IdRegistroActualizacion, request.IdSuceso, TipoRegistroActualizacionEnum.OtraInformacion);

            OtraInformacion otraInformacion = await GetOrCreateOtraInformacion(request, registroActualizacion);

            var detalleOtraInformacionOriginales = otraInformacion.DetallesOtraInformacion.ToDictionary(d => d.Id, d => _mapper.Map<CreateDetalleOtraInformacionDto>(d));
            MapAndSaveDirecciones(request, otraInformacion);

            var direccionesParaEliminar = await DeleteLogicalDirecciones(request, otraInformacion, registroActualizacion.Id);

            await SaveOtraInformacion(otraInformacion);

            await _registroActualizacionService.SaveRegistroActualizacion<
                OtraInformacion, DetalleOtraInformacion, CreateDetalleOtraInformacionDto>(
                registroActualizacion,
                otraInformacion,
                ApartadoRegistroEnum.OtraInformacion,
                direccionesParaEliminar, detalleOtraInformacionOriginales);

            await _unitOfWork.CommitAsync();

            _logger.LogInformation($"{nameof(CreateOrUpdateDireccionCoordinacionCommandHandler)} - END");

            return new ManageOtraInformacionResponse
            {
                IdOtrainformacion = otraInformacion.Id,
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

    private async Task ValidateFechas(ManageOtraInformacionCommand request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var incendioAsociado = await _unitOfWork.Repository<Incendio>()
           .GetByIdWithSpec(new IncendioActiveByIdSpecification(request.IdSuceso))
           ?? throw new BadRequestException($"No se encontró el incendio con ID {request.IdSuceso}.");

        bool fechasValidas = request.Lista.All(detalleDocumentacion => detalleDocumentacion.FechaHora > incendioAsociado.FechaInicio);
        bool fechasValidasActual = request.Lista.All(detalleDocumentacion => detalleDocumentacion.FechaHora <= DateTime.Now);

        if (!fechasValidas)
        {
            throw new BadRequestException("Una o mas fechas es menor a la fecha del incendio asociado.");
        }

        if (!fechasValidasActual)
        {
            throw new BadRequestException("Una o mas fechas es mayor a la fecha del sistema.");
        }

    }

    private async Task SaveOtraInformacion(OtraInformacion otraInformacion)
    {
        if (otraInformacion.Id > 0)
        {
            _unitOfWork.Repository<OtraInformacion>().UpdateEntity(otraInformacion);
        }
        else
        {
            _unitOfWork.Repository<OtraInformacion>().AddEntity(otraInformacion);
        }

        if (await _unitOfWork.Complete() <= 0)
            throw new Exception("No se pudo insertar/actualizar la Otra Informacion");
    }

    private async Task ValidateIdsAsync(ManageOtraInformacionCommand request)
    {
        await ValidateMedio(request);
        await ValidateProcedenciasDestinosAsync(request);
    }

    private void MapAndSaveDirecciones(ManageOtraInformacionCommand request, OtraInformacion direccionCoordinacion)
    {
        foreach (var direccionDto in request.Lista)
        {
            if (direccionDto.Id.HasValue && direccionDto.Id > 0)
            {
                var direccionExistente = direccionCoordinacion.DetallesOtraInformacion.FirstOrDefault(d => d.Id == direccionDto.Id.Value);
                if (direccionExistente != null)
                {
                    var copiaOriginal = _mapper.Map<CreateDetalleOtraInformacionDto>(direccionExistente);
                    var copiaNueva = _mapper.Map<CreateDetalleOtraInformacionDto>(direccionDto);

                    if (!copiaOriginal.Equals(copiaNueva))
                    {
                        _mapper.Map(direccionDto, direccionExistente);
                        direccionExistente.Borrado = false;
                    }
                }
                else
                {
                    direccionCoordinacion.DetallesOtraInformacion.Add(_mapper.Map<DetalleOtraInformacion>(direccionDto));
                }
            }
            else
            {
                direccionCoordinacion.DetallesOtraInformacion.Add(_mapper.Map<DetalleOtraInformacion>(direccionDto));
            }
        }
    }



    private async Task<List<int>> DeleteLogicalDirecciones(ManageOtraInformacionCommand request, OtraInformacion otraInformacion, int idRegistroActualizacion)
    {
        if (otraInformacion.Id > 0)
        {
            var idsEnRequest = request.Lista.Where(d => d.Id.HasValue && d.Id > 0).Select(d => d.Id).ToList();
            var otrasDireccionesParaEliminar = otraInformacion.DetallesOtraInformacion
                .Where(d => d.Id > 0 && !idsEnRequest.Contains(d.Id))
                .ToList();

            if (otrasDireccionesParaEliminar.Count == 0)
            {
                return new List<int>();
            }

            // Obtener el historial de creación de estas direcciones
            var idsOtrasInformacionesParaEliminar = otrasDireccionesParaEliminar.Select(d => d.Id).ToList();
            var historialOtrasInformaciones = await _unitOfWork.Repository<DetalleRegistroActualizacion>()
                .GetAsync(d =>
                idsOtrasInformacionesParaEliminar.Contains(d.IdReferencia) && d.IdApartadoRegistro == (int)ApartadoRegistroEnum.OtraInformacion);

            foreach (var detalleOtraInformacion in otrasDireccionesParaEliminar)
            {
                var historial = historialOtrasInformaciones.FirstOrDefault(d =>
                d.IdReferencia == detalleOtraInformacion.Id &&
                (d.IdEstadoRegistro == EstadoRegistroEnum.Creado || d.IdEstadoRegistro == EstadoRegistroEnum.CreadoYModificado));

                if (historial == null || historial.IdRegistroActualizacion != idRegistroActualizacion)
                {
                    throw new BadRequestException($"El detalleOtraInformacion con ID {detalleOtraInformacion.Id} solo puede eliminarse en el registro en que fue creada.");
                }

                _unitOfWork.Repository<DetalleOtraInformacion>().DeleteEntity(detalleOtraInformacion);
            }

            return idsOtrasInformacionesParaEliminar;
        }

        return new List<int>();
    }


    private async Task ValidateMedio(ManageOtraInformacionCommand request)
    {
        var idsMedio = request.Lista.Select(d => d.IdMedio).Distinct();
        var tiposMedioExistentes = await _unitOfWork.Repository<Medio>().GetAsync(td => idsMedio.Contains(td.Id));

        if (tiposMedioExistentes.Count() != idsMedio.Count())
        {
            var idsInvalidos = idsMedio.Except(tiposMedioExistentes.Select(td => td.Id)).ToList();
            throw new NotFoundException(nameof(Medio), string.Join(", ", idsInvalidos));
        }
    }

    private async Task ValidateProcedenciasDestinosAsync(ManageOtraInformacionCommand request)
    {
        if (request.Lista != null && request.Lista.Count > 0)
        {
            var idsOtraInformacionProcedenciaDestinos = request.Lista
                .SelectMany(d => d.IdsProcedenciasDestinos ?? new List<int>())
                .Distinct();
            var otraInformacionProcedenciaDestinosExistentes = await _unitOfWork.Repository<ProcedenciaDestino>().GetAsync(ic => idsOtraInformacionProcedenciaDestinos.Contains(ic.Id));

            if (otraInformacionProcedenciaDestinosExistentes.Count() != idsOtraInformacionProcedenciaDestinos.Count())
            {
                var idsOtraInformacionProcedenciaDestinosExistentes = otraInformacionProcedenciaDestinosExistentes.Select(ic => ic.Id).ToList();
                var idsOtraInformacionProcedenciaDestinosInvalidos = idsOtraInformacionProcedenciaDestinos.Except(idsOtraInformacionProcedenciaDestinosExistentes).ToList();

                if (idsOtraInformacionProcedenciaDestinosInvalidos.Any())
                {
                    _logger.LogWarning($"Los siguientes Id's de procedencia destino: {string.Join(", ", idsOtraInformacionProcedenciaDestinosInvalidos)}, no se encontraron");
                    throw new NotFoundException(nameof(ProcedenciaDestino), string.Join(", ", idsOtraInformacionProcedenciaDestinosInvalidos));
                }
            }
        }
    }

    private async Task<OtraInformacion> GetOrCreateOtraInformacion(ManageOtraInformacionCommand request, RegistroActualizacion registroActualizacion)
    {
        if (registroActualizacion.IdReferencia > 0)
        {
            List<int> idsReferencias = new List<int>();
            List<int> idsOtrasInformaciones = new List<int>();

            // Separar IdReferencia según su tipo
            foreach (var detalle in registroActualizacion.DetallesRegistro)
            {
                switch (detalle.IdApartadoRegistro)
                {
                    case (int)ApartadoRegistroEnum.OtraInformacion:
                        idsOtrasInformaciones.Add(detalle.IdReferencia);
                        break;
                    default:
                        idsReferencias.Add(detalle.IdReferencia);
                        break;
                }
            }

            // Buscar la Dirección y Coordinación de Emergencia por IdReferencia
            var otraInformacion = await _unitOfWork.Repository<OtraInformacion>()
                .GetByIdWithSpec(new OtraInformacionWithDetailsAndProcedenciasSpecification(registroActualizacion.IdReferencia, idsOtrasInformaciones));

            if (otraInformacion is null || otraInformacion.Borrado)
                throw new BadRequestException($"El registro de actualización con Id [{registroActualizacion.Id}] no tiene registro de Otra Informacion");

            return otraInformacion;
        }

        // Validar si ya existe un registro de Dirección y Coordinación de Emergencia para este suceso
        var specSuceso = new OtraInformacionWithDetalle(new OtraInformacionParams { IdSuceso = request.IdSuceso });
        var otraInformacionExistente = await _unitOfWork.Repository<OtraInformacion>().GetByIdWithSpec(specSuceso);

        return otraInformacionExistente ?? new OtraInformacion { IdSuceso = request.IdSuceso };
    }


}
