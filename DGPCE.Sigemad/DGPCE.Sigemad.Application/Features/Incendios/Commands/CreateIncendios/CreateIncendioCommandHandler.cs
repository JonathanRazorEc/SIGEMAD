using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Contracts.RegistrosActualizacion;
using DGPCE.Sigemad.Application.Dtos.AreasAfectadas;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.Registros.Command.CreateRegistros;
using DGPCE.Sigemad.Domain.Constracts;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Incendios.Commands.CreateIncendios;

public class CreateIncendioCommandHandler : IRequestHandler<CreateIncendioCommand, CreateIncendioResponse>
{
    private readonly ILogger<CreateIncendioCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGeometryValidator _geometryValidator;
    private readonly ICoordinateTransformationService _coordinateTransformationService;
    private readonly IMapper _mapper;
    private readonly IRegistroActualizacionService _registroActualizacionService;

    public CreateIncendioCommandHandler(
        ILogger<CreateIncendioCommandHandler> logger,
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

    public async Task<CreateIncendioResponse> Handle(CreateIncendioCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(CreateIncendioCommandHandler) + " - BEGIN");

        var territorio = await _unitOfWork.Repository<Territorio>().GetByIdAsync((int)request.IdTerritorio);
        if (territorio is null)
        {
            _logger.LogWarning($"request.IdTerritorio: {request.IdTerritorio}, no encontrado");
            throw new NotFoundException(nameof(Territorio), request.IdTerritorio);
        }

        var claseSuceso = await _unitOfWork.Repository<ClaseSuceso>().GetByIdAsync(request.IdClaseSuceso);
        if (claseSuceso is null)
        {
            _logger.LogWarning($"request.IdTipoSuceso: {request.IdClaseSuceso}, no encontrado");
            throw new NotFoundException(nameof(ClaseSuceso), request.IdClaseSuceso);
        }

        var estado = await _unitOfWork.Repository<EstadoSuceso>().GetByIdAsync(request.IdEstadoSuceso);
        if (estado is null)
        {
            _logger.LogWarning($"request.IdEstado: {request.IdEstadoSuceso}, no encontrado");
            throw new NotFoundException(nameof(EstadoSuceso), request.IdEstadoSuceso);
        }

        if (!_geometryValidator.IsGeometryValidAndInEPSG4326(request.GeoPosicion))
        {
            ValidationFailure validationFailure = new ValidationFailure();
            validationFailure.ErrorMessage = "No es una geometria valida o no tiene el EPS4326";

            _logger.LogWarning($"{validationFailure}, geometria -> {request.GeoPosicion}");
            throw new ValidationException(new List<ValidationFailure> { validationFailure });
        }

        var suceso = new Suceso
        {
            IdTipo = (int)TipoSucesoEnum.IncendioForestal,
        };

        var (utmX, utmY, huso) = _coordinateTransformationService.ConvertToUTM(request.GeoPosicion);


        var incendioEntity = _mapper.Map<Incendio>(request);
        incendioEntity.UtmX = (decimal?)utmX;
        incendioEntity.UtmY = (decimal?)utmY;
        incendioEntity.Huso = huso;
        incendioEntity.Suceso = suceso;


        if(request.IdTerritorio == TipoTerritorio.Nacional)
        {
            var provincia = await _unitOfWork.Repository<Provincia>().GetByIdAsync(request.IdProvincia.Value);
            if (provincia is null)
            {
                _logger.LogWarning($"request.IdProvincia: {request.IdProvincia}, no encontrado");
                throw new NotFoundException(nameof(Provincia), request.IdProvincia);
            }

            var municipio = await _unitOfWork.Repository<Municipio>().GetByIdAsync(request.IdMunicipio.Value);
            if (municipio is null)
            {
                _logger.LogWarning($"request.IdMunicipio: {request.IdMunicipio}, no encontrado");
                throw new NotFoundException(nameof(Municipio), request.IdMunicipio);
            }
            
            incendioEntity.IdPais = (int)PaisesEnum.Espana;
        } 
        else if(request.IdTerritorio == TipoTerritorio.Extranjero)
        {
            var pais = await _unitOfWork.Repository<Pais>().GetByIdAsync(request.IdPais.Value);
            if (pais is null)
            {
                _logger.LogWarning($"request.IdPais: {request.IdPais}, no encontrado");
                throw new NotFoundException(nameof(Pais), request.IdPais);
            }

            /*
            var distrito = await _unitOfWork.Repository<Distrito>().GetByIdAsync(request.IdDistrito.Value);
            if (distrito is null)
            {
                _logger.LogWarning($"request.IdDistrito: {request.IdDistrito}, no encontrado");
                throw new NotFoundException(nameof(Distrito), request.IdDistrito);
            }
            */
            if (request.IdMunicipioExtranjero.HasValue)
            {
                var municipioExtranjero = await _unitOfWork.Repository<MunicipioExtranjero>().GetByIdAsync(request.IdMunicipioExtranjero.Value);
                if (municipioExtranjero is null)
                {
                    _logger.LogWarning($"request.IdMunicipioExtranjero: {request.IdMunicipioExtranjero}, no encontrado");
                    throw new NotFoundException(nameof(EntidadMenor), request.IdMunicipioExtranjero);
                }
                incendioEntity.IdDistrito = municipioExtranjero.IdDistrito;
            }
                        
            if (request.IdProvincia.HasValue)
            {
                var provincia = await _unitOfWork.Repository<Provincia>().GetByIdAsync(request.IdProvincia.Value);
                if (provincia is null)
                {
                    _logger.LogWarning($"request.IdProvincia: {request.IdProvincia}, no encontrado");
                    throw new NotFoundException(nameof(Provincia), request.IdProvincia);
                }
            }

            if (request.IdMunicipio.HasValue)
            {
                var municipio = await _unitOfWork.Repository<Municipio>().GetByIdAsync(request.IdMunicipio.Value);
                if (municipio is null)
                {
                    _logger.LogWarning($"request.IdMunicipio: {request.IdMunicipio}, no encontrado");
                    throw new NotFoundException(nameof(Municipio), request.IdMunicipio);
                }
            }
        }

        if (request.EsLimitrofe)
        {
            incendioEntity.IdTerritorio = (int)TipoTerritorio.Transfronterizo;
        }

        Registro registro = null;

        if (request.IdTerritorio == TipoTerritorio.Nacional)
        {
            registro = new Registro
            {
                IdMedio = 12,
                IdEntradaSalida = 5,
                FechaHoraEvolucion = DateTimeOffset.UtcNow.UtcDateTime,
                ProcedenciaDestinos = new List<RegistroProcedenciaDestino>
        {
            new RegistroProcedenciaDestino
            {
                IdProcedenciaDestino = 181
            }
        },
                AreaAfectadas = new List<AreaAfectada>
        {
            new AreaAfectada
            {
                FechaHora = DateTimeOffset.UtcNow.UtcDateTime,
                IdProvincia = request.IdProvincia.Value,
                IdMunicipio = request.IdMunicipio.Value,
                Observaciones = "Registro automático"
            }
        }
            };

            incendioEntity.Suceso.Registros = new List<Registro> { registro };
        }

        _unitOfWork.Repository<Incendio>().AddEntity(incendioEntity);

        var result = await _unitOfWork.Complete();
        if (result <= 0)
        {
            throw new Exception("No se pudo insertar nuevo incendio");
        }


        if (registro != null && request.IdTerritorio == TipoTerritorio.Nacional)
        {
            RegistroActualizacion registroActualizacion = await _registroActualizacionService.GetOrCreateRegistroActualizacion<Registro>(
                null, incendioEntity.IdSuceso, TipoRegistroActualizacionEnum.Registro);

            await _registroActualizacionService.SaveRegistroActualizacion<
               Registro, Registro, CreateRegistroCommand>(
               registroActualizacion,
               registro,
                ApartadoRegistroEnum.Registro,
                new List<int>(),
                new Dictionary<int, CreateRegistroCommand>()
                );


            await _registroActualizacionService.SaveRegistroActualizacion<
                Registro, AreaAfectada, CreateOrUpdateAreaAfectadaDto>(
                registroActualizacion,
                registro,
                ApartadoRegistroEnum.AreaAfectada,
                new List<int>(), new());
        }


        _logger.LogInformation($"El incendio {incendioEntity.Id} fue creado correctamente");

        _logger.LogInformation(nameof(CreateIncendioCommandHandler) + " - END");
        return new CreateIncendioResponse { Id = incendioEntity.Id } ;
    }
}
