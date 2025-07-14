using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Files;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Contracts.RegistrosActualizacion;
using DGPCE.Sigemad.Application.Dtos.ActivacionesPlanes;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;
using DGPCE.Sigemad.Application.Specifications.Incendios;
using DGPCE.Sigemad.Application.Specifications.Registros;

namespace DGPCE.Sigemad.Application.Features.ActivacionesPlanesEmergencia.Commands.ManageActivacionPlanEmergencia;
public class ManageActivacionPlanEmergenciaCommandHandler : IRequestHandler<ManageActivacionPlanEmergenciaCommand, ManageActivacionPlanEmergenciaResponse>
{
	private readonly ILogger<ManageActivacionPlanEmergenciaCommandHandler> _logger;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
	private readonly IFileService _fileService;
	private const string ARCHIVOS_PATH = "activacion-plan-emergencia";
	private readonly IRegistroActualizacionService _registroActualizacionService;
	private int? ambito;

	public ManageActivacionPlanEmergenciaCommandHandler(
		ILogger<ManageActivacionPlanEmergenciaCommandHandler> logger,
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

	public async Task<ManageActivacionPlanEmergenciaResponse> Handle(ManageActivacionPlanEmergenciaCommand request, CancellationToken cancellationToken)
	{
		_logger.LogInformation($"{nameof(ManageActivacionPlanEmergenciaCommandHandler)} - BEGIN");

		await ValidateTipoPlanes(request);
        ValidarTipoPlanDuplicados(request);
        await ValidatePlanesEmergencias(request);
		await ValidateFechas(request);

		await _unitOfWork.BeginTransactionAsync();

		try
		{
            RegistroActualizacion registroActualizacion = await _registroActualizacionService.GetOrCreateRegistroActualizacion<Registro>(
                request.IdRegistroActualizacion, request.IdSuceso, TipoRegistroActualizacionEnum.Registro);

            var registro = await GetRegistro(request, registroActualizacion);

            var activacionPlanesOriginales = registro.ActivacionPlanEmergencias.ToDictionary(d => d.Id, d => _mapper.Map<ManageActivacionPlanEmergenciaDto>(d));
			MapAndSaveDetallesActivacionPlanEmergencia(request, registro);

			var activacionPlanesParaEliminar = await DeleteLogicalActivacionesPlanes(request, registro, registroActualizacion.Id);

            await SaveRegistro(registro);

            await _registroActualizacionService.SaveRegistroActualizacion<
				Registro, ActivacionPlanEmergencia, ManageActivacionPlanEmergenciaDto>(
				registroActualizacion,
                registro,
				ApartadoRegistroEnum.ActivacionDePlanes,
				activacionPlanesParaEliminar, activacionPlanesOriginales);

			await _unitOfWork.CommitAsync();

			_logger.LogInformation($"{nameof(ManageActivacionPlanEmergenciaCommandHandler)} - END");

			return new ManageActivacionPlanEmergenciaResponse
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


    public void ValidarTipoPlanDuplicados(ManageActivacionPlanEmergenciaCommand comando)
    {
        var duplicados = comando.ActivacionesPlanes
        .GroupBy(p => new { p.IdPlanEmergencia})
        .Where(g => g.Count() > 1)
        .ToList();

        if (duplicados.Any())
        {
            throw new BadRequestException("No se permite el mismo plan en mas de un registro");
        }
    }


    private async Task ValidateFechas(ManageActivacionPlanEmergenciaCommand request)
	{
		if (request == null)
			throw new ArgumentNullException(nameof(request));

		if (request.ActivacionesPlanes == null)
			throw new BadRequestException("Los detalles de la convocatoria no pueden estar vacios.");

		var incendioAsociado = await _unitOfWork.Repository<Incendio>()
		   .GetByIdWithSpec(new IncendioActiveByIdSpecification(request.IdSuceso))
		   ?? throw new BadRequestException($"No se encontró el incendio con ID {request.IdSuceso}.");

		bool fechasValidas = request.ActivacionesPlanes.All(AP => AP.FechaHoraInicio >= incendioAsociado.FechaInicio.UtcDateTime);


        var fechasInvalidas = request.ActivacionesPlanes
         .Where(AP => AP.FechaHoraInicio < incendioAsociado.FechaInicio.UtcDateTime)
         .Select(AP => AP.FechaHoraInicio.ToString("yyyy-MM-dd HH:mm"))
         .ToList();


        bool fechasValidasInicioFin = request.ActivacionesPlanes
		    .Where(AP => AP.FechaHoraFin.HasValue)
			.All(AP => AP.FechaHoraFin >= AP.FechaHoraInicio);

        bool fechasValidasActual = request.ActivacionesPlanes.All(AP => AP.FechaHoraInicio <= DateTimeOffset.UtcNow);

        if (!fechasValidas)
		{
            throw new BadRequestException(
            $"Una o más fechas de activación son anteriores a la fecha del incendio asociado ({incendioAsociado.FechaInicio.UtcDateTime:yyyy-MM-dd HH:mm}). " +
            $"Fechas inválidas: {string.Join(", ", fechasInvalidas)}"
            );

        }

        if (!fechasValidasInicioFin )
		{
			throw new BadRequestException("Una o mas fechas final es menor a la fecha inicio.");
		}

        if (!fechasValidasActual)
        {
            throw new BadRequestException("Una o mas fechas es mayor a la fecha actual del sistema.");
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


    private async Task<List<int>> DeleteLogicalActivacionesPlanes(ManageActivacionPlanEmergenciaCommand request, Registro registro, int idRegistroActualizacion)
	{
		if (registro.Id > 0)
		{
			var idsEnRequest = request.ActivacionesPlanes.Where(d => d.Id.HasValue && d.Id > 0).Select(d => d.Id).ToList();
			var activacionDePlanesParaEliminar = registro.ActivacionPlanEmergencias
				.Where(d => d.Id > 0 && !idsEnRequest.Contains(d.Id))
				.ToList();

			if (activacionDePlanesParaEliminar.Count == 0)
			{
				return new List<int>();
			}

			// Obtener el historial de creación de estas activacionDePlanes
			var idsActivacionDePlanesParaEliminarParaEliminar = activacionDePlanesParaEliminar.Select(d => d.Id).ToList();
			var historialDirecciones = await _unitOfWork.Repository<DetalleRegistroActualizacion>()
				.GetAsync(d =>
				idsActivacionDePlanesParaEliminarParaEliminar.Contains(d.IdReferencia) && d.IdApartadoRegistro == (int)ApartadoRegistroEnum.ActivacionDePlanes);

			foreach (var detalle in activacionDePlanesParaEliminar)
			{
				var historial = historialDirecciones.FirstOrDefault(d =>
				d.IdReferencia == detalle.Id &&
				(d.IdEstadoRegistro == EstadoRegistroEnum.Creado || d.IdEstadoRegistro == EstadoRegistroEnum.CreadoYModificado));

				if (historial == null || historial.IdRegistroActualizacion != idRegistroActualizacion)
				{
					throw new BadRequestException($"Las activacion de plan de emergencia con ID {detalle.Id} solo puede eliminarse en el registro en que fue creada.");
				}

				if (detalle.IdArchivo.HasValue)
				{
					_unitOfWork.Repository<Archivo>().DeleteEntity(detalle.Archivo);
				}

				_unitOfWork.Repository<ActivacionPlanEmergencia>().DeleteEntity(detalle);
			}

			return idsActivacionDePlanesParaEliminarParaEliminar;
		}

		return new List<int>();
	}

    private async void MapAndSaveDetallesActivacionPlanEmergencia(ManageActivacionPlanEmergenciaCommand request, Registro registro)
	{
		foreach (var detalleActivacionPlanesDto in request.ActivacionesPlanes)
		{
			if (detalleActivacionPlanesDto.Id.HasValue && detalleActivacionPlanesDto.Id > 0)
			{
				var activacionPlanesExistente = registro.ActivacionPlanEmergencias.FirstOrDefault(d => d.Id == detalleActivacionPlanesDto.Id.Value);
				if (activacionPlanesExistente != null)
				{
					var copiaOriginal = _mapper.Map<ManageActivacionPlanEmergenciaDto>(activacionPlanesExistente);
					var copiaNueva = _mapper.Map<ManageActivacionPlanEmergenciaDto>(detalleActivacionPlanesDto);

					if (!copiaOriginal.Equals(copiaNueva))
					{
						_mapper.Map(detalleActivacionPlanesDto, activacionPlanesExistente);
						activacionPlanesExistente.Borrado = false;
                        activacionPlanesExistente.Archivo = await _fileService.MapArchivo(detalleActivacionPlanesDto.Archivo, activacionPlanesExistente.Archivo, ARCHIVOS_PATH, detalleActivacionPlanesDto.ActualizarFichero);
					}
				}
				else
				{
                    registro.ActivacionPlanEmergencias.Add(await CreateDetalleActivacionPlanesEmergencia(detalleActivacionPlanesDto));
				}
			}
			else
			{
                registro.ActivacionPlanEmergencias.Add(await CreateDetalleActivacionPlanesEmergencia(detalleActivacionPlanesDto));
			}
		}
	}

	private async Task<ActivacionPlanEmergencia> CreateDetalleActivacionPlanesEmergencia(ManageActivacionPlanEmergenciaDto detalleActivacionPlanesDto)
	{
		var nuevoDetalleActivacionPlanEmergencia = new ActivacionPlanEmergencia
		{
			IdTipoPlan = detalleActivacionPlanesDto.IdTipoPlan,
			TipoPlanPersonalizado = detalleActivacionPlanesDto.TipoPlanPersonalizado,
			IdPlanEmergencia = detalleActivacionPlanesDto.IdPlanEmergencia,
			PlanEmergenciaPersonalizado = detalleActivacionPlanesDto.PlanEmergenciaPersonalizado,
			FechaHoraInicio = detalleActivacionPlanesDto.FechaHoraInicio,
			FechaHoraFin = detalleActivacionPlanesDto.FechaHoraFin,
			Autoridad = detalleActivacionPlanesDto.Autoridad,
			Observaciones = detalleActivacionPlanesDto.Observaciones
		};

		nuevoDetalleActivacionPlanEmergencia.Archivo = await _fileService.MapArchivo(detalleActivacionPlanesDto.Archivo, null, ARCHIVOS_PATH, detalleActivacionPlanesDto.ActualizarFichero);

		return nuevoDetalleActivacionPlanEmergencia;
	}


    private async Task<Registro> GetRegistro(ManageActivacionPlanEmergenciaCommand request, RegistroActualizacion registroActualizacion)
    {
        if (registroActualizacion.IdReferencia > 0)
        {
            List<int> idsActivacionPlanEmergencias = new List<int>();

            foreach (var detalle in registroActualizacion.DetallesRegistro)
            {
                if (detalle.IdApartadoRegistro == (int)ApartadoRegistroEnum.ActivacionDePlanes)
                {
                    idsActivacionPlanEmergencias.Add(detalle.IdReferencia);
                }
            }

            // Buscar el registro por IdReferencia
            var registro = await _unitOfWork.Repository<Registro>()
                .GetByIdWithSpec(new RegistroWithFilteredDataSpecification(
                    registroActualizacion.IdReferencia, idsActivacionPlanEmergencias: idsActivacionPlanEmergencias, ambito:ambito));

            if (registro is null || registro.Borrado)
                throw new BadRequestException($"El registro de actualización con Id [{registroActualizacion.Id}] no tiene registro");

            return registro;
        }

        throw new BadRequestException($"No se ha proporcionado un IdregistroActualizacion válido");
    }




	private async Task ValidateTipoPlanes(ManageActivacionPlanEmergenciaCommand request)
	{
		var idsTipoPlan = request.ActivacionesPlanes.Select(c => c.IdTipoPlan).Distinct();
		var tipoPlanesExistentes = await _unitOfWork.Repository<TipoPlan>().GetAsync(p => idsTipoPlan.Contains(p.Id));

       
        if (tipoPlanesExistentes.Count() != idsTipoPlan.Count())
		{
			var idsTipoPlanesExistentes = tipoPlanesExistentes.Select(p => p.Id).Cast<int?>().ToList();
			var idsTipoPlanesInvalidas = idsTipoPlan.Except(idsTipoPlanesExistentes).ToList();

			if (idsTipoPlanesInvalidas.Any())
			{
				_logger.LogWarning($"Los siguientes Id's de Tipo Plan: {string.Join(", ", idsTipoPlanesInvalidas)}, no se encontraron");
				throw new NotFoundException(nameof(TipoPlan), string.Join(", ", idsTipoPlanesInvalidas));
			}
        }
    }

	private async Task ValidatePlanesEmergencias(ManageActivacionPlanEmergenciaCommand request)
	{
		var idsPlanesEmergencias = request.ActivacionesPlanes.Select(c => c.IdPlanEmergencia).Distinct();
		var planesEmergenciasExistentes = await _unitOfWork.Repository<PlanEmergencia>().GetAsync(p => idsPlanesEmergencias.Contains(p.Id));

		if (planesEmergenciasExistentes.Count() != idsPlanesEmergencias.Count())
		{
			var idsPlanesEmergenciasExistentes = planesEmergenciasExistentes.Select(p => p.Id).Cast<int?>().ToList();
			var idsPlanesEmergenciasInvalidas = idsPlanesEmergencias.Except(idsPlanesEmergenciasExistentes).ToList();

			if (idsPlanesEmergenciasInvalidas.Any())
			{
				_logger.LogWarning($"Los siguientes Id's de Planes Emergencias: {string.Join(", ", idsPlanesEmergenciasInvalidas)}, no se encontraron");
				throw new NotFoundException(nameof(PlanEmergencia), string.Join(", ", idsPlanesEmergenciasInvalidas));
			}
		}


        var ambitos = planesEmergenciasExistentes.Select(p => p.IdAmbitoPlan).Distinct().ToList();
        ambito = ambitos.Count == 1 ? ambitos.First() : null;
    }


}
