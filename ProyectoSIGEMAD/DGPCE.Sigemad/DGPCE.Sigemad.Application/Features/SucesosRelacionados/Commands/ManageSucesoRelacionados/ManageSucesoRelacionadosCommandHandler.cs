using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Contracts.RegistrosActualizacion;
using DGPCE.Sigemad.Application.Dtos.SucesoRelacionados;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Commands.Manage;
using DGPCE.Sigemad.Application.Specifications.RegistrosActualizaciones;
using DGPCE.Sigemad.Application.Specifications.SucesosRelacionados;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.SucesosRelacionados.Commands.ManageSucesoRelacionados;
public class ManageSucesoRelacionadosCommandHandler : IRequestHandler<ManageSucesoRelacionadosCommand, ManageSucesoRelacionadoResponse>
{
    private readonly ILogger<ManageSucesoRelacionadosCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRegistroActualizacionService _registroActualizacionService;

    public ManageSucesoRelacionadosCommandHandler(
        ILogger<ManageSucesoRelacionadosCommandHandler> logger,
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

    public async Task<ManageSucesoRelacionadoResponse> Handle(ManageSucesoRelacionadosCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(ManageSucesoRelacionadosCommandHandler)} - BEGIN");

        await _registroActualizacionService.ValidarSuceso(request.IdSuceso);
        await ValidateProcedenciasDestinosAsync(request);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            RegistroActualizacion registroActualizacion = await _registroActualizacionService.GetOrCreateRegistroActualizacion<SucesoRelacionado>(
                request.IdRegistroActualizacion, request.IdSuceso, TipoRegistroActualizacionEnum.SucesosRelacionados);

            SucesoRelacionado sucesoRelacionado = await GetOrCreateSucesoRelacionado(request, registroActualizacion);

            (List<int> idsNuevos, List<int> idsEliminar, List<int> idsPermanentes) =
                await MapAndSaveAndDeleteSucesosRelacionados(request, sucesoRelacionado, registroActualizacion.Id);

            await SaveSucesoRelacionado(sucesoRelacionado);

            await MapAndSaveRegistroActualizacion(registroActualizacion, sucesoRelacionado, request, idsNuevos, idsEliminar, idsPermanentes);

            await _unitOfWork.CommitAsync();

            _logger.LogInformation($"{nameof(CreateOrUpdateDireccionCoordinacionCommandHandler)} - END");

            return new ManageSucesoRelacionadoResponse
            {
                IdSucesoRelacionado = sucesoRelacionado.Id,
                IdRegistroActualizacion = registroActualizacion.Id
            };

        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error en la transacción de ManageSucesoRelacionadosCommandHandler");
            throw;
        }


    }

    private async Task MapAndSaveRegistroActualizacion(RegistroActualizacion registroActualizacion, SucesoRelacionado sucesoRelacionado, ManageSucesoRelacionadosCommand originalSucesoRelacionado,
        List<int> idsNuevos, List<int> idsEliminar, List<int> idsPermanentes)
    {
        registroActualizacion.IdReferencia = sucesoRelacionado.Id;
        string ambito = "Sucesos Relacionados";


        foreach (int idNuevo in idsNuevos)
        {
            var detalle = sucesoRelacionado.DetalleSucesoRelacionados.FirstOrDefault(s => s.IdSucesoAsociado == idNuevo);
            string denominacion = await GetDescripcionTipoSuceso(detalle.SucesoAsociado.IdTipo, detalle.SucesoAsociado.TipoSuceso);

            DetalleRegistroActualizacion detalleRegistro = new()
            {
                IdApartadoRegistro = (int)ApartadoRegistroEnum.SucesosRelacionados,
                IdReferencia = idNuevo,
                IdEstadoRegistro = EstadoRegistroEnum.Creado,
                Ambito = ambito,
                Descripcion = $"Denominación [{denominacion}]"
            };
            registroActualizacion.DetallesRegistro.Add(detalleRegistro);
        }

        foreach (int idEliminar in idsEliminar)
        {
            var registroExistente = registroActualizacion.DetallesRegistro
                        .FirstOrDefault(d => d.IdReferencia == idEliminar && d.IdApartadoRegistro == (int)ApartadoRegistroEnum.SucesosRelacionados);

            var detalle = sucesoRelacionado.DetalleSucesoRelacionados.FirstOrDefault(s => s.IdSucesoAsociado == idEliminar);

            if (registroExistente != null)
            {
                EstadoRegistroEnum estadoRegistro = (registroExistente.IdEstadoRegistro == EstadoRegistroEnum.CreadoYModificado ||
                    registroExistente.IdEstadoRegistro == EstadoRegistroEnum.Creado) ? EstadoRegistroEnum.CreadoYEliminado : EstadoRegistroEnum.Eliminado;
                registroExistente.IdEstadoRegistro = estadoRegistro;
            }
            else
            {
                string denominacion = await GetDescripcionTipoSuceso(detalle.SucesoAsociado.IdTipo, detalle.SucesoAsociado.TipoSuceso);

                DetalleRegistroActualizacion detalleRegistro = new()
                {
                    IdApartadoRegistro = (int)ApartadoRegistroEnum.SucesosRelacionados,
                    IdReferencia = idEliminar,
                    IdEstadoRegistro = EstadoRegistroEnum.Eliminado,
                    Ambito = ambito,
                    Descripcion = $"Denominación [{denominacion}]"
                };
                registroActualizacion.DetallesRegistro.Add(detalleRegistro);
            }
        }

        foreach (int idPermanente in idsPermanentes)
        {
            var registroExistente = registroActualizacion.DetallesRegistro
                        .FirstOrDefault(d => d.IdReferencia == idPermanente && d.IdApartadoRegistro == (int)ApartadoRegistroEnum.SucesosRelacionados);

            var detalle = sucesoRelacionado.DetalleSucesoRelacionados.FirstOrDefault(s => s.IdSucesoAsociado == idPermanente);

            if (registroExistente == null)
            {
                string denominacion = await GetDescripcionTipoSuceso(detalle.SucesoAsociado.IdTipo, detalle.SucesoAsociado.TipoSuceso);

                DetalleRegistroActualizacion detalleRegistro = new()
                {
                    IdApartadoRegistro = (int)ApartadoRegistroEnum.SucesosRelacionados,
                    IdReferencia = idPermanente,
                    IdEstadoRegistro = EstadoRegistroEnum.Permanente,
                    Ambito = ambito,
                    Descripcion = $"Denominación [{denominacion}]"
                };
                registroActualizacion.DetallesRegistro.Add(detalleRegistro);
            }
        }


        if (registroActualizacion.Id > 0)
        {
            await ReflectNewRegistrosInFuture(
                registroActualizacion,
                ApartadoRegistroEnum.SucesosRelacionados,
                sucesoRelacionado,
                idsNuevos);

            _unitOfWork.Repository<RegistroActualizacion>().UpdateEntity(registroActualizacion);
        }
        else
        {
            _unitOfWork.Repository<RegistroActualizacion>().AddEntity(registroActualizacion);
        }

        if (await _unitOfWork.Complete() <= 0)
            throw new Exception("No se pudo insertar/actualizar registros de actualizaciones");
    }

    private async Task ReflectNewRegistrosInFuture(
        RegistroActualizacion registroActualizacion,
        ApartadoRegistroEnum apartadoRegistro,
        SucesoRelacionado sucesoRelacionado,
        List<int> nuevasReferenciasIds)
    {
        if (!nuevasReferenciasIds.Any()) return;

        var spec = new RegistroActualizacionSpecification(new RegistroActualizacionSpecificationParams
        {
            IdMinimo = registroActualizacion.Id,
            IdSuceso = registroActualizacion.IdSuceso,
            IdTipoRegistroActualizacion = registroActualizacion.IdTipoRegistroActualizacion
        });
        var registrosPosteriores = await _unitOfWork.Repository<RegistroActualizacion>().GetAllWithSpec(spec);
        string ambito = "Sucesos Relacionados";

        foreach (var registroPosterior in registrosPosteriores)
        {
            bool seActualizoRegistroPosterior = false;
            var detallesPrevios = registroPosterior.DetallesRegistro;

            foreach (var idReferencia in nuevasReferenciasIds)
            {
                if (detallesPrevios.Any(d => d.IdReferencia == idReferencia)) continue;

                var detalle = sucesoRelacionado.DetalleSucesoRelacionados.FirstOrDefault(s => s.IdSucesoAsociado == idReferencia);
                string denominacion = await GetDescripcionTipoSuceso(detalle.SucesoAsociado.IdTipo, detalle.SucesoAsociado.TipoSuceso);

                var nuevoDetalle = new DetalleRegistroActualizacion
                {
                    IdRegistroActualizacion = registroPosterior.Id,
                    IdApartadoRegistro = (int)apartadoRegistro,
                    IdReferencia = idReferencia,
                    IdEstadoRegistro = EstadoRegistroEnum.CreadoEnRegistroAnterior,
                    Ambito = ambito,
                    Descripcion = $"Denominación [{denominacion}]"
                };

                registroPosterior.DetallesRegistro.Add(nuevoDetalle);
                seActualizoRegistroPosterior = true;
            }

            if (seActualizoRegistroPosterior)
                _unitOfWork.Repository<RegistroActualizacion>().UpdateEntity(registroPosterior);
        }
    }


    private async Task SaveSucesoRelacionado(SucesoRelacionado sucesoRelacionado)
    {
        if (sucesoRelacionado.Id > 0)
        {
            _unitOfWork.Repository<SucesoRelacionado>().UpdateEntity(sucesoRelacionado);
        }
        else
        {
            _unitOfWork.Repository<SucesoRelacionado>().AddEntity(sucesoRelacionado);
        }

        if (await _unitOfWork.Complete() <= 0)
            throw new Exception("No se pudo insertar/actualizar el Suceso Relacionado");
    }

    private async Task<(List<int> idsNuvos, List<int> idsAEliminar, List<int> idsPermanentes)> MapAndSaveAndDeleteSucesosRelacionados(
        ManageSucesoRelacionadosCommand request,
        SucesoRelacionado sucesoRelacionado,
        int idRegistroActualizacion
    )
    {
        // Manejo de DetalleSucesoRelacionado
        var idsExistentes = sucesoRelacionado.DetalleSucesoRelacionados
            .Select(d => d.IdSucesoAsociado)
            .ToList();

        // IDs recibidos en la request
        var idsRequest = request.IdsSucesosAsociados.ToList();

        // 1️⃣ IDs que deben agregarse (están en request pero no en la BD)
        var idsNuevos = idsRequest.Except(idsExistentes).ToList();

        // 2️⃣ IDs que deben eliminarse (están en la BD pero no en request)
        var idsAEliminar = idsExistentes.Except(idsRequest).ToList();

        // 3️⃣ IDs que permanecerán (ya existen y siguen estando en request)
        var idsPermanentes = idsExistentes.Intersect(idsRequest).ToList();


        if (sucesoRelacionado.Id > 0)
        {
            // *************************************************************************
            // Eliminar los que no se enviaron en el listado

            var historialSucesoRelacionados = await _unitOfWork.Repository<DetalleRegistroActualizacion>()
                .GetAsync(d =>
                idsAEliminar.Contains(d.IdReferencia) && d.IdApartadoRegistro == (int)ApartadoRegistroEnum.SucesosRelacionados);

            foreach (var idEliminar in idsAEliminar)
            {
                var detalle = sucesoRelacionado.DetalleSucesoRelacionados
                    .FirstOrDefault(d => d.IdSucesoAsociado == idEliminar);

                if (detalle == null) continue;


                var historial = historialSucesoRelacionados.FirstOrDefault(d =>
                    d.IdReferencia == idEliminar &&
                    (d.IdEstadoRegistro == EstadoRegistroEnum.Creado || d.IdEstadoRegistro == EstadoRegistroEnum.CreadoYModificado));

                if (historial == null || historial.IdRegistroActualizacion != idRegistroActualizacion)
                {
                    throw new BadRequestException($"Suceso asociado con ID {idEliminar} solo puede eliminarse en el registro en que fue creada.");
                }

                sucesoRelacionado.DetalleSucesoRelacionados.Remove(detalle);
            }
        }


        // *************************************************************************
        // Agregar nuevos sucesos asociados
        foreach (var idNuevo in idsNuevos)
        {
            sucesoRelacionado.DetalleSucesoRelacionados.Add(new DetalleSucesoRelacionado
            {
                IdSucesoAsociado = idNuevo
            });
        }


        return (idsNuevos, idsAEliminar, idsPermanentes);
    }



    private async Task<SucesoRelacionado> GetOrCreateSucesoRelacionado(ManageSucesoRelacionadosCommand request, RegistroActualizacion registroActualizacion)
    {
        if (registroActualizacion.IdReferencia > 0)
        {
            List<int> idsReferencias = new List<int>();
            List<int> idsSucesosRelacioneados = new List<int>();

            // Separar IdReferencia según su tipo
            foreach (var detalle in registroActualizacion.DetallesRegistro)
            {
                switch (detalle.IdApartadoRegistro)
                {
                    case (int)ApartadoRegistroEnum.SucesosRelacionados:
                        idsSucesosRelacioneados.Add(detalle.IdReferencia);
                        break;
                    default:
                        idsReferencias.Add(detalle.IdReferencia);
                        break;
                }
            }

            // Buscar el suceso relacionado por IdReferencia
            var sucesoRelacionado = await _unitOfWork.Repository<SucesoRelacionado>()
                .GetByIdWithSpec(new SucesosRelacionadosActiveByIdPrincipalSpecification(registroActualizacion.IdReferencia, idsSucesosRelacioneados));

            if (sucesoRelacionado is null || sucesoRelacionado.Borrado)
                throw new BadRequestException($"El registro de actualización con Id [{registroActualizacion.Id}] no tiene registro de Otra Informacion");

            return sucesoRelacionado;
        }

        // Validar si ya existe un registro de Dirección y Coordinación de Emergencia para este suceso
        var specSuceso = new SucesosRelacionadosWithDetails(new SucesoRelacionadoParams { IdSucesoPrincipal = request.IdSuceso });
        var sucesoRelacionadoExistente = await _unitOfWork.Repository<SucesoRelacionado>().GetByIdWithSpec(specSuceso);

        return sucesoRelacionadoExistente ?? new SucesoRelacionado { IdSucesoPrincipal = request.IdSuceso };
    }

    private async Task ValidateProcedenciasDestinosAsync(ManageSucesoRelacionadosCommand request)
    {
        if (request.IdsSucesosAsociados != null && request.IdsSucesosAsociados.Count > 0)
        {
            var idsSucesosAsociados = request.IdsSucesosAsociados
                .Distinct();

            var sucesosAsociadosExistentes = await _unitOfWork.Repository<Suceso>().GetAsync(ic => idsSucesosAsociados.Contains(ic.Id));

            if (sucesosAsociadosExistentes.Count() != idsSucesosAsociados.Count())
            {
                var idsSucesosAsiciadosExistentes = sucesosAsociadosExistentes.Select(ic => ic.Id).ToList();
                var idsSucesosAsociadosInvalidos = idsSucesosAsociados.Except(idsSucesosAsiciadosExistentes).ToList();

                if (idsSucesosAsociadosInvalidos.Any())
                {
                    _logger.LogWarning($"Los siguientes Id's de sucesos asociados: {string.Join(", ", idsSucesosAsociadosInvalidos)}, no se encontraron");
                    throw new NotFoundException(nameof(Suceso), string.Join(", ", idsSucesosAsociadosInvalidos));
                }
            }
        }
    }

    private async Task<string> GetDescripcionTipoSuceso(int idTipoSuceso, TipoSuceso tipoSuceso)
    {
        if (tipoSuceso != null)
        {
            return tipoSuceso.Descripcion;
        }


        var tipoSucesoEntity = await _unitOfWork.Repository<TipoSuceso>().GetByIdAsync(idTipoSuceso);
        return tipoSucesoEntity.Descripcion;
    }



}
