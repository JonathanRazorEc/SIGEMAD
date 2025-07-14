using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Incendios;
using DGPCE.Sigemad.Domain.Constracts;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Incendios.Commands.UpdateIncendios;

public class UpdateIncendioCommandHandler : IRequestHandler<UpdateIncendioCommand>
{
    private readonly ILogger<UpdateIncendioCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGeometryValidator _geometryValidator;
    private readonly ICoordinateTransformationService _coordinateTransformationService;
    private readonly IMapper _mapper;

    public UpdateIncendioCommandHandler(
        ILogger<UpdateIncendioCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IGeometryValidator geometryValidator,
        ICoordinateTransformationService coordinateTransformationService,
        IMapper mapper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _geometryValidator = geometryValidator;
        _coordinateTransformationService = coordinateTransformationService;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateIncendioCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(UpdateIncendioCommandHandler) + " - BEGIN");

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

        var incendioSpec = new IncendioActiveByIdSpecification(request.Id);
        var incendioToUpdate = await _unitOfWork.Repository<Incendio>().GetByIdWithSpec(incendioSpec);

        if (incendioToUpdate == null)
        {
            _logger.LogWarning($"No se encontro incendio con id: {request.Id}");
            throw new NotFoundException(nameof(Incendio), request.Id);
        }

        var (utmX, utmY, huso) = _coordinateTransformationService.ConvertToUTM(request.GeoPosicion);        

        incendioToUpdate.UtmX = (decimal?)utmX;
        incendioToUpdate.UtmY = (decimal?)utmY;
        incendioToUpdate.Huso = huso;        


        if (request.IdTerritorio == TipoTerritorio.Nacional)
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
            
        }
        else if (request.IdTerritorio == TipoTerritorio.Extranjero)
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
                incendioToUpdate.IdDistrito = municipioExtranjero.IdDistrito;
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

        _mapper.Map(request, incendioToUpdate, typeof(UpdateIncendioCommand), typeof(Incendio));

        if(request.IdTerritorio == TipoTerritorio.Nacional)
        {
            incendioToUpdate.IdPais = (int)PaisesEnum.Espana;
        }

        _unitOfWork.Repository<Incendio>().UpdateEntity(incendioToUpdate);
        await _unitOfWork.Complete();        

        _logger.LogInformation($"Se actualizo correctamente el incendio con id: {request.Id}");
        _logger.LogInformation(nameof(UpdateIncendioCommandHandler) + " - END");

        return Unit.Value;
    }
}
