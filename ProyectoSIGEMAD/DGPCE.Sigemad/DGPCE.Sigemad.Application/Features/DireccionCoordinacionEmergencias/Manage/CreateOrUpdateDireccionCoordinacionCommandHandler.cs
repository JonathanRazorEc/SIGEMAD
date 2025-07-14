using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Files;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Contracts.RegistrosActualizacion;
using DGPCE.Sigemad.Application.Dtos.CoordinacionCecopis;
using DGPCE.Sigemad.Application.Dtos.CoordinacionesPMA;
using DGPCE.Sigemad.Application.Dtos.Direcciones;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Incendios;
using DGPCE.Sigemad.Application.Specifications.Registros;
using DGPCE.Sigemad.Domain.Constracts;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Commands.Manage;
internal class CreateOrUpdateDireccionCoordinacionCommandHandler : IRequestHandler<CreateOrUpdateDireccionCoordinacionCommand, CreateOrUpdateDireccionResponse>
{
    private readonly ILogger<CreateOrUpdateDireccionCoordinacionCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRegistroActualizacionService _registroActualizacionService;
    private readonly IFileService _fileService;
    private readonly IGeometryValidator _geometryValidator;
    private const string ARCHIVOS_PATH = "documentacion";

    public CreateOrUpdateDireccionCoordinacionCommandHandler(
        ILogger<CreateOrUpdateDireccionCoordinacionCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IGeometryValidator geometryValidator,
        IRegistroActualizacionService registroActualizacionService,
        IFileService fileService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _registroActualizacionService = registroActualizacionService;
        _fileService = fileService;
        _geometryValidator = geometryValidator;
    }

    public async Task<CreateOrUpdateDireccionResponse> Handle(CreateOrUpdateDireccionCoordinacionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(CreateOrUpdateDireccionCoordinacionCommandHandler)} - BEGIN");

        await _registroActualizacionService.ValidarSuceso(request.IdSuceso);
        await ValidateTipoDireccionEmergencia(request);

        ValidarDuplicados(request.Direcciones, x => x.FechaInicio, "Direccion");
        ValidarDuplicados(request.CoordinacionesPMA, x => x.FechaInicio, "Coordinaciones PMA");
        ValidarDuplicados(request.CoordinacionesCECOPI, x => x.FechaInicio, "Coordinaciones CECOPI");

        await ValidateProvincia(request);
        await ValidateMunicipio(request);
        var incendio = await ValidateIncendio(request.IdSuceso);

        // 🔒 Validación adicional de geometría
        foreach (var cecopi in request.CoordinacionesCECOPI ?? new List<CreateOrUpdateCoordinacionCecopiDto>())
        {
            if (cecopi.GeoPosicion != null)
            {
                if (!_geometryValidator.IsGeometryValidAndInEPSG4326(cecopi.GeoPosicion))
                {
                    _logger.LogWarning($"Coordinacion CECOPI con Id {cecopi.Id}: GeoPosicion inválida o mal formada.");
                    throw new ValidationException(new List<ValidationFailure>
                    {
                        new ValidationFailure("GeoPosicion", $"La geometría enviada para Coordinación CECOPI con Id {cecopi.Id} no es válida.")
                    });
                }
            }
        }

        foreach (var pma in request.CoordinacionesPMA ?? new List<CreateOrUpdateCoordinacionPmaDto>())
        {
            if (pma.GeoPosicion != null)
            {
                if (!_geometryValidator.IsGeometryValidAndInEPSG4326(pma.GeoPosicion))
                {
                    _logger.LogWarning($"Coordinacion PMA con Id {pma.Id}: GeoPosicion inválida o mal formada.");
                    throw new ValidationException(new List<ValidationFailure>
                    {
                        new ValidationFailure("GeoPosicion", $"La geometría enviada para Coordinación PMA con Id {pma.Id} no es válida.")
                    });
                }
            }
        }


        await _unitOfWork.BeginTransactionAsync();

        try
        {
            RegistroActualizacion registroActualizacion = await _registroActualizacionService.GetOrCreateRegistroActualizacion<Registro>(
                    request.IdRegistroActualizacion, request.IdSuceso, TipoRegistroActualizacionEnum.Registro);

            var registro = await GetRegistro(request, registroActualizacion);

            // Direcciones
            ValidateFechasGenerico(request.Direcciones, request.IdSuceso, incendio, "FechaInicio");
            var direccionesOriginales = registro.Direcciones.ToDictionary(d => d.Id, d => _mapper.Map<CreateOrUpdateDireccionDto>(d));
            var direccionesParaEliminar = await DeleteLogicalDirecciones(request, registro, registroActualizacion.Id);
            await MapAndSaveDirecciones(request, registro);

            // Coordinaciones PMA
            ValidateFechasGenerico(request.CoordinacionesPMA, request.IdSuceso, incendio, "FechaInicio");
            var coordinacionesPMAOriginales = registro.CoordinacionesPMA.ToDictionary(d => d.Id, d => _mapper.Map<CreateOrUpdateCoordinacionPmaDto>(d));
            var coordinacionesPMAParaEliminar = await DeleteLogicalCoordinacionesPMA(request, registro, registroActualizacion.Id);
            await MapAndSaveCoordinacionesPMA(request, registro);

            // Coordinaciones CECOPI
            ValidateFechasGenerico(request.CoordinacionesCECOPI, request.IdSuceso, incendio, "FechaInicio");
            var coordinacionesCECOPIOriginales = registro.CoordinacionesCecopi.ToDictionary(d => d.Id, d => _mapper.Map<CreateOrUpdateCoordinacionCecopiDto>(d));
            var coordinacionesCECOPIParaEliminar = await DeleteLogicalCoordinacionesCECOPI(request, registro, registroActualizacion.Id);
            await MapAndSaveCoordinacionesCECOPI(request, registro);

            await SaveRegistro(registro);

            await _registroActualizacionService.SaveRegistroActualizacion<Registro, Direccion, CreateOrUpdateDireccionDto>(
              registroActualizacion, registro, ApartadoRegistroEnum.Direccion, direccionesParaEliminar, direccionesOriginales);

            await _registroActualizacionService.SaveRegistroActualizacion<Registro, CoordinacionCecopi, CreateOrUpdateCoordinacionCecopiDto>(
              registroActualizacion, registro, ApartadoRegistroEnum.CoordinacionCECOPI, coordinacionesCECOPIParaEliminar, coordinacionesCECOPIOriginales);

            await _registroActualizacionService.SaveRegistroActualizacion<Registro, CoordinacionPMA, CreateOrUpdateCoordinacionPmaDto>(
              registroActualizacion, registro, ApartadoRegistroEnum.CoordinacionPMA, coordinacionesPMAParaEliminar, coordinacionesPMAOriginales);

            await _unitOfWork.CommitAsync();

            _logger.LogInformation($"{nameof(CreateOrUpdateDireccionCoordinacionCommandHandler)} - END");

            return new CreateOrUpdateDireccionResponse
            {
                IdRegistro = registro.Id,
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


    private void ValidarDuplicados<T>(List<T> lista,Func<T, object> clave,string nombreLista)
    {
        if (lista == null || !lista.Any()) return;

        var duplicados = lista
        .GroupBy(clave)
        .Where(g => g.Count() > 1)
        .ToList();

        if (duplicados.Any())
        {
            throw new BadRequestException($"Existen elementos duplicados en la lista {nombreLista} con la misma fecha Inicio.");
        }
    }

    private async Task<List<int>> DeleteLogicalCoordinacionesCECOPI(CreateOrUpdateDireccionCoordinacionCommand request, Registro registro, int idRegistroActualizacion)
    {
        if (registro.Id > 0)
        {
            var idsEnRequest = request.CoordinacionesCECOPI.Where(d => d.Id.HasValue && d.Id > 0).Select(d => d.Id).ToList();
            var coordinacionesParaEliminar = registro.CoordinacionesCecopi
                .Where(d => d.Id > 0 && !idsEnRequest.Contains(d.Id))
                .ToList();

            if (coordinacionesParaEliminar.Count == 0)
            {
                return new List<int>();
            }

            // Obtener el historial de creación de estas direcciones
            var idsCoordinacionesParaEliminar = coordinacionesParaEliminar.Select(d => d.Id).ToList();
            var historialCoordinaciones = await _unitOfWork.Repository<DetalleRegistroActualizacion>()
                .GetAsync(d =>
                idsCoordinacionesParaEliminar.Contains(d.IdReferencia) && d.IdApartadoRegistro == (int)ApartadoRegistroEnum.CoordinacionCECOPI);

            foreach (var coordinacion in coordinacionesParaEliminar)
            {
                var historial = historialCoordinaciones.FirstOrDefault(d =>
                d.IdReferencia == coordinacion.Id &&
                (d.IdEstadoRegistro == EstadoRegistroEnum.Creado || d.IdEstadoRegistro == EstadoRegistroEnum.CreadoYModificado));

                if (historial == null || historial.IdRegistroActualizacion != idRegistroActualizacion)
                {
                    throw new BadRequestException($"La coordinacion cecopi con ID {coordinacion.Id} solo puede eliminarse en el registro en que fue creada.");
                }

                if (coordinacion.IdArchivo.HasValue)
                {
                    _unitOfWork.Repository<Archivo>().DeleteEntity(coordinacion.Archivo);
                }

                _unitOfWork.Repository<CoordinacionCecopi>().DeleteEntity(coordinacion);
            }

            return idsCoordinacionesParaEliminar;
        }

        return new List<int>();
    }


    private async Task<List<int>> DeleteLogicalCoordinacionesPMA(CreateOrUpdateDireccionCoordinacionCommand request, Registro registro, int idRegistroActualizacion)
    {
        if (registro.Id > 0)
        {
            var idsEnRequest = request.CoordinacionesPMA.Where(d => d.Id.HasValue && d.Id > 0).Select(d => d.Id).ToList();
            var coordinacionesParaEliminar = registro.CoordinacionesPMA
                .Where(d => d.Id > 0 && !idsEnRequest.Contains(d.Id))
                .ToList();

            if (coordinacionesParaEliminar.Count == 0)
            {
                return new List<int>();
            }

            // Obtener el historial de creación de estas direcciones
            var idsCoordinacionesParaEliminar = coordinacionesParaEliminar.Select(d => d.Id).ToList();
            var historialCoordinaciones = await _unitOfWork.Repository<DetalleRegistroActualizacion>()
                .GetAsync(d =>
                idsCoordinacionesParaEliminar.Contains(d.IdReferencia) && d.IdApartadoRegistro == (int)ApartadoRegistroEnum.CoordinacionPMA);

            foreach (var coordinacion in coordinacionesParaEliminar)
            {
                var historial = historialCoordinaciones.FirstOrDefault(d =>
                d.IdReferencia == coordinacion.Id &&
                (d.IdEstadoRegistro == EstadoRegistroEnum.Creado || d.IdEstadoRegistro == EstadoRegistroEnum.CreadoYModificado));

                if (historial == null || historial.IdRegistroActualizacion != idRegistroActualizacion)
                {
                    throw new BadRequestException($"La coordinacion PMA con ID {coordinacion.Id} solo puede eliminarse en el registro en que fue creada.");
                }

                if (coordinacion.IdArchivo.HasValue)
                {
                    _unitOfWork.Repository<Archivo>().DeleteEntity(coordinacion.Archivo);
                }

                _unitOfWork.Repository<CoordinacionPMA>().DeleteEntity(coordinacion);
            }

            return idsCoordinacionesParaEliminar;
        }

        return new List<int>();
    }




    private async Task ValidateProvincia(CreateOrUpdateDireccionCoordinacionCommand request)
    {
        var idsProvincia = request.CoordinacionesPMA
         .Select(d => d.IdProvincia)
         .Concat(request.CoordinacionesCECOPI.Select(d => d.IdProvincia))
         .Distinct();

        var provinciasExistentes = await _unitOfWork.Repository<Provincia>().GetAsync(td => idsProvincia.Contains(td.Id) && td.Borrado == false);

        if (provinciasExistentes.Count() != idsProvincia.Count())
        {
            var idsInvalidos = idsProvincia.Except(provinciasExistentes.Select(td => td.Id)).ToList();
            throw new NotFoundException(nameof(Provincia), string.Join(", ", idsInvalidos));
        }
    }

    private async Task ValidateMunicipio(CreateOrUpdateDireccionCoordinacionCommand request)
    {
        var idsMunicipio = request.CoordinacionesPMA
                 .Select(d => d.IdMunicipio)
                 .Concat(request.CoordinacionesCECOPI.Select(d => d.IdMunicipio))
                 .Distinct();
        var municipiosExistentes = await _unitOfWork.Repository<Municipio>().GetAsync(td => idsMunicipio.Contains(td.Id) && td.Borrado == false);

        if (municipiosExistentes.Count() != idsMunicipio.Count())
        {
            var idsInvalidos = idsMunicipio.Except(municipiosExistentes.Select(td => td.Id)).ToList();
            throw new NotFoundException(nameof(Municipio), string.Join(", ", idsInvalidos));
        }
    }


    private async Task<Incendio> ValidateIncendio(int idSuceso)
    {
        var incendioAsociado = await _unitOfWork.Repository<Incendio>()
             .GetByIdWithSpec(new IncendioActiveByIdSpecification(idSuceso))
             ?? throw new BadRequestException($"No se encontró el incendio con ID {idSuceso}.");

        return incendioAsociado;
    }

    private void ValidateFechasGenerico<T>(List<T> lista, int idSuceso, Incendio incendioAsociado, string nombrePropiedadFechaInicio)
    {
        if (lista.Count == 0) return;

        foreach (var item in lista)
        {
            var fechaInicioProp = typeof(T).GetProperty(nombrePropiedadFechaInicio);
            if (fechaInicioProp == null)
                throw new InvalidOperationException($"La propiedad '{nombrePropiedadFechaInicio}' no existe en el tipo {typeof(T).Name}.");

            var fechaInicio = (DateTime)fechaInicioProp.GetValue(item)!;

            if (fechaInicio <= incendioAsociado.FechaInicio)
                throw new BadRequestException("Una o más fechas de inicio son menores o iguales a la fecha del incendio.");

            if (fechaInicio > DateTime.Now)
                throw new BadRequestException("Una o más fechas de inicio son mayores a la fecha actual del sistema.");
        }
    }



    private async Task<Registro> GetRegistro(CreateOrUpdateDireccionCoordinacionCommand request, RegistroActualizacion registroActualizacion)
    {
        if (registroActualizacion.IdReferencia > 0)
        {
            List<int> idsReferencias = new List<int>();
            List<int> idsDirecciones = new List<int>();
            List<int> idsCecopi = new List<int>();
            List<int> idsPma = new List<int>();


            // Separar IdReferencia según su tipo
            foreach (var detalle in registroActualizacion.DetallesRegistro)
            {
                switch (detalle.IdApartadoRegistro)
                {
                    case (int)ApartadoRegistroEnum.Direccion:
                        idsDirecciones.Add(detalle.IdReferencia);
                        break;
                    case (int)ApartadoRegistroEnum.CoordinacionCECOPI:
                        idsCecopi.Add(detalle.IdReferencia);
                        break;
                    case (int)ApartadoRegistroEnum.CoordinacionPMA:
                        idsPma.Add(detalle.IdReferencia);
                        break;
                    default:
                        idsReferencias.Add(detalle.IdReferencia);
                        break;
                }
            }

            // Buscar el registro por IdReferencia
            var registro = await _unitOfWork.Repository<Registro>()
                .GetByIdWithSpec(new RegistroWithFilteredDataSpecification(
                    registroActualizacion.IdReferencia, idsDirecciones: idsDirecciones, idsCecopi: idsCecopi, idsPma: idsPma));

            if (registro is null || registro.Borrado)
                throw new BadRequestException($"El registro de actualización con Id [{registroActualizacion.Id}] no tiene registro");

            return registro;
        }

        throw new BadRequestException($"No se ha proporcionado un IdregistroActualizacion válido");
    }

    private async Task ValidateTipoDireccionEmergencia(CreateOrUpdateDireccionCoordinacionCommand request)
    {
        var idsTipoDireccion = request.Direcciones.Select(d => d.IdTipoDireccionEmergencia).Distinct();
        var tiposDireccionExistentes = await _unitOfWork.Repository<TipoDireccionEmergencia>().GetAsync(td => idsTipoDireccion.Contains(td.Id));

        if (tiposDireccionExistentes.Count() != idsTipoDireccion.Count())
        {
            var idsInvalidos = idsTipoDireccion.Except(tiposDireccionExistentes.Select(td => td.Id)).ToList();
            throw new NotFoundException(nameof(TipoDireccionEmergencia), string.Join(", ", idsInvalidos));
        }
    }



    private async Task MapAndSaveDirecciones(CreateOrUpdateDireccionCoordinacionCommand request, Registro registro)
    {
        foreach (var direccionDto in request.Direcciones)
        {
            if (direccionDto.Id.HasValue && direccionDto.Id > 0)
            {
                var direccionExistente = registro.Direcciones.FirstOrDefault(d => d.Id == direccionDto.Id.Value);
                if (direccionExistente != null)
                {
                    var copiaOriginal = _mapper.Map<CreateOrUpdateDireccionDto>(direccionExistente);
                    var copiaNueva = _mapper.Map<CreateOrUpdateDireccionDto>(direccionDto);

                    if (!copiaOriginal.Equals(copiaNueva))
                    {
                        _mapper.Map(direccionDto, direccionExistente);
                        direccionExistente.Borrado = false;
                        direccionExistente.Archivo = await _fileService.MapArchivo(direccionDto.Archivo, direccionExistente.Archivo, ARCHIVOS_PATH, direccionDto.ActualizarFichero);
                    }
                }
                else
                {
                    await createNewDireccionAsync(direccionDto, registro);
                }
            }
            else
            {
                await createNewDireccionAsync(direccionDto, registro);
            }
        }
    }

    private async Task MapAndSaveCoordinacionesPMA(CreateOrUpdateDireccionCoordinacionCommand request, Registro registro)
    {
        foreach (var coordinacionPMADto in request.CoordinacionesPMA)
        {
            if (coordinacionPMADto.Id.HasValue && coordinacionPMADto.Id > 0)
            {
                var coordinacionPMAExistente = registro.CoordinacionesPMA.FirstOrDefault(d => d.Id == coordinacionPMADto.Id.Value);
                if (coordinacionPMAExistente != null)
                {
                    var copiaOriginal = _mapper.Map<CreateOrUpdateCoordinacionPmaDto>(coordinacionPMAExistente);
                    var copiaNueva = _mapper.Map<CreateOrUpdateCoordinacionPmaDto>(coordinacionPMADto);

                    if (!copiaOriginal.Equals(copiaNueva))
                    {
                        _mapper.Map(coordinacionPMADto, coordinacionPMAExistente);
                        coordinacionPMAExistente.Borrado = false;
                        coordinacionPMAExistente.Archivo = await _fileService.MapArchivo(coordinacionPMADto.Archivo, coordinacionPMAExistente.Archivo, ARCHIVOS_PATH, coordinacionPMADto.ActualizarFichero);
                    }
                }
                else
                {
                    await createNewCoordinacionPMAAsync(coordinacionPMADto, registro);
                }
            }
            else
            {
                await createNewCoordinacionPMAAsync(coordinacionPMADto, registro);
            }
        }
    }

    private async Task MapAndSaveCoordinacionesCECOPI(CreateOrUpdateDireccionCoordinacionCommand request, Registro registro)
    {
        foreach (var coordinacionCECOPIDto in request.CoordinacionesCECOPI)
        {
            if (coordinacionCECOPIDto.Id.HasValue && coordinacionCECOPIDto.Id > 0)
            {
                var coordinacionCECOPIExistente = registro.CoordinacionesCecopi.FirstOrDefault(d => d.Id == coordinacionCECOPIDto.Id.Value);
                if (coordinacionCECOPIExistente != null)
                {
                    var copiaOriginal = _mapper.Map<CreateOrUpdateCoordinacionCecopiDto>(coordinacionCECOPIExistente);
                    var copiaNueva = _mapper.Map<CreateOrUpdateCoordinacionCecopiDto>(coordinacionCECOPIDto);

                    if (!copiaOriginal.Equals(copiaNueva))
                    {
                        _mapper.Map(coordinacionCECOPIDto, coordinacionCECOPIExistente);
                        coordinacionCECOPIExistente.Borrado = false;
                        coordinacionCECOPIExistente.Archivo = await _fileService.MapArchivo(coordinacionCECOPIDto.Archivo, coordinacionCECOPIExistente.Archivo, ARCHIVOS_PATH, coordinacionCECOPIDto.ActualizarFichero);
                    }
                }
                else
                {
                    await createNewCoordinacionCECOPIasync(coordinacionCECOPIDto, registro);
                }
            }
            else
            {
                await createNewCoordinacionCECOPIasync(coordinacionCECOPIDto, registro);
            }
        }
    }



    private async Task createNewDireccionAsync(CreateOrUpdateDireccionDto direccion, Registro registro)
    {
        Direccion newDireccion = _mapper.Map<Direccion>(direccion);
        newDireccion.Id = 0;
        newDireccion.Archivo = await _fileService.MapArchivo(direccion.Archivo, null, ARCHIVOS_PATH,direccion.ActualizarFichero);
        registro.Direcciones.Add(newDireccion);
    }


    private async Task createNewCoordinacionPMAAsync(CreateOrUpdateCoordinacionPmaDto coordinacionPMA, Registro registro)
    {
        CoordinacionPMA newCoordinacion = _mapper.Map<CoordinacionPMA>(coordinacionPMA);
        newCoordinacion.Id = 0;
        newCoordinacion.Archivo = await _fileService.MapArchivo(coordinacionPMA.Archivo, null, ARCHIVOS_PATH, coordinacionPMA.ActualizarFichero);
        registro.CoordinacionesPMA.Add(newCoordinacion);
    }

    private async Task createNewCoordinacionCECOPIasync(CreateOrUpdateCoordinacionCecopiDto coordinacionCECOPI, Registro registro)
    {
        CoordinacionCecopi newCoordinacion = _mapper.Map<CoordinacionCecopi>(coordinacionCECOPI);
        newCoordinacion.Id = 0;
        newCoordinacion.Archivo = await _fileService.MapArchivo(coordinacionCECOPI.Archivo, null, ARCHIVOS_PATH, coordinacionCECOPI.ActualizarFichero);
        registro.CoordinacionesCecopi.Add(newCoordinacion);
    }



    private async Task<List<int>> DeleteLogicalDirecciones(CreateOrUpdateDireccionCoordinacionCommand request, Registro registro, int idRegistroActualizacion)
    {
        if (registro.Id > 0)
        {
            var idsEnRequest = request.Direcciones.Where(d => d.Id.HasValue && d.Id > 0).Select(d => d.Id).ToList();
            var direccionesParaEliminar = registro.Direcciones
                .Where(d => d.Id > 0 && !idsEnRequest.Contains(d.Id))
                .ToList();

            if (direccionesParaEliminar.Count == 0)
            {
                return new List<int>();
            }

            // Obtener el historial de creación de estas direcciones
            var idsDireccionesParaEliminar = direccionesParaEliminar.Select(d => d.Id).ToList();
            var historialDirecciones = await _unitOfWork.Repository<DetalleRegistroActualizacion>()
                .GetAsync(d =>
                idsDireccionesParaEliminar.Contains(d.IdReferencia) && d.IdApartadoRegistro == (int)ApartadoRegistroEnum.Direccion);

            foreach (var direccion in direccionesParaEliminar)
            {
                var historial = historialDirecciones.FirstOrDefault(d =>
                d.IdReferencia == direccion.Id &&
                (d.IdEstadoRegistro == EstadoRegistroEnum.Creado || d.IdEstadoRegistro == EstadoRegistroEnum.CreadoYModificado));

                if (historial == null || historial.IdRegistroActualizacion != idRegistroActualizacion)
                {
                    throw new BadRequestException($"La dirección con ID {direccion.Id} solo puede eliminarse en el registro en que fue creada.");
                }

                if (direccion.IdArchivo.HasValue)
                {
                    _unitOfWork.Repository<Archivo>().DeleteEntity(direccion.Archivo);
                }

                _unitOfWork.Repository<Direccion>().DeleteEntity(direccion);
            }

            return idsDireccionesParaEliminar;
        }

        return new List<int>();
    }



    private async Task SaveRegistro(Registro registro)
    {
        if (registro.Id > 0)
        {
            _unitOfWork.Repository<Registro>().UpdateEntity(registro);
        }

        if (await _unitOfWork.Complete() <= 0)
            throw new Exception("No se pudo actualizar el registro");
    }
}
