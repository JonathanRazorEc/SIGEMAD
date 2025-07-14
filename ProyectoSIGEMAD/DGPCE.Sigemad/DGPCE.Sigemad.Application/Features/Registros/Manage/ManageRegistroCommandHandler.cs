using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Contracts.RegistrosActualizacion;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.Registros.Command.CreateRegistros;
using DGPCE.Sigemad.Application.Specifications.Incendios;
using DGPCE.Sigemad.Application.Specifications.Registros;
using DGPCE.Sigemad.Application.Specifications.RegistrosActualizaciones;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DGPCE.Sigemad.Application.Features.Registros.CreateRegistros;

public class ManageRegistroCommandHandler : IRequestHandler<CreateRegistroCommand, ManageRegistroResponse>
{
    private readonly ILogger<ManageRegistroCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRegistroActualizacionService _registroActualizacionService;

    public ManageRegistroCommandHandler(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        ILogger<ManageRegistroCommandHandler> logger,
        IRegistroActualizacionService registroActualizacionService)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _registroActualizacionService = registroActualizacionService;
    }

    public async Task<ManageRegistroResponse> Handle(CreateRegistroCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(ManageRegistroCommandHandler) + " - BEGIN");

        await _registroActualizacionService.ValidarSuceso(request.IdSuceso);
        await ComprobarRegistro(request);
        await ValidateFechas(request);
        await ValidarProcedenciasDestinos(request.RegistroProcedenciasDestinos);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            RegistroActualizacion registroActualizacion = await _registroActualizacionService.GetOrCreateRegistroActualizacion<Registro>(
                request.IdRegistroActualizacion, request.IdSuceso, TipoRegistroActualizacionEnum.Registro);

            var registro = await GetOrCreateRegistro(request, registroActualizacion);

            await SaveRegistro(registro,request);

            //await _registroActualizacionService.SaveCabeceraRegistroActualizacion<
            //    Registro, Registro, CreateRegistroCommand>(
            //    registroActualizacion,
            //    registro,
            //    ApartadoRegistroEnum.Registro);



            await _registroActualizacionService.SaveRegistroActualizacion<
               Registro, Registro, CreateRegistroCommand>(
               registroActualizacion,
               registro,
                ApartadoRegistroEnum.Registro,
                new List<int>(),
                new Dictionary<int, CreateRegistroCommand>()
                );


            await _unitOfWork.CommitAsync();

            _logger.LogInformation($"{nameof(ManageRegistroCommandHandler)} - END");
            return new ManageRegistroResponse
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


    private async Task SaveRegistro(Registro registro, CreateRegistroCommand request)
    {
        _mapper.Map(request, registro);
        if (registro.Id > 0)
        {
            _unitOfWork.Repository<Registro>().UpdateEntity(registro);
        }
        else
        {
           // var registroNuevo = _mapper.Map<Registro>(request);
            _unitOfWork.Repository<Registro>().AddEntity(registro);
        }

        if (await _unitOfWork.Complete() <= 0)
            throw new Exception("No se pudo insertar/actualizar el Registro");
    }



    private async Task<Registro> GetOrCreateRegistro(CreateRegistroCommand request, RegistroActualizacion registroActualizacion)
    {
        if (registroActualizacion.IdReferencia > 0)
        {
            // Buscar el registro por IdReferencia
            var registro = await _unitOfWork.Repository<Registro>()
                .GetByIdWithSpec(new RegistroWithFilteredDataSpecification(
                    registroActualizacion.IdReferencia));

            if (registro is null || registro.Borrado)
                throw new BadRequestException($"El registro de actualización con Id [{registroActualizacion.Id}] no tiene registro");

            return registro;
        }

        return new Registro { IdSuceso = request.IdSuceso};
    }

    private async Task ValidateFechas(CreateRegistroCommand request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var incendioAsociado = await _unitOfWork.Repository<Incendio>()
            .GetByIdWithSpec(new IncendioActiveByIdSpecification(request.IdSuceso))
            ?? throw new BadRequestException($"No se encontró el incendio con ID {request.IdSuceso}.");

        var registroActualizacion = await _unitOfWork.Repository<RegistroActualizacion>()
            .GetFirstOrDefaultAsync(new RegistroActualizacionByIdSucesoSpecification(request.IdSuceso));

        // 🔥 Comparación directa ya que ambos son DateTimeOffset (UTC)
        /*
        if (request.FechaHoraEvolucion < incendioAsociado.FechaInicio)
            throw new BadRequestException("La fecha de evolución no puede ser menor a la fecha de inicio del incendio.");
        */
        if (request.FechaHoraEvolucion > DateTimeOffset.UtcNow)
            throw new BadRequestException("La fecha de evolución no puede ser mayor a la fecha del sistema. FechaHoraEvolucion " + request.FechaHoraEvolucion + " Fecha actual " + DateTimeOffset.UtcNow);
    }



    private async Task ComprobarRegistro(CreateRegistroCommand request)
    {
        if (request.IdMedio.HasValue)
        {
            var medio = await _unitOfWork.Repository<Medio>().GetByIdAsync(request.IdMedio.Value);
            if (medio is null)
            {
                _logger.LogWarning($"request.IdMedio: {request.IdMedio}, no encontrado");
                throw new NotFoundException(nameof(Medio), request.IdMedio);
            }
        }

        if (request.IdEntradaSalida.HasValue)
        {
            var entradaSalida = await _unitOfWork.Repository<EntradaSalida>().GetByIdAsync(request.IdEntradaSalida.Value);
            if (entradaSalida is null)
            {
                _logger.LogWarning($"request.IdEntradaSalida: {request.IdEntradaSalida}, no encontrado");
                throw new NotFoundException(nameof(EntradaSalida), request.IdEntradaSalida);
            }
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

}