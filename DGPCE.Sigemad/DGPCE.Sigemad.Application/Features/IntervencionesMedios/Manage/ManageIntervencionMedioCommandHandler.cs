using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Contracts.RegistrosActualizacion;
using DGPCE.Sigemad.Application.Dtos.IntervencionMedios;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Commands.Manage;
using DGPCE.Sigemad.Application.Features.IntervencionesMedios.Commands;
using DGPCE.Sigemad.Application.Specifications.Incendios;
using DGPCE.Sigemad.Application.Specifications.Registros;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.IntervencionesMedios.Manage;
public class ManageIntervencionMedioCommandHandler : IRequestHandler<ManageIntervencionMedioCommand, ManageIntervencionMedioResponse>
{
    private readonly ILogger<ManageIntervencionMedioCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRegistroActualizacionService _registroActualizacionService;
    public ManageIntervencionMedioCommandHandler(
    ILogger<ManageIntervencionMedioCommandHandler> logger,
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

    public async Task<ManageIntervencionMedioResponse> Handle(ManageIntervencionMedioCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(CreateOrUpdateDireccionCoordinacionCommandHandler)} - BEGIN");

        await _registroActualizacionService.ValidarSuceso(request.IdSuceso);
        await ValidateCaracterMedio(request);
        await ValidateTitularMedio(request);
        await ValidateCapacidad(request);
        await ValidateProvincia(request);
        await ValidateMunicipio(request);
        await ValidateMediosCapacidad(request);
        await ValidateFechas(request);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            RegistroActualizacion registroActualizacion = await _registroActualizacionService.GetOrCreateRegistroActualizacion<Registro>(
                request.IdRegistroActualizacion, request.IdSuceso, TipoRegistroActualizacionEnum.Registro);

            var registro = await GetRegistro(request, registroActualizacion);

            var intervencionesOriginales = registro.IntervencionMedios.ToDictionary(d => d.Id, d => _mapper.Map<CreateOrUpdateIntervencionMedioDto>(d));
            MapAndSaveIntervenciones(request, registro);

            var intervencionesParaEliminar = await DeleteLogicalIntervenciones(request, registro, registroActualizacion.Id);
            await SaveRegistro(registro);

            await _registroActualizacionService.SaveRegistroActualizacion<
                Registro, IntervencionMedio, CreateOrUpdateIntervencionMedioDto>(
                registroActualizacion,
                registro,
                ApartadoRegistroEnum.IntervencionMedios,
                intervencionesParaEliminar, intervencionesOriginales);

            await _unitOfWork.CommitAsync();

            _logger.LogInformation($"{nameof(CreateOrUpdateDireccionCoordinacionCommandHandler)} - END");
            return new ManageIntervencionMedioResponse
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

    private async Task<Registro> GetRegistro(ManageIntervencionMedioCommand request, RegistroActualizacion registroActualizacion)
    {
        if (registroActualizacion.IdReferencia > 0)
        {
            List<int> idsIntervencionMedios = new List<int>();


            foreach (var detalle in registroActualizacion.DetallesRegistro)
            {
                if (detalle.IdApartadoRegistro == (int)ApartadoRegistroEnum.IntervencionMedios)
                {
                    idsIntervencionMedios.Add(detalle.IdReferencia);
                }
            }

            // Buscar el registro por IdReferencia
            var registro = await _unitOfWork.Repository<Registro>()
                .GetByIdWithSpec(new RegistroWithFilteredDataSpecification(
                    registroActualizacion.IdReferencia, null, null, null, idsIntervencionMedios));

            if (registro is null || registro.Borrado)
                throw new BadRequestException($"El registro de actualización con Id [{registroActualizacion.Id}] no tiene registro");

            return registro;
        }

        throw new BadRequestException($"No se ha proporcionado un IdregistroActualizacion válido");
    }

    private async Task ValidateFechas(ManageIntervencionMedioCommand request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var incendioAsociado = await _unitOfWork.Repository<Incendio>()
            .GetByIdWithSpec(new IncendioActiveByIdSpecification(request.IdSuceso))
            ?? throw new BadRequestException($"No se encontró el incendio con ID {request.IdSuceso}.");

        bool fechaInicioValidas = request.Intervenciones.All(intervenccion => intervenccion.FechaHoraInicio > incendioAsociado.FechaInicio);

        bool fechaInicioFinValidas = request.Intervenciones.All(intervenccion => intervenccion.FechaHoraFin == null ? true : intervenccion.FechaHoraFin > intervenccion.FechaHoraInicio);

        bool fechaInicioActual = request.Intervenciones.All(intervenccion => intervenccion.FechaHoraInicio <= DateTime.Now);

        if (!fechaInicioValidas)
        {
            throw new BadRequestException("Una o mas fechas es menor a la fecha del incendio asociado.");
        }

        if (!fechaInicioFinValidas)
        {
            throw new BadRequestException("Una o mas fechas es menor a la fecha de inicio.");
        }

        if (!fechaInicioActual)
        {
            throw new BadRequestException("Una o mas fechas es mayor a la fecha del sistema.");
        }

    }


    private async Task ValidateCaracterMedio(ManageIntervencionMedioCommand request)
    {
        var idsCaracterMedio = request.Intervenciones.Select(d => d.IdCaracterMedio).Distinct();
        var caracterMediosExistentes = await _unitOfWork.Repository<CaracterMedio>().GetAsync(td => idsCaracterMedio.Contains(td.Id) && td.Obsoleto == false);

        if (caracterMediosExistentes.Count() != idsCaracterMedio.Count())
        {
            var idsInvalidos = idsCaracterMedio.Except(caracterMediosExistentes.Select(td => td.Id)).ToList();
            throw new NotFoundException(nameof(CaracterMedio), string.Join(", ", idsInvalidos));
        }
    }

    private async Task ValidateTitularMedio(ManageIntervencionMedioCommand request)
    {
        var idsTitularMedio = request.Intervenciones.Select(d => d.IdCaracterMedio).Distinct();
        var titularMediosExistentes = await _unitOfWork.Repository<TitularidadMedio>().GetAsync(td => idsTitularMedio.Contains(td.Id));

        if (titularMediosExistentes.Count() != idsTitularMedio.Count())
        {
            var idsInvalidos = idsTitularMedio.Except(titularMediosExistentes.Select(td => td.Id)).ToList();
            throw new NotFoundException(nameof(TitularidadMedio), string.Join(", ", idsInvalidos));
        }
    }

    private async Task ValidateCapacidad(ManageIntervencionMedioCommand request)
    {
        var idsCapacidad = request.Intervenciones.Select(d => d.IdCapacidad).Distinct();
        var capacidadesExistentes = await _unitOfWork.Repository<Capacidad>().GetAsync(td => idsCapacidad.Contains(td.Id));

        if (capacidadesExistentes.Count() != idsCapacidad.Count())
        {
            var idsInvalidos = idsCapacidad.Except(capacidadesExistentes.Select(td => td.Id)).ToList();
            throw new NotFoundException(nameof(Capacidad), string.Join(", ", idsInvalidos));
        }
    }

    private async Task ValidateProvincia(ManageIntervencionMedioCommand request)
    {
        var idsProvincia = request.Intervenciones
            .Select(d => d.IdProvincia)
            .Where(id => id.HasValue)
            .Select(id => id.Value)
            .Distinct()
            .ToList();

        var provinciasExistentes = await _unitOfWork.Repository<Provincia>()
            .GetAsync(td => idsProvincia.Contains(td.Id) && td.Borrado == false);

        if (provinciasExistentes.Count() != idsProvincia.Count)
        {
            var idsInvalidos = idsProvincia
                .Except(provinciasExistentes.Select(td => td.Id))
                .ToList();

            throw new NotFoundException(nameof(Provincia), string.Join(", ", idsInvalidos));
        }
    }


    private async Task ValidateMunicipio(ManageIntervencionMedioCommand request)
    {
        var idsMunicipio = request.Intervenciones
            .Select(d => d.IdMunicipio)
            .Where(id => id.HasValue)
            .Select(id => id.Value)
            .Distinct()
            .ToList();

        var municipiosExistentes = await _unitOfWork.Repository<Municipio>()
            .GetAsync(td => idsMunicipio.Contains(td.Id) && td.Borrado == false);

        if (municipiosExistentes.Count() != idsMunicipio.Count)
        {
            var idsInvalidos = idsMunicipio
                .Except(municipiosExistentes.Select(td => td.Id))
                .ToList();

            throw new NotFoundException(nameof(Municipio), string.Join(", ", idsInvalidos));
        }
    }


    private async Task ValidateMediosCapacidad(ManageIntervencionMedioCommand request)
    {
        // Flatten the nested lists into a single list of unique IDs
        var idsMediosCapacidad = request.Intervenciones
            .SelectMany(d => d.DetalleIntervencionMedios.Select(dt => dt.IdMediosCapacidad))
            .Distinct()
            .ToList(); // Ensure it is a list for Contains() operation

        if (!idsMediosCapacidad.Any())
        {
            return; // No capacity IDs to validate
        }
        var mediosCapacidadExistentes = await _unitOfWork.Repository<MediosCapacidad>().GetAsync(td => idsMediosCapacidad.Contains(td.Id));

        if (mediosCapacidadExistentes.Count() != idsMediosCapacidad.Count())
        {
            var idsInvalidos = idsMediosCapacidad.Except(mediosCapacidadExistentes.Select(td => td.Id)).ToList();
            throw new NotFoundException(nameof(MediosCapacidad), string.Join(", ", idsInvalidos));
        }
    }

    private void MapAndSaveIntervenciones(ManageIntervencionMedioCommand request, Registro registro)
    {
        foreach (var intervencionDto in request.Intervenciones)
        {
            if (intervencionDto.Id.HasValue && intervencionDto.Id > 0)
            {
                var intervencionExistente = registro.IntervencionMedios.FirstOrDefault(d => d.Id == intervencionDto.Id.Value);
                if (intervencionExistente != null)
                {
                    var copiaOriginal = _mapper.Map<CreateOrUpdateIntervencionMedioDto>(intervencionExistente);
                    var copiaNueva = _mapper.Map<CreateOrUpdateIntervencionMedioDto>(intervencionDto);

                    if (!copiaOriginal.Equals(copiaNueva))
                    {
                        _mapper.Map(intervencionDto, intervencionExistente);
                        intervencionExistente.Borrado = false;
                    }
                }
                else
                {
                    registro.IntervencionMedios.Add(_mapper.Map<IntervencionMedio>(intervencionDto));
                }
            }
            else
            {
                registro.IntervencionMedios.Add(_mapper.Map<IntervencionMedio>(intervencionDto));
            }
        }
    }

    private async Task<List<int>> DeleteLogicalIntervenciones(ManageIntervencionMedioCommand request, Registro registro, int idRegistroActualizacion)
    {
        if (registro.Id > 0)
        {
            var idsEnRequest = request.Intervenciones.Where(d => d.Id.HasValue && d.Id > 0).Select(d => d.Id).ToList();
            var intervencionesParaEliminar = registro.IntervencionMedios
                .Where(d => d.Id > 0 && !idsEnRequest.Contains(d.Id) && d.Borrado == false)
                .ToList();

            if (intervencionesParaEliminar.Count == 0)
            {
                return new List<int>();
            }

            // Obtener el historial de creación de estas direcciones
            var idsIntervencionesParaEliminar = intervencionesParaEliminar.Select(d => d.Id).ToList();
            var historialIntervenciones = await _unitOfWork.Repository<DetalleRegistroActualizacion>()
                .GetAsync(d =>
                idsIntervencionesParaEliminar.Contains(d.IdReferencia) && d.IdApartadoRegistro == (int)ApartadoRegistroEnum.IntervencionMedios);

            foreach (var intervencion in intervencionesParaEliminar)
            {
                var historial = historialIntervenciones.FirstOrDefault(d =>
                d.IdReferencia == intervencion.Id &&
                (d.IdEstadoRegistro == EstadoRegistroEnum.Creado || d.IdEstadoRegistro == EstadoRegistroEnum.CreadoYModificado));

                foreach (var detalle in intervencion.DetalleIntervencionMedios)
                {
                    _unitOfWork.Repository<DetalleIntervencionMedio>().DeleteEntity(detalle);
                }

                if (historial == null || historial.IdRegistroActualizacion != idRegistroActualizacion)
                {
                    throw new BadRequestException($"Intervencion de medio con ID {intervencion.Id} solo puede eliminarse en el registro en que fue creada.");
                }

                _unitOfWork.Repository<IntervencionMedio>().DeleteEntity(intervencion);
            }

            return idsIntervencionesParaEliminar;
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
