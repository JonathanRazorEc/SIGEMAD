using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Contracts.RegistrosActualizacion;
using DGPCE.Sigemad.Application.Dtos.ActivacionSistema;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.ActivacionesSistemas.Commands;
using DGPCE.Sigemad.Application.Features.NotificacionesEmergencias.Commands.ManageNotificacionEmergencia;
using DGPCE.Sigemad.Application.Specifications.Incendios;
using DGPCE.Sigemad.Application.Specifications.Registros;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.ActivacionesSistemas.Manage;
public class ManageActivacionSistemaCommandHandler : IRequestHandler<ManageActivacionSistemaCommand, ManageActivacionSistemaResponse>
{

    private readonly ILogger<ManageActivacionSistemaCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRegistroActualizacionService _registroActualizacionService;

    public ManageActivacionSistemaCommandHandler(
        ILogger<ManageActivacionSistemaCommandHandler> logger,
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

    public async Task<ManageActivacionSistemaResponse> Handle(ManageActivacionSistemaCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(ManageNotificacionEmergenciaHandler)} - BEGIN");

        await _registroActualizacionService.ValidarSuceso(request.IdSuceso);
        TieneActivacionesDuplicadas(request);
        await ValidateTipoSistemaEmergencia(request);
        await ValidateModosActivacion(request);
        await ValidateFechas(request);

        await _unitOfWork.BeginTransactionAsync();
  

        try
        {
            RegistroActualizacion registroActualizacion = await _registroActualizacionService.GetOrCreateRegistroActualizacion<Registro>(
                request.IdRegistroActualizacion, request.IdSuceso, TipoRegistroActualizacionEnum.Registro);

            var registro = await GetRegistro(request, registroActualizacion);
          

            var activacionSistemasOriginales = registro.ActivacionSistemas.ToDictionary(d => d.Id, d => _mapper.Map<ManageActivacionSistemaDto>(d));
            MapAndSaveActivaciones(request, registro);

            var activacionesParaEliminar = await DeleteLogicalActivaciones(request, registro, registroActualizacion.Id);

            await SaveRegistro(registro);

            await _registroActualizacionService.SaveRegistroActualizacion<
                Registro, ActivacionSistema, ManageActivacionSistemaDto>(
                registroActualizacion,
                registro,
                ApartadoRegistroEnum.ActivacionDeSistemas,
                activacionesParaEliminar, activacionSistemasOriginales);

            await _unitOfWork.CommitAsync();

            _logger.LogInformation($"{nameof(ManageNotificacionEmergenciaHandler)} - END");

            return new ManageActivacionSistemaResponse
            {
                IdRegistro = registro.Id,
                IdRegistroActualizacion = registroActualizacion.Id
            };

        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error en la transacción de ManageConvocatoriaCECODCommandHandler");
            throw;
        }
    }



    public void TieneActivacionesDuplicadas(ManageActivacionSistemaCommand request)
    {
        if (request.Detalles != null && request.Detalles.Any())
        {
            // Verificación de duplicados dentro de la misma petición
            var hayDuplicadosEnPeticion = request.Detalles
                .Where(d => d.FechaHoraActivacion.HasValue)
                .GroupBy(d => new { d.IdTipoSistemaEmergencia, d.FechaHoraActivacion })
                .Any(g => g.Count() > 1);

            if (hayDuplicadosEnPeticion)
            {
                throw new BadRequestException("Existen en la misma petición 2 o más activaciones con el mismo tipo y fecha.");
            }
        }
    }


    private async Task ValidateFechas(ManageActivacionSistemaCommand request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (request.Detalles == null)
            throw new BadRequestException("Los detalles de Activacion Sistema no pueden estar vacios.");

        var incendioAsociado = await _unitOfWork.Repository<Incendio>()
           .GetByIdWithSpec(new IncendioActiveByIdSpecification(request.IdSuceso))
           ?? throw new BadRequestException($"No se encontró el incendio con ID {request.IdSuceso}.");

        bool fechasValidasActivacion = request.Detalles.All(activacion => activacion.FechaHoraActivacion > incendioAsociado.FechaInicio);

        //bool fechasValidasActualizacion = request.Detalles.All(activacion => activacion.FechaHoraActualizacion > incendioAsociado.FechaInicio);

        bool fechasValidasActual = request.Detalles.All(activacion => activacion.FechaHoraActivacion <= DateTime.Now && activacion.FechaHoraActualizacion <= DateTime.Now);

        if (!fechasValidasActivacion)
        {
            throw new BadRequestException("Una o mas fechas de activacion es menor a la fecha del incendio asociado.");
        }

        //if (!fechasValidasActualizacion)
        //{
        //    throw new BadRequestException("Una o mas fechas de actualizacion es menor a la fecha del incendio asociado.");
        //}

        if (!fechasValidasActual)
        {
            throw new BadRequestException("Una o mas fechas es mayor a la fecha del sistema.");
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


    private async Task<List<int>> DeleteLogicalActivaciones(ManageActivacionSistemaCommand request, Registro registro, int idRegistroActualizacion)
    {
        if (registro.Id > 0)
        {
            var idsEnRequest = request.Detalles.Where(d => d.Id.HasValue && d.Id > 0).Select(d => d.Id).ToList();
            var activacionesSistemasParaEliminar = registro.ActivacionSistemas
                .Where(d => d.Id > 0 && !idsEnRequest.Contains(d.Id))
                .ToList();

            if (activacionesSistemasParaEliminar.Count == 0)
            {
                return new List<int>();
            }

            // Obtener el historial de creación de estas convocatorias
            var idsactivacionesSistemasParaEliminar = activacionesSistemasParaEliminar.Select(d => d.Id).ToList();
            var historialDirecciones = await _unitOfWork.Repository<DetalleRegistroActualizacion>()
                .GetAsync(d =>
                idsactivacionesSistemasParaEliminar.Contains(d.IdReferencia) && d.IdApartadoRegistro == (int)ApartadoRegistroEnum.ActivacionDeSistemas);

            foreach (var declaracion in activacionesSistemasParaEliminar)
            {
                var historial = historialDirecciones.FirstOrDefault(d =>
                d.IdReferencia == declaracion.Id &&
                (d.IdEstadoRegistro == EstadoRegistroEnum.Creado || d.IdEstadoRegistro == EstadoRegistroEnum.CreadoYModificado));

                if (historial == null || historial.IdRegistroActualizacion != idRegistroActualizacion)
                {
                    throw new BadRequestException($"La Activacion Sistema con ID {declaracion.Id} solo puede eliminarse en el registro en que fue creada.");
                }

                _unitOfWork.Repository<ActivacionSistema>().DeleteEntity(declaracion);
            }

            return idsactivacionesSistemasParaEliminar;
        }

        return new List<int>();
    }


    private void MapAndSaveActivaciones(ManageActivacionSistemaCommand request, Registro registro)
    {
        foreach (var activacionDto in request.Detalles)
        {
            if (activacionDto.Id.HasValue && activacionDto.Id > 0)
            {
                var activacionExistente = registro.ActivacionSistemas.FirstOrDefault(d => d.Id == activacionDto.Id.Value);
                if (activacionExistente != null)
                {
                    var copiaOriginal = _mapper.Map<ManageActivacionSistemaDto>(activacionExistente);
                    var copiaNueva = _mapper.Map<ManageActivacionSistemaDto>(activacionDto);

                    if (!copiaOriginal.Equals(copiaNueva))
                    {
                        _mapper.Map(activacionDto, activacionExistente);
                        activacionExistente.Borrado = false;
                    }
                }
                else
                {
                    registro.ActivacionSistemas.Add(_mapper.Map<ActivacionSistema>(activacionDto));
                }
            }
            else
            {
                registro.ActivacionSistemas.Add(_mapper.Map<ActivacionSistema>(activacionDto));
            }
        }
    }

    private async Task<Registro> GetRegistro(ManageActivacionSistemaCommand request, RegistroActualizacion registroActualizacion)
    {
        if (registroActualizacion.IdReferencia > 0)
        {
            List<int> idsActivacionSistemas = new List<int>();

            foreach (var detalle in registroActualizacion.DetallesRegistro)
            {
                if (detalle.IdApartadoRegistro == (int)ApartadoRegistroEnum.ActivacionDeSistemas)
                {
                    idsActivacionSistemas.Add(detalle.IdReferencia);
                }
            }

            // Buscar el registro por IdReferencia
            var registro = await _unitOfWork.Repository<Registro>()
                .GetByIdWithSpec(new RegistroWithFilteredDataSpecification(
                    registroActualizacion.IdReferencia, idsActivacionSistemas: idsActivacionSistemas));

            if (registro is null || registro.Borrado)
                throw new BadRequestException($"El registro de actualización con Id [{registroActualizacion.Id}] no tiene registro");

            return registro;
        }

        throw new BadRequestException($"No se ha proporcionado un IdregistroActualizacion válido");
    }


    private async Task ValidateModosActivacion(ManageActivacionSistemaCommand request)
    {
        var idsModosActivacion = request.Detalles.Select(c => c.IdModoActivacion).Where(c => c.HasValue).Distinct();
        var ModosActivacionExistentes = await _unitOfWork.Repository<ModoActivacion>().GetAsync(p => idsModosActivacion.Contains(p.Id));

        if (ModosActivacionExistentes.Count() != idsModosActivacion.Count())
        {
            var idsModosActivacionExistentes = ModosActivacionExistentes.Select(p => p.Id).Cast<int?>().ToList();
            var idsModosActivacionInvalidas = idsModosActivacion.Except(idsModosActivacionExistentes).ToList();

            if (idsModosActivacionInvalidas.Any())
            {
                _logger.LogWarning($"Los siguientes Id's de Modos de activación: {string.Join(", ", idsModosActivacionInvalidas)}, no se encontraron");
                throw new NotFoundException(nameof(ModoActivacion), string.Join(", ", idsModosActivacionInvalidas));
            }
        }
    }


    private async Task ValidateTipoSistemaEmergencia(ManageActivacionSistemaCommand request)
    {
        var idsTipoSistemaEmergencia = request.Detalles.Select(c => c.IdTipoSistemaEmergencia).Distinct();
        var tipoSistemaEmergenciaExistentes = await _unitOfWork.Repository<TipoSistemaEmergencia>().GetAsync(p => idsTipoSistemaEmergencia.Contains(p.Id));

        if (tipoSistemaEmergenciaExistentes.Count() != idsTipoSistemaEmergencia.Count())
        {
            var idsTipoSistemaEmergenciasExistentes = tipoSistemaEmergenciaExistentes.Select(p => p.Id).Cast<int>().ToList();
            var idsTipoSistemaEmergenciasInvalidas = idsTipoSistemaEmergencia.Except(idsTipoSistemaEmergenciasExistentes).ToList();

            if (idsTipoSistemaEmergenciasInvalidas.Any())
            {
                _logger.LogWarning($"Los siguientes Id's de Sistema Emergencia: {string.Join(", ", idsTipoSistemaEmergenciasInvalidas)}, no se encontraron");
                throw new NotFoundException(nameof(TipoPlan), string.Join(", ", idsTipoSistemaEmergenciasInvalidas));
            }
        }
    }


}