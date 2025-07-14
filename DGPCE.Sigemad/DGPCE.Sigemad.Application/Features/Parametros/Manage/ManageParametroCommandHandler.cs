using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Contracts.RegistrosActualizacion;
using DGPCE.Sigemad.Application.Dtos.Parametros.Dtos;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Commands.Manage;
using DGPCE.Sigemad.Application.Features.Parametros.Commands;
using DGPCE.Sigemad.Application.Specifications.Incendios;
using DGPCE.Sigemad.Application.Specifications.Registros;
using DGPCE.Sigemad.Application.Specifications.RegistrosActualizaciones;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Parametros.Manage;


public class ManageParametroCommandHandler : IRequestHandler<ManageParametroCommand, ManageParametroResponse>
{
    private readonly ILogger<ManageParametroCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRegistroActualizacionService _registroActualizacionService;


    public ManageParametroCommandHandler(
        ILogger<ManageParametroCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IRegistroActualizacionService registroActualizacionService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _registroActualizacionService = registroActualizacionService;
    }

    public async Task<ManageParametroResponse> Handle(ManageParametroCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(ManageParametroCommandHandler) + " - BEGIN");

        await _registroActualizacionService.ValidarSuceso(request.IdSuceso);
        await ComprobarParametros(request);
        await ValidateFechas(request);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            RegistroActualizacion registroActualizacion = await _registroActualizacionService.GetOrCreateRegistroActualizacion<Registro>(
                    request.IdRegistroActualizacion, request.IdSuceso, TipoRegistroActualizacionEnum.Registro);

            var registro = await GetRegistro(request, registroActualizacion);

            var parametrosOriginales = registro.Parametros.ToDictionary(d => d.Id, d => _mapper.Map<CreateOrUpdateParametroDto>(d));
            MapAndSaveParametros(request, registro);

            var parametrosParaEliminar = await DeleteLogicalAreasAfectadas(request, registro, registroActualizacion.Id);

            await ActualizarEstadoSuceso(request);

            //No hay listas para eliminar objeto
            await SaveRegistro(registro);

            await _registroActualizacionService.SaveRegistroActualizacion<
                Registro, Parametro, CreateOrUpdateParametroDto>(
                registroActualizacion,
                registro,
                ApartadoRegistroEnum.Parametro,
                parametrosParaEliminar, parametrosOriginales);

            await _unitOfWork.CommitAsync();

            _logger.LogInformation($"{nameof(CreateOrUpdateDireccionCoordinacionCommandHandler)} - END");
            return new ManageParametroResponse
            {
                Id = registro.Id,
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


    private async Task<List<int>> DeleteLogicalAreasAfectadas(ManageParametroCommand request, Registro registro, int idRegistroActualizacion)
    {
        if (registro.Id > 0)
        {
            var idsEnRequest = request.Parametro.Where(d => d.Id.HasValue && d.Id > 0).Select(d => d.Id).ToList();
            var parametrosParaEliminar = registro.Parametros
                .Where(d => d.Id > 0 && !idsEnRequest.Contains(d.Id) && d.Borrado == false)
                .ToList();

            if (parametrosParaEliminar.Count == 0)
            {
                return new List<int>();
            }

            // Obtener el historial de creación de estas direcciones
            var idsParametrosParaEliminar = parametrosParaEliminar.Select(d => d.Id).ToList();
            var historialParametros = await _unitOfWork.Repository<DetalleRegistroActualizacion>()
                .GetAsync(d =>
                idsParametrosParaEliminar.Contains(d.IdReferencia) && d.IdApartadoRegistro == (int)ApartadoRegistroEnum.Parametro);

            foreach (var parametro in parametrosParaEliminar)
            {
                var historial = historialParametros.FirstOrDefault(d =>
                d.IdReferencia == parametro.Id &&
                (d.IdEstadoRegistro == EstadoRegistroEnum.Creado || d.IdEstadoRegistro == EstadoRegistroEnum.CreadoYModificado));

                if (historial == null || historial.IdRegistroActualizacion != idRegistroActualizacion)
                {
                    throw new BadRequestException($"El parametro con ID {parametro.Id} solo puede eliminarse en el registro en que fue creada.");
                }

                _unitOfWork.Repository<Parametro>().DeleteEntity(parametro);
            }

            return idsParametrosParaEliminar;
        }

        return new List<int>();
    }




    private async Task ActualizarEstadoSuceso(ManageParametroCommand request)
    {
        bool estadoExtinguido = request.Parametro.Any(parametro =>
        {
            if (!parametro.IdEstadoIncendio.HasValue || parametro.IdEstadoIncendio.Value == 0)
            {
                return false;
            }

            var estadoIncendio = _unitOfWork.Repository<EstadoIncendio>().GetByIdAsync(parametro.IdEstadoIncendio.Value).Result;
            if (estadoIncendio is null || estadoIncendio.Borrado)
            {
                _logger.LogWarning($"parametro.IdEstadoIncendio: {parametro.IdEstadoIncendio}, no encontrado");
                throw new NotFoundException(nameof(EstadoIncendio), parametro.IdEstadoIncendio);
            }
            return estadoIncendio.Id == (int)TipoEstadoIncendioEnum.Extinguido;
        });

        if (estadoExtinguido)
        {
            var incendioParams = new IncendiosSpecificationParams
            {
                IdSuceso = request.IdSuceso
            };

            var spec = new IncendiosSpecification(incendioParams);
            var incendio = await _unitOfWork.Repository<Incendio>().GetByIdWithSpec(spec);

            if (incendio != null && incendio.IdEstadoSuceso != (int)TipoEstadoSucesoEnum.Cerrado)
            {
                incendio.IdEstadoSuceso = (int)TipoEstadoSucesoEnum.Cerrado;
                _unitOfWork.Repository<Incendio>().UpdateEntity(incendio);
            }
        }

    }


    private async Task ComprobarParametros(ManageParametroCommand request)
    {

        var idsFasesEmergencia = request.Parametro.Where(d => d.IdFaseEmergencia.HasValue && d.IdFaseEmergencia.Value != 0).Select(d => d.IdFaseEmergencia.Value).Distinct();

        var FasesExistentesExistentes = await _unitOfWork.Repository<FaseEmergencia>().GetAsync(td => idsFasesEmergencia.Contains(td.Id));

        if (FasesExistentesExistentes.Count() != idsFasesEmergencia.Count())
        {
            var idsInvalidos = idsFasesEmergencia.Except(FasesExistentesExistentes.Select(td => td.Id)).ToList();
            throw new NotFoundException(nameof(FaseEmergencia), string.Join(", ", idsInvalidos));
        }



        var idsPlanSituacion = request.Parametro
         .Where(d => d.IdPlanSituacion.HasValue && d.IdPlanSituacion.Value != 0)
         .Select(d => d.IdPlanSituacion.Value)
         .Distinct();

        var PlanSituacionExistentes = await _unitOfWork.Repository<PlanSituacion>().GetAsync(td => idsPlanSituacion.Contains(td.Id));

        if (PlanSituacionExistentes.Count() != idsPlanSituacion.Count())
        {
            var idsInvalidos = idsPlanSituacion.Except(PlanSituacionExistentes.Select(td => td.Id)).ToList();
            throw new NotFoundException(nameof(PlanSituacion), string.Join(", ", idsInvalidos));
        }



        var idsSituacionEquivalente = request.Parametro
         .Where(d => d.IdSituacionEquivalente.HasValue && d.IdSituacionEquivalente.Value != 0)
         .Select(d => d.IdSituacionEquivalente.Value)
         .Distinct();

        var SituacionEquivalenteExistentes = await _unitOfWork.Repository<SituacionEquivalente>().GetAsync(td => idsSituacionEquivalente.Contains(td.Id));

        if (SituacionEquivalenteExistentes.Count() != idsSituacionEquivalente.Count())
        {
            var idsInvalidos = idsSituacionEquivalente.Except(SituacionEquivalenteExistentes.Select(td => td.Id)).ToList();
            throw new NotFoundException(nameof(SituacionEquivalente), string.Join(", ", idsInvalidos));
        }

    }

    private async Task ValidarProcedenciasDestinos(List<int> registroProcedenciasDestinos)
    {
        var idsEvolucionProcedenciaDestinos = registroProcedenciasDestinos.Distinct();
        var evolucionProcedenciaDestinosExistentes = await _unitOfWork.Repository<ProcedenciaDestino>().GetAsync(p => idsEvolucionProcedenciaDestinos.Contains(p.Id));

        if (evolucionProcedenciaDestinosExistentes.Count() != idsEvolucionProcedenciaDestinos.Count())
        {
            var idsEvolucionProcedenciaDestinoExistentes = evolucionProcedenciaDestinosExistentes.Select(p => p.Id).ToList();
            var idsEvolucionProcedenciaDestinosExistentesInvalidas = idsEvolucionProcedenciaDestinos.Except(idsEvolucionProcedenciaDestinoExistentes).ToList();

            if (idsEvolucionProcedenciaDestinosExistentesInvalidas.Any())
            {
                _logger.LogWarning($"Las siguientes Id's de procedencia destinos: {string.Join(", ", idsEvolucionProcedenciaDestinosExistentesInvalidas)}, no se encontraron");
                throw new NotFoundException(nameof(ProcedenciaDestino), string.Join(", ", idsEvolucionProcedenciaDestinosExistentesInvalidas));
            }
        }
    }

    private async Task ValidateFechas(ManageParametroCommand request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (request.Parametro == null)
            throw new BadRequestException("El registro de Parámetro es obligatorio.");

        var incendioAsociado = await _unitOfWork.Repository<Incendio>()
            .GetByIdWithSpec(new IncendioActiveByIdSpecification(request.IdSuceso))
            ?? throw new BadRequestException($"No se encontró el incendio con ID {request.IdSuceso}.");

        // No puede ser anterior al inicio del incendio
        if (request.Parametro.Any(p => p.FechaHoraActualizacion < incendioAsociado.FechaInicio))
            throw new BadRequestException("La FechaHoraActualizacion no puede ser menor a la fecha de inicio del incendio.");

        // Comparamos solo la fecha (sin hora) contra la fecha actual del servidor en UTC
        var todayUtc = DateTime.UtcNow.Date;

        if (request.Parametro.Any(p =>
                p.FechaHoraActualizacion.HasValue
             && p.FechaHoraActualizacion.Value.ToUniversalTime().Date > todayUtc))
        {
            throw new BadRequestException("La FechaHoraActualizacion no puede ser mayor a la fecha actual.");
        }


        // Validación adicional sobre FechaFinal vs última actualización
        var registroActualizacion = await _unitOfWork.Repository<RegistroActualizacion>()
            .GetFirstOrDefaultAsync(new RegistroActualizacionByIdSucesoSpecification(request.IdSuceso));

        if (registroActualizacion != null && request.Parametro.Any(p => p.IdEstadoIncendio != 1))
        {
            if (request.Parametro.Any(p => p.FechaFinal < registroActualizacion.FechaCreacion))
                throw new BadRequestException("La fecha de Parámetro, Fecha Final no puede ser menor a la última fecha de actualización del incendio.");
        }
    }




    private async Task<Registro> GetRegistro(ManageParametroCommand request, RegistroActualizacion registroActualizacion)
    {
        if (registroActualizacion.IdReferencia > 0)
        {
            List<int> idsAreaAfectada = new List<int>();
            List<int> idsParametros = new List<int>();


            foreach (var detalle in registroActualizacion.DetallesRegistro)
            {
                if (detalle.IdApartadoRegistro == (int)ApartadoRegistroEnum.Parametro)
                {
                    idsParametros.Add(detalle.IdReferencia);
                }
            }

            // Buscar el registro por IdReferencia
            var registro = await _unitOfWork.Repository<Registro>()
                .GetByIdWithSpec(new RegistroWithFilteredDataSpecification(
                    registroActualizacion.IdReferencia, idsAreaAfectada, idsParametros));

            if (registro is null || registro.Borrado)
                throw new BadRequestException($"El registro de actualización con Id [{registroActualizacion.Id}] no tiene registro");

            return registro;
        }

        throw new BadRequestException($"No se ha proporcionado un IdregistroActualizacion válido");
    }

    private void MapAndSaveParametros(ManageParametroCommand request, Registro registro)
    {
        foreach (var parametroDto in request.Parametro)
        {
            if (parametroDto.Id.HasValue && parametroDto.Id > 0)
            {
                var parametroExistente = registro.Parametros.FirstOrDefault(d => d.Id == parametroDto.Id.Value);
                if (parametroExistente != null)
                {
                    var copiaOriginal = _mapper.Map<CreateOrUpdateParametroDto>(parametroExistente);
                    var copiaNueva = _mapper.Map<CreateOrUpdateParametroDto>(parametroDto);

                    if (!copiaOriginal.Equals(copiaNueva))
                    {
                        _mapper.Map(parametroDto, parametroExistente);
                        parametroExistente.Borrado = false;
                    }
                }
                else
                {
                    registro.Parametros.Add(_mapper.Map<Parametro>(parametroDto));
                }
            }
            else
            {
                registro.Parametros.Add(_mapper.Map<Parametro>(parametroDto));
            }
        }
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