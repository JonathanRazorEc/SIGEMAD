using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Contracts.RegistrosActualizacion;
using DGPCE.Sigemad.Application.Dtos.Impactos;
using DGPCE.Sigemad.Application.Dtos.TipoImpactoEvolucion;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Commands;
using DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.CreateListaImpactoEvolucion;
using DGPCE.Sigemad.Application.Specifications.Registros;
using DGPCE.Sigemad.Domain;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.CreateListaImpactos;
public class ManageImpactosCommandHandler : IRequestHandler<ManageImpactosCommand, ManageImpactoResponse>
{
    private readonly ILogger<ManageImpactosCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRegistroActualizacionService _registroActualizacionService;

    public ManageImpactosCommandHandler(
        ILogger<ManageImpactosCommandHandler> logger,
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

    public async Task<ManageImpactoResponse> Handle(ManageImpactosCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(ManageImpactosCommandHandler)} - BEGIN");

        await _registroActualizacionService.ValidarSuceso(request.IdSuceso);
        await ValidateTipoImpactoAsync(request);
        await ValidateImpactosClasificadosAsync(request);
        await ValidateTiposDanioAsync(request);
        await ValidateAlteracionInterrupcionAsync(request);
        await ValidateZonaPlanificacionAsync(request);
        await ValidateProvincia(request);
        await ValidateMunicipio(request);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            RegistroActualizacion registroActualizacion = await _registroActualizacionService.GetOrCreateRegistroActualizacion<Registro>(
                request.IdRegistroActualizacion, request.IdSuceso, TipoRegistroActualizacionEnum.Registro);

            var registro = await GetRegistro(request, registroActualizacion);

            var tipoImpactosOriginales = registro.TipoImpactosEvoluciones.ToDictionary(d => d.Id, d => _mapper.Map<ManageTipoImpactoEvolucionDTO>(d));
            MapAndManageTipoImpactos(request, registro);

            var impactosParaEliminar = await DeleteLogicalImpactos(request, registro, registroActualizacion.Id);
            await SaveRegistro(registro);

            await _registroActualizacionService.SaveRegistroActualizacion<
                  Registro, TipoImpactoEvolucion, ManageTipoImpactoEvolucionDTO>(
                  registroActualizacion,
                  registro,
                  ApartadoRegistroEnum.ConsecuenciaActuacion,
                  impactosParaEliminar, tipoImpactosOriginales);

            await _unitOfWork.CommitAsync();

            _logger.LogInformation($"{nameof(ManageImpactosCommandHandler)} - END");

            return new ManageImpactoResponse
            {
                IdRegistro = registro.Id,
                IdRegistroActualizacion = registroActualizacion.Id
            };
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error en la transacción de ManageImpactosCommandHandler");
            throw;
        }
    }


    private async Task ValidateProvincia(ManageImpactosCommand request)
    {

        var idsProvincia = request.TipoImpactos
         .SelectMany(i => i.ImpactosEvoluciones)
         .Select(ie => ie.IdProvincia)
         .Where(id => id.HasValue) // Filtrar nulos
         .Select(id => id.Value)
         .Distinct()
         .ToList();

        var provinciasExistentes = await _unitOfWork.Repository<Provincia>().GetAsync(td => idsProvincia.Contains(td.Id) && td.Borrado == false);

        if (provinciasExistentes.Count() != idsProvincia.Count())
        {
            var idsInvalidos = idsProvincia
             .Except(provinciasExistentes.Select(td => td.Id))
             .ToList();

            throw new NotFoundException(nameof(Provincia), string.Join(", ", idsInvalidos));
        }
    }

    private async Task ValidateMunicipio(ManageImpactosCommand request)
    {

        var idsMunicipio = request.TipoImpactos
        .SelectMany(i => i.ImpactosEvoluciones)
        .Select(ie => ie.IdMunicipio)
        .Where(id => id.HasValue) // Filtrar nulos
         .Select(id => id.Value)
        .Distinct()
        .ToList();

        var municipiosExistentes = await _unitOfWork.Repository<Municipio>().GetAsync(td => idsMunicipio.Contains(td.Id) && td.Borrado == false);

        if (municipiosExistentes.Count() != idsMunicipio.Count())
        {
            var idsInvalidos =  idsMunicipio
           .Except(municipiosExistentes.Select(td => td.Id))
           .ToList();
            throw new NotFoundException(nameof(Municipio), string.Join(", ", idsInvalidos));
        }
    }


    private async Task<Registro> GetRegistro(ManageImpactosCommand request, RegistroActualizacion registroActualizacion)
    {
        if (registroActualizacion.IdReferencia > 0)
        {
            List<int> idsConsecuenciaActuacion = new List<int>();


            foreach (var detalle in registroActualizacion.DetallesRegistro)
            {
                if (detalle.IdApartadoRegistro == (int)ApartadoRegistroEnum.ConsecuenciaActuacion)
                {
                    idsConsecuenciaActuacion.Add(detalle.IdReferencia);
                }
            }

            // Buscar el registro por IdReferencia
            var registro = await _unitOfWork.Repository<Registro>()
                .GetByIdWithSpec(new RegistroWithFilteredDataSpecification(
                    registroActualizacion.IdReferencia, null,null, idsConsecuenciaActuacion));

            if (registro is null || registro.Borrado)
                throw new BadRequestException($"El registro de actualización con Id [{registroActualizacion.Id}] no tiene registro");
           
            return registro;
        }

        throw new BadRequestException($"No se ha proporcionado un IdregistroActualizacion válido");
    }

    private async Task ValidateImpactosClasificadosAsync(ManageImpactosCommand request)
    {
        var idsImpactosClasificados = request.TipoImpactos.SelectMany(i => i.ImpactosEvoluciones).Select(ie => ie.IdImpactoClasificado).Distinct();
        var impactosClasificadosExistentes = await _unitOfWork.Repository<ImpactoClasificado>().GetAsync(ic => idsImpactosClasificados.Contains(ic.Id));

        if (impactosClasificadosExistentes.Count() != idsImpactosClasificados.Count())
        {
            var idsImpactosClasificadosExistentes = impactosClasificadosExistentes.Select(ic => ic.Id).ToList();
            var idsImpactosClasificadosInvalidos = idsImpactosClasificados.Except(idsImpactosClasificadosExistentes).ToList();

            if (idsImpactosClasificadosInvalidos.Any())
            {
                _logger.LogWarning($"Los siguientes Id's de impacto clasificado: {string.Join(", ", idsImpactosClasificadosInvalidos)}, no se encontraron");
                throw new NotFoundException(nameof(ImpactoClasificado), string.Join(", ", idsImpactosClasificadosInvalidos));
            }
        }
    }


    private async Task ValidateTipoImpactoAsync(ManageImpactosCommand request)
    {
        var idsTipoImpactos = request.TipoImpactos.Select(ie => ie.IdTipoImpacto).Distinct();
        var tipoImpactosExistentes = await _unitOfWork.Repository<TipoImpacto>().GetAsync(ic => idsTipoImpactos.Contains(ic.Id));

        if (tipoImpactosExistentes.Count() != idsTipoImpactos.Count())
        {
            var idsTipoImpactosClasificadosExistentes = tipoImpactosExistentes.Select(ic => ic.Id).ToList();
            var idsTiposImpactosClasificadosInvalidos = idsTipoImpactos.Except(idsTipoImpactosClasificadosExistentes).ToList();

            if (idsTiposImpactosClasificadosInvalidos.Any())
            {
                _logger.LogWarning($"Los siguientes Id's de tipo impacto: {string.Join(", ", idsTiposImpactosClasificadosInvalidos)}, no se encontraron");
                throw new NotFoundException(nameof(ImpactoClasificado), string.Join(", ", idsTiposImpactosClasificadosInvalidos));
            }
        }
    }

    private async Task ValidateTiposDanioAsync(ManageImpactosCommand request)
    {
        var idsTipoDanio = request.TipoImpactos.SelectMany(i => i.ImpactosEvoluciones)
            .Where(i => i.IdTipoDanio.HasValue)
            .Select(i => i.IdTipoDanio.Value)
            .Distinct()
            .ToList();

        if (idsTipoDanio.Any())
        {
            var tiposDanioExistentes = await _unitOfWork.Repository<TipoDanio>().GetAsync(t => idsTipoDanio.Contains(t.Id));
            if (tiposDanioExistentes.Count() != idsTipoDanio.Count())
            {
                var idsTipoDanioExistentes = tiposDanioExistentes.Select(t => t.Id).ToList();
                var idsTipoDanioInvalidos = idsTipoDanio.Except(idsTipoDanioExistentes).ToList();

                if (idsTipoDanioInvalidos.Any())
                {
                    _logger.LogWarning($"Los siguientes Id's de tipos de daño: {string.Join(", ", idsTipoDanioInvalidos)}, no se encontraron");
                    throw new NotFoundException(nameof(TipoDanio), string.Join(", ", idsTipoDanioInvalidos));
                }
            }
        }
    }

    private async Task ValidateAlteracionInterrupcionAsync(ManageImpactosCommand request)
    {
        var idsAlteracion = request.TipoImpactos.SelectMany(i => i.ImpactosEvoluciones)
            .Where(i => i.IdAlteracionInterrupcion.HasValue)
            .Select(i => i.IdAlteracionInterrupcion.Value)
            .Distinct()
            .ToList();

        if (idsAlteracion.Any())
        {
            var alteracionExistentes = await _unitOfWork.Repository<AlteracionInterrupcion>().GetAsync(t => idsAlteracion.Contains(t.Id));
            if (alteracionExistentes.Count() != idsAlteracion.Count())
            {
                var idsAlteracionExistentes = alteracionExistentes.Select(t => t.Id).ToList();
                var idsAlteracionInvalidos = idsAlteracion.Except(idsAlteracionExistentes).ToList();

                if (idsAlteracionInvalidos.Any())
                {
                    _logger.LogWarning($"Los siguientes Id's de alteracion/interrupcion: {string.Join(", ", idsAlteracionInvalidos)}, no se encontraron");
                    throw new NotFoundException(nameof(AlteracionInterrupcion), string.Join(", ", idsAlteracionInvalidos));
                }
            }
        }
    }

    private async Task ValidateZonaPlanificacionAsync(ManageImpactosCommand request)
    {
        var idsZona = request.TipoImpactos.SelectMany(i => i.ImpactosEvoluciones)
            .Where(i => i.IdZonaPlanificacion.HasValue)
            .Select(i => i.IdZonaPlanificacion.Value)
            .Distinct()
            .ToList();

        if (idsZona.Any())
        {
            var zonaExistentes = await _unitOfWork.Repository<ZonaPlanificacion>().GetAsync(t => idsZona.Contains(t.Id));
            if (zonaExistentes.Count() != idsZona.Count())
            {
                var idsZonaExistentes = zonaExistentes.Select(t => t.Id).ToList();
                var idsZonaInvalidos = idsZona.Except(idsZonaExistentes).ToList();

                if (idsZonaInvalidos.Any())
                {
                    _logger.LogWarning($"Los siguientes Id's de Zona Planificacion: {string.Join(", ", idsZonaInvalidos)}, no se encontraron");
                    throw new NotFoundException(nameof(ZonaPlanificacion), string.Join(", ", idsZonaInvalidos));
                }
            }
        }
    }

    private void MapAndManageTipoImpactos(ManageImpactosCommand request, Registro registro)
    {
        foreach (var tipoimpactoDto in request.TipoImpactos)
        {
            if (tipoimpactoDto.Id.HasValue && tipoimpactoDto.Id > 0)
            {
                var tipoimpactoExistente = registro.TipoImpactosEvoluciones
                    .FirstOrDefault(d => d.Id == tipoimpactoDto.Id.Value);

                if (tipoimpactoExistente != null)
                {
                    var copiaOriginal = _mapper.Map<ManageTipoImpactoEvolucionDTO>(tipoimpactoExistente);
                    var copiaNueva = _mapper.Map<ManageTipoImpactoEvolucionDTO>(tipoimpactoDto);

                    if (!copiaOriginal.Equals(copiaNueva))
                    {
                        // Mapear propiedades simples, excluyendo la colección
                        tipoimpactoExistente.IdTipoImpacto = tipoimpactoDto.IdTipoImpacto;
                        tipoimpactoExistente.Estimado = tipoimpactoDto.Estimado;
                        tipoimpactoExistente.Borrado = false;


                        // Obtener IDs del DTO (solo los que tienen ID asignado)
                        var idsDto = tipoimpactoDto.ImpactosEvoluciones
                            .Where(i => i.Id.HasValue)
                            .Select(i => i.Id.Value)
                            .ToHashSet();

                        // Eliminar los que no están en el DTO
                        var impactosAEliminar = tipoimpactoExistente.ImpactosEvoluciones
                            .Where(i => !idsDto.Contains(i.Id))
                            .ToList();

                        foreach (var impacto in impactosAEliminar)
                        {
                            _unitOfWork.Repository<ImpactoEvolucion>().DeleteEntity(impacto);
                        }

                        // Agregar o actualizar los del DTO
                        foreach (var impactoDto in tipoimpactoDto.ImpactosEvoluciones)
                        {
                            if (impactoDto.Id.HasValue)
                            {
                                var impactoExistente = tipoimpactoExistente.ImpactosEvoluciones
                                    .FirstOrDefault(i => i.Id == impactoDto.Id.Value);

                                if (impactoExistente != null)
                                {
                                    _mapper.Map(impactoDto, impactoExistente);
                                }
                                else
                                {
                                    var nuevoImpacto = _mapper.Map<ImpactoEvolucion>(impactoDto);
                                    tipoimpactoExistente.ImpactosEvoluciones.Add(nuevoImpacto);
                                }
                            }
                            else
                            {
                                // Nuevo impacto sin ID
                                var nuevoImpacto = _mapper.Map<ImpactoEvolucion>(impactoDto);
                                tipoimpactoExistente.ImpactosEvoluciones.Add(nuevoImpacto);
                            }
                        }
                    }
                }
                else
                {
                    registro.TipoImpactosEvoluciones.Add(_mapper.Map<TipoImpactoEvolucion>(tipoimpactoDto));
                }
            }
            else
            {
                registro.TipoImpactosEvoluciones.Add(_mapper.Map<TipoImpactoEvolucion>(tipoimpactoDto));
            }
        }
    }



    private async Task<List<int>> DeleteLogicalImpactos(ManageImpactosCommand request, Registro registro, int idRegistroActualizacion)
    {
        if (registro.Id > 0)
        {
            var idsEnRequest = request.TipoImpactos
                .Where(d => d.Id.HasValue && d.Id > 0)
                .Select(d => d.Id)
                .ToList();

            var impactosParaEliminar = registro.TipoImpactosEvoluciones
                .Where(d => d.Id > 0 && !idsEnRequest.Contains(d.Id) && d.Borrado == false)
                .ToList();

            if (impactosParaEliminar.Count == 0)
            {
                return new List<int>();
            }

            // Obtener el historial de creación
            var idsImpactosParaEliminar = impactosParaEliminar.Select(d => d.Id).ToList();
            var historialAreas = await _unitOfWork.Repository<DetalleRegistroActualizacion>()
                .GetAsync(d =>
                idsImpactosParaEliminar.Contains(d.IdReferencia) && d.IdApartadoRegistro == (int)ApartadoRegistroEnum.ConsecuenciaActuacion);


            foreach (var Tipoimpacto in impactosParaEliminar)
            {
                var historial = historialAreas.FirstOrDefault(d =>
                d.IdReferencia == Tipoimpacto.Id &&
                (d.IdEstadoRegistro == EstadoRegistroEnum.Creado || d.IdEstadoRegistro == EstadoRegistroEnum.CreadoYModificado));

                if (historial == null || historial.IdRegistroActualizacion != idRegistroActualizacion)
                {
                    throw new BadRequestException($"La consecuencia/impacto con ID {Tipoimpacto.Id} solo puede eliminarse en el registro en que fue creada.");
                }

                foreach (var impacto in Tipoimpacto.ImpactosEvoluciones)
                {
                    impacto.Borrado = true;
                    _unitOfWork.Repository<ImpactoEvolucion>().DeleteEntity(impacto);
                }

                Tipoimpacto.Borrado = true;
                _unitOfWork.Repository<TipoImpactoEvolucion>().DeleteEntity(Tipoimpacto);
            }

            return idsImpactosParaEliminar;
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
