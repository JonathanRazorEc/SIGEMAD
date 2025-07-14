using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Files;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Contracts.RegistrosActualizacion;
using DGPCE.Sigemad.Application.Dtos.DetallesDocumentaciones;
using DGPCE.Sigemad.Application.Dtos.Documentaciones;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Documentos;
using DGPCE.Sigemad.Application.Specifications.Incendios;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Documentaciones.Commands.ManageDocumentaciones;
public class ManageDocumentacionesCommandHandler : IRequestHandler<ManageDocumentacionesCommand, CreateOrUpdateDocumentacionResponse>
{
    private readonly ILogger<ManageDocumentacionesCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;
    private const string ARCHIVOS_PATH = "documentacion";
    private readonly IRegistroActualizacionService _registroActualizacionService;

    public ManageDocumentacionesCommandHandler(
        ILogger<ManageDocumentacionesCommandHandler> logger,
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

    public async Task<CreateOrUpdateDocumentacionResponse> Handle(ManageDocumentacionesCommand request, CancellationToken cancellationToken)
    {

        _logger.LogInformation($"{nameof(ManageDocumentacionesCommandHandler)} - BEGIN");

        await _registroActualizacionService.ValidarSuceso(request.IdSuceso);
        await ValidateIdsAsync(request);
        await ValidateFechas(request);


        await _unitOfWork.BeginTransactionAsync();

        try
        {
            RegistroActualizacion registroActualizacion = await _registroActualizacionService.GetOrCreateRegistroActualizacion<Documentacion>(
                request.IdRegistroActualizacion, request.IdSuceso, TipoRegistroActualizacionEnum.Documentacion);

            Documentacion documentacion = await GetOrCreateDocumentacionAsync(request, registroActualizacion);

            var detallesDocumentacionOriginales = documentacion.DetallesDocumentacion.ToDictionary(d => d.Id, d => _mapper.Map<DetalleDocumentacionDto>(d));
            MapAndSaveDetallesDocumentacion(request, documentacion);

            var detallesDocumentacionParaEliminar = await DeleteLogicalDirecciones(request, documentacion, registroActualizacion.Id);

            await SaveDireccionCoordinacion(documentacion);

            await _registroActualizacionService.SaveRegistroActualizacion<
                Documentacion, DetalleDocumentacion, DetalleDocumentacionDto>(
                registroActualizacion,
                documentacion,
                ApartadoRegistroEnum.Documentacion,
                detallesDocumentacionParaEliminar, detallesDocumentacionOriginales);

            await _unitOfWork.CommitAsync();

            _logger.LogInformation($"{nameof(ManageDocumentacionesCommandHandler)} - END");

            return new CreateOrUpdateDocumentacionResponse
            {
                IdDocumentacion = documentacion.Id,
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

    private async Task ValidateFechas(ManageDocumentacionesCommand request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if(request.DetallesDocumentaciones == null)
            throw new BadRequestException($"Los detalles de la documentacion son obligatorios.");

        var incendioAsociado = await _unitOfWork.Repository<Incendio>()
           .GetByIdWithSpec(new IncendioActiveByIdSpecification(request.IdSuceso))
           ?? throw new BadRequestException($"No se encontró el incendio con ID {request.IdSuceso}.");

        bool fechasValidas = request.DetallesDocumentaciones.All(documentacion => documentacion.FechaHora > incendioAsociado.FechaInicio);
        bool fechasValidasActual = request.DetallesDocumentaciones.All(documentacion => documentacion.FechaHora <= DateTime.Now);

        if (!fechasValidas)
        {
            throw new BadRequestException("Una o mas fechas es menor a la fecha del incendio asociado.");
        }

        if (!fechasValidasActual)
        {
            throw new BadRequestException("Una o mas fechas es mayor a la fecha del sistema.");
        }

    }

    private async Task SaveDireccionCoordinacion(Documentacion documentacion)
    {
        if (documentacion.Id > 0)
        {
            _unitOfWork.Repository<Documentacion>().UpdateEntity(documentacion);
        }
        else
        {
            _unitOfWork.Repository<Documentacion>().AddEntity(documentacion);
        }

        if (await _unitOfWork.Complete() <= 0)
            throw new Exception("No se pudo insertar/actualizar la documentacion");
    }

    private async Task<List<int>> DeleteLogicalDirecciones(ManageDocumentacionesCommand request, Documentacion documentacion, int idRegistroActualizacion)
    {
        if (documentacion.Id > 0)
        {
            var idsEnRequest = request.DetallesDocumentaciones.Where(d => d.Id.HasValue && d.Id > 0).Select(d => d.Id).ToList();
            var detallesDocumentacionParaEliminar = documentacion.DetallesDocumentacion
                .Where(d => d.Id > 0 && !idsEnRequest.Contains(d.Id))
                .ToList();

            if (detallesDocumentacionParaEliminar.Count == 0)
            {
                return new List<int>();
            }

            // Obtener el historial de creación de estas direcciones
            var idsDetalleDocumentacionParaEliminar = detallesDocumentacionParaEliminar.Select(d => d.Id).ToList();
            var historialDirecciones = await _unitOfWork.Repository<DetalleRegistroActualizacion>()
                .GetAsync(d =>
                idsDetalleDocumentacionParaEliminar.Contains(d.IdReferencia) && d.IdApartadoRegistro == (int)ApartadoRegistroEnum.Documentacion);

            foreach (var detalle in detallesDocumentacionParaEliminar)
            {
                var historial = historialDirecciones.FirstOrDefault(d =>
                d.IdReferencia == detalle.Id &&
                (d.IdEstadoRegistro == EstadoRegistroEnum.Creado || d.IdEstadoRegistro == EstadoRegistroEnum.CreadoYModificado));

                if (historial == null || historial.IdRegistroActualizacion != idRegistroActualizacion)
                {
                    throw new BadRequestException($"El detalle de documentacion con ID {detalle.Id} solo puede eliminarse en el registro en que fue creada.");
                }

                if (detalle.IdArchivo.HasValue)
                {
                    _unitOfWork.Repository<Archivo>().DeleteEntity(detalle.Archivo);
                }

                _unitOfWork.Repository<DetalleDocumentacion>().DeleteEntity(detalle);
            }

            return idsDetalleDocumentacionParaEliminar;
        }

        return new List<int>();
    }

    private async void MapAndSaveDetallesDocumentacion(ManageDocumentacionesCommand request, Documentacion documentacion)
    {
        foreach (var detalleDocumentoDto in request.DetallesDocumentaciones)
        {
            if (detalleDocumentoDto.Id.HasValue && detalleDocumentoDto.Id > 0)
            {
                var documentacionExistente = documentacion.DetallesDocumentacion.FirstOrDefault(d => d.Id == detalleDocumentoDto.Id.Value);
                if (documentacionExistente != null)
                {
                    var copiaOriginal = _mapper.Map<DetalleDocumentacionDto>(documentacionExistente);
                    var copiaNueva = _mapper.Map<DetalleDocumentacionDto>(detalleDocumentoDto);

                    if (!copiaOriginal.Equals(copiaNueva))
                    {
                        _mapper.Map(detalleDocumentoDto, documentacionExistente);
                        documentacionExistente.Borrado = false;
                        documentacionExistente.Archivo = await MapArchivo(detalleDocumentoDto, documentacionExistente.Archivo);
                    }
                }
                else
                {
                    var nuevoDetalleDocumentacion = await CreateDetalleDocumentacion(detalleDocumentoDto);
                    documentacion.DetallesDocumentacion.Add(nuevoDetalleDocumentacion);
                }
            }
            else
            {
                var nuevoDetalleDocumentacion = await CreateDetalleDocumentacion(detalleDocumentoDto);
                documentacion.DetallesDocumentacion.Add(nuevoDetalleDocumentacion);
            }
        }
    }

    private async Task<Documentacion> GetOrCreateDocumentacionAsync(ManageDocumentacionesCommand request, RegistroActualizacion registroActualizacion)
    {
        if (registroActualizacion.IdReferencia > 0)
        {
            List<int> idsReferencias = new List<int>();
            List<int> idsDetallesDocumentaciones = new List<int>();

            // Separar IdReferencia según su tipo
            foreach (var detalle in registroActualizacion.DetallesRegistro)
            {
                switch (detalle.IdApartadoRegistro)
                {
                    case (int)ApartadoRegistroEnum.Documentacion:
                        idsDetallesDocumentaciones.Add(detalle.IdReferencia);
                        break;
                    default:
                        idsReferencias.Add(detalle.IdReferencia);
                        break;
                }
            }

            // Buscar la Dirección y Coordinación de Emergencia por IdReferencia
            var documentacion = await _unitOfWork.Repository<Documentacion>()
                .GetByIdWithSpec(new DocumentacionWithFilteredData(registroActualizacion.IdReferencia, idsDetallesDocumentaciones));

            if (documentacion is null || documentacion.Borrado)
                throw new BadRequestException($"El registro de actualización con Id [{registroActualizacion.Id}] no tiene registro de documentación");

            return documentacion;
        }

        // Validar si ya existe un registro de Dirección y Coordinación de Emergencia para este suceso
        var specSuceso = new DocumentacionSpecificationWithParams(new DocumentacionParams { IdSuceso = request.IdSuceso });
        var documentacionExistente = await _unitOfWork.Repository<Documentacion>().GetByIdWithSpec(specSuceso);

        return documentacionExistente ?? new Documentacion { IdSuceso = request.IdSuceso };
    }

    private async Task ValidateIdsAsync(ManageDocumentacionesCommand request)
    {
        await ValidateTipoDocumentosAsync(request);
        await ValidateProcedenciasDestinosAsync(request);
    }

    private async Task ValidateTipoDocumentosAsync(ManageDocumentacionesCommand request)
    {
        var idsTipoDocumento = request.DetallesDocumentaciones.Select(c => c.IdTipoDocumento).Distinct();
        var tipoDocumentoExistentes = await _unitOfWork.Repository<TipoDocumento>().GetAsync(p => idsTipoDocumento.Contains(p.Id));

        if (tipoDocumentoExistentes.Count() != idsTipoDocumento.Count())
        {
            var idsTipoDocumentosExistentes = tipoDocumentoExistentes.Select(p => p.Id).ToList();
            var idsTipoDocumentosInvalidas = idsTipoDocumento.Except(idsTipoDocumentosExistentes).ToList();

            if (idsTipoDocumentosInvalidas.Any())
            {
                _logger.LogWarning($"Los siguientes Id's de Tipo Documento: {string.Join(", ", idsTipoDocumentosInvalidas)}, no se encontraron");
                throw new NotFoundException(nameof(TipoDocumento), string.Join(", ", idsTipoDocumentosInvalidas));
            }
        }
    }

    private async Task ValidateProcedenciasDestinosAsync(ManageDocumentacionesCommand request)
    {
        if (request.DetallesDocumentaciones != null && request.DetallesDocumentaciones.Count >0)
        {
        var idsDocumentacionProcedenciaDestinos = request.DetallesDocumentaciones
            .SelectMany(d => d.IdsProcedenciasDestinos ?? new List<int>())
            .Distinct();
        var documentacionProcedenciaDestinosExistentes = await _unitOfWork.Repository<ProcedenciaDestino>().GetAsync(ic => idsDocumentacionProcedenciaDestinos.Contains(ic.Id));

        if (documentacionProcedenciaDestinosExistentes.Count() != idsDocumentacionProcedenciaDestinos.Count())
        {
            var idsDocumentacionProcedenciaDestinosExistentes = documentacionProcedenciaDestinosExistentes.Select(ic => ic.Id).ToList();
            var idsDocumentacionProcedenciaDestinosInvalidos = idsDocumentacionProcedenciaDestinos.Except(idsDocumentacionProcedenciaDestinosExistentes).ToList();

            if (idsDocumentacionProcedenciaDestinosInvalidos.Any())
            {
                _logger.LogWarning($"Los siguientes Id's de procedencia destino: {string.Join(", ", idsDocumentacionProcedenciaDestinosInvalidos)}, no se encontraron");
                throw new NotFoundException(nameof(ProcedenciaDestino), string.Join(", ", idsDocumentacionProcedenciaDestinosInvalidos));
            }
        }
       }
    }

    private async Task MapAndManageDetallesDocumentacion(ManageDocumentacionesCommand request, Documentacion documentacion)
    {
        foreach (var detalleDocumentoDto in request.DetallesDocumentaciones)
        {
            if (detalleDocumentoDto.Id.HasValue && detalleDocumentoDto.Id > 0)
            {
                var detalleDocumentacion = documentacion.DetallesDocumentacion.FirstOrDefault(c => c.Id == detalleDocumentoDto.Id.Value);

                if (detalleDocumentacion != null)
                {
                    _mapper.Map(detalleDocumentoDto, detalleDocumentacion);
                    detalleDocumentacion.Borrado = false;
                    detalleDocumentacion.Archivo = await MapArchivo(detalleDocumentoDto, detalleDocumentacion.Archivo);
                }
                else
                {
                    var nuevoDetalleDocumentacion = await CreateDetalleDocumentacion(detalleDocumentoDto);
                    documentacion.DetallesDocumentacion.Add(nuevoDetalleDocumentacion);
                }
            }
            else
            {
                var nuevoDetalleDocumentacion = await CreateDetalleDocumentacion(detalleDocumentoDto);
                documentacion.DetallesDocumentacion.Add(nuevoDetalleDocumentacion);
            }
        }

        if (request.IdRegistroActualizacion.HasValue)
        {
            var idsEnRequest = request.DetallesDocumentaciones
                .Where(c => c.Id.HasValue && c.Id > 0)
                .Select(c => c.Id)
                .ToList();

            var detallesDocumentacionParaEliminar = documentacion.DetallesDocumentacion
                .Where(c => c.Id > 0 && c.Borrado == false && !idsEnRequest.Contains(c.Id))
                .ToList();

            foreach (var detalleAEliminar in detallesDocumentacionParaEliminar)
            {
                if (detalleAEliminar.IdArchivo.HasValue)
                {
                    _unitOfWork.Repository<Archivo>().DeleteEntity(detalleAEliminar.Archivo);
                }

                _unitOfWork.Repository<DetalleDocumentacion>().DeleteEntity(detalleAEliminar);
            }
        }
    }

    private async Task<Archivo?> MapArchivo(DetalleDocumentacionDto detalleDocumentoDto, Archivo? archivoExistente)
    {
        if (detalleDocumentoDto.Archivo != null)
        {
            var fileEntity = new Archivo
            {
                NombreOriginal = detalleDocumentoDto.Archivo?.FileName ?? string.Empty,
                NombreUnico = $"{Path.GetFileNameWithoutExtension(detalleDocumentoDto.Archivo?.FileName ?? string.Empty)}_{Guid.NewGuid()}{detalleDocumentoDto.Archivo?.Extension ?? string.Empty}",
                Tipo = detalleDocumentoDto.Archivo?.ContentType ?? string.Empty,
                Extension = detalleDocumentoDto.Archivo?.Extension ?? string.Empty,
                PesoEnBytes = detalleDocumentoDto.Archivo?.Length ?? 0,
            };

            fileEntity.RutaDeAlmacenamiento = await _fileService.SaveFileAsync(detalleDocumentoDto.Archivo?.Content ?? new byte[0], fileEntity.NombreUnico, ARCHIVOS_PATH);
            fileEntity.FechaCreacion = DateTime.Now;
            return fileEntity;
        }

        return archivoExistente;
    }

    private async Task<DetalleDocumentacion> CreateDetalleDocumentacion(DetalleDocumentacionDto detalleDocumentoDto)
    {
        var nuevoDetalleDocumentacion = new DetalleDocumentacion
        {
            Descripcion = detalleDocumentoDto.Descripcion,
            DocumentacionProcedenciaDestinos = detalleDocumentoDto.IdsProcedenciasDestinos?
                .Select(id => new DocumentacionProcedenciaDestino
                {
                    IdProcedenciaDestino = id
                }).ToList() ?? new List<DocumentacionProcedenciaDestino>(),
            FechaHora = detalleDocumentoDto.FechaHora,
            FechaHoraSolicitud = detalleDocumentoDto.FechaHoraSolicitud,
            IdTipoDocumento = detalleDocumentoDto.IdTipoDocumento,
        };

        nuevoDetalleDocumentacion.Archivo = await MapArchivo(detalleDocumentoDto, null);

        return nuevoDetalleDocumentacion;
    }




}
