using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Contracts.RegistrosActualizacion;
using DGPCE.Sigemad.Application.Dtos.AreasAfectadas;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.AreasAfectadas.Commands;
using DGPCE.Sigemad.Application.Specifications.Incendios;
using DGPCE.Sigemad.Application.Specifications.Registros;
using DGPCE.Sigemad.Domain.Constracts;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DGPCE.Sigemad.Application.Features.AreasAfectadas.Manage;
public class ManageAreaAfectadaCommandHandler : IRequestHandler<ManageAreaAfectadaCommand, CreateOrUpdateAreaAfectadaResponse>
{
    private readonly ILogger<ManageAreaAfectadaCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGeometryValidator _geometryValidator;
    private readonly ICoordinateTransformationService _coordinateTransformationService;
    private readonly IMapper _mapper;
    private readonly IRegistroActualizacionService _registroActualizacionService;

    public ManageAreaAfectadaCommandHandler(
        ILogger<ManageAreaAfectadaCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IGeometryValidator geometryValidator,
        ICoordinateTransformationService coordinateTransformationService,
        IMapper mapper,
        IRegistroActualizacionService registroActualizacionService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _geometryValidator = geometryValidator;
        _coordinateTransformationService = coordinateTransformationService;
        _mapper = mapper;
        _registroActualizacionService = registroActualizacionService;
    }


    public async Task<CreateOrUpdateAreaAfectadaResponse> Handle(ManageAreaAfectadaCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(ManageAreaAfectadaCommandHandler) + " - BEGIN");

        await _registroActualizacionService.ValidarSuceso(request.IdSuceso);
        await ValidateProvincia(request);
        await ValidateMunicipio(request);
        await ValidateEntidadMenor(request);
        await ValidateFechas(request);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            RegistroActualizacion registroActualizacion = await _registroActualizacionService.GetOrCreateRegistroActualizacion<Registro>(
                request.IdRegistroActualizacion, request.IdSuceso, TipoRegistroActualizacionEnum.Registro);

            var registro = await GetRegistro(request, registroActualizacion);

            var areasOriginales = registro.AreaAfectadas.ToDictionary(d => d.Id, d => _mapper.Map<CreateOrUpdateAreaAfectadaDto>(d));
            MapAndSaveAreasAfectadas(request, registro);

            var areasParaEliminar = await DeleteLogicalAreasAfectadas(request, registro, registroActualizacion.Id);
            await SaveRegistro(registro);

            await _registroActualizacionService.SaveRegistroActualizacion<
                Registro, AreaAfectada, CreateOrUpdateAreaAfectadaDto>(
                registroActualizacion,
                registro,
                ApartadoRegistroEnum.AreaAfectada,
                areasParaEliminar, areasOriginales);

            await _unitOfWork.CommitAsync();

            _logger.LogInformation($"{nameof(ManageAreaAfectadaCommandHandler)} - END");
            return new CreateOrUpdateAreaAfectadaResponse
            {
                IdRegistro = registro.Id,
                IdRegistroActualizacion = registroActualizacion.Id
            };

        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error en la transacción de CreateOrUpdateAreaAfectadaCommandHandler");
            throw;
        }
    }

    private async Task ValidateFechas(ManageAreaAfectadaCommand request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (request.AreasAfectadas == null)
            throw new BadRequestException("La lista de Areas Afectadas no puede estar vacia");

        var incendioAsociado = await _unitOfWork.Repository<Incendio>()
           .GetByIdWithSpec(new IncendioActiveByIdSpecification(request.IdSuceso))
           ?? throw new BadRequestException($"No se encontró el incendio con ID {request.IdSuceso}.");

        bool fechasValidas = request.AreasAfectadas.All(area => area.FechaHora > incendioAsociado.FechaInicio);

        bool fechasValidasActual = request.AreasAfectadas.All(area => area.FechaHora <= DateTime.Now);

        if (!fechasValidas)
        {
            throw new BadRequestException("Una o mas fechas es menor a la fecha del incendio asociado.");
        }

        if (!fechasValidasActual)
        {
            throw new BadRequestException("Una o mas fechas es mayor a la fecha del sistema.");
        }

    }

    private async Task<Registro> GetRegistro(ManageAreaAfectadaCommand request, RegistroActualizacion registroActualizacion)
    {
        if (registroActualizacion.IdReferencia > 0)
        {
            List<int> idsAreaAfectada = new List<int>();

            foreach (var detalle in registroActualizacion.DetallesRegistro)
            {
                if (detalle.IdApartadoRegistro == (int)ApartadoRegistroEnum.AreaAfectada)
                {
                    idsAreaAfectada.Add(detalle.IdReferencia);
                }
            }

            // Buscar el registro por IdReferencia
            var registro = await _unitOfWork.Repository<Registro>()
                .GetByIdWithSpec(new RegistroWithFilteredDataSpecification(
                    registroActualizacion.IdReferencia, idsAreaAfectada));

            if (registro is null || registro.Borrado)
                throw new BadRequestException($"El registro de actualización con Id [{registroActualizacion.Id}] no tiene registro");

            return registro;
        }

        throw new BadRequestException($"No se ha proporcionado un IdregistroActualizacion válido");
    }


    private async Task ValidateProvincia(ManageAreaAfectadaCommand request)
    {
        var idsProvincia = request.AreasAfectadas.Select(d => d.IdProvincia).Distinct();
        var provinciasExistentes = await _unitOfWork.Repository<Provincia>().GetAsync(td => idsProvincia.Contains(td.Id) && td.Borrado == false);

        if (provinciasExistentes.Count() != idsProvincia.Count())
        {
            var idsInvalidos = idsProvincia.Except(provinciasExistentes.Select(td => td.Id)).ToList();
            throw new NotFoundException(nameof(Provincia), string.Join(", ", idsInvalidos));
        }
    }

    private async Task ValidateMunicipio(ManageAreaAfectadaCommand request)
    {
        var idsMunicipio = request.AreasAfectadas.Select(d => d.IdMunicipio).Distinct();
        var municipiosExistentes = await _unitOfWork.Repository<Municipio>().GetAsync(td => idsMunicipio.Contains(td.Id) && td.Borrado == false);

        if (municipiosExistentes.Count() != idsMunicipio.Count())
        {
            var idsInvalidos = idsMunicipio.Except(municipiosExistentes.Select(td => td.Id)).ToList();
            throw new NotFoundException(nameof(Municipio), string.Join(", ", idsInvalidos));
        }
    }

    private async Task ValidateEntidadMenor(ManageAreaAfectadaCommand request)
    {
        var idsEntidadMenor = request.AreasAfectadas
        .Where(a => a.IdEntidadMenor.HasValue)
        .Select(d => d.IdEntidadMenor.Value) // Asegurar el uso de Value ya que es un Nullable<int>
        .Distinct() ?? Enumerable.Empty<int>();

        if (!idsEntidadMenor.Any()) return;

        var entidadExistentes = await _unitOfWork.Repository<EntidadMenor>().GetAsync(td => idsEntidadMenor.Contains(td.Id) && td.Borrado == false);

        if (entidadExistentes.Count() != idsEntidadMenor.Count())
        {
            var idsInvalidos = idsEntidadMenor.Except(entidadExistentes.Select(td => td.Id)).ToList();
            throw new NotFoundException(nameof(EntidadMenor), string.Join(", ", idsInvalidos));
        }
    }

    private void MapAndSaveAreasAfectadas(ManageAreaAfectadaCommand request, Registro registro)
    {
        foreach (var areaAfectadaDto in request.AreasAfectadas)
        {
            if (areaAfectadaDto.Id.HasValue && areaAfectadaDto.Id > 0)
            {
                var areaExistente = registro.AreaAfectadas.FirstOrDefault(d => d.Id == areaAfectadaDto.Id.Value);
                if (areaExistente != null)
                {
                    var copiaOriginal = _mapper.Map<CreateOrUpdateAreaAfectadaDto>(areaExistente);
                    var copiaNueva = _mapper.Map<CreateOrUpdateAreaAfectadaDto>(areaAfectadaDto);

                    if (!copiaOriginal.Equals(copiaNueva))
                    {
                        _mapper.Map(areaAfectadaDto, areaExistente);
                        areaExistente.Borrado = false;
                    }
                }
                else
                {
                    registro.AreaAfectadas.Add(_mapper.Map<AreaAfectada>(areaAfectadaDto));
                }
            }
            else
            {
                registro.AreaAfectadas.Add(_mapper.Map<AreaAfectada>(areaAfectadaDto));
            }
        }
    }

    private async Task<List<int>> DeleteLogicalAreasAfectadas(ManageAreaAfectadaCommand request, Registro registro, int idRegistroActualizacion)
    {
        if (registro.Id > 0)
        {
            var idsEnRequest = request.AreasAfectadas.Where(d => d.Id.HasValue && d.Id > 0).Select(d => d.Id).ToList();
            var areasParaEliminar = registro.AreaAfectadas
                .Where(d => d.Id > 0 && !idsEnRequest.Contains(d.Id) && d.Borrado == false)
                .ToList();

            if (areasParaEliminar.Count == 0)
            {
                return new List<int>();
            }

            // Obtener el historial de creación de estas direcciones
            var idsAreasParaEliminar = areasParaEliminar.Select(d => d.Id).ToList();
            var historialAreas = await _unitOfWork.Repository<DetalleRegistroActualizacion>()
                .GetAsync(d =>
                idsAreasParaEliminar.Contains(d.IdReferencia) && d.IdApartadoRegistro == (int)ApartadoRegistroEnum.AreaAfectada);

            foreach (var area in areasParaEliminar)
            {
                var historial = historialAreas.FirstOrDefault(d =>
                d.IdReferencia == area.Id &&
                (d.IdEstadoRegistro == EstadoRegistroEnum.Creado || d.IdEstadoRegistro == EstadoRegistroEnum.CreadoYModificado));

                if (historial == null || historial.IdRegistroActualizacion != idRegistroActualizacion)
                {
                    throw new BadRequestException($"El area afectada con ID {area.Id} solo puede eliminarse en el registro en que fue creada.");
                }

                _unitOfWork.Repository<AreaAfectada>().DeleteEntity(area);
            }

            return idsAreasParaEliminar;
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
