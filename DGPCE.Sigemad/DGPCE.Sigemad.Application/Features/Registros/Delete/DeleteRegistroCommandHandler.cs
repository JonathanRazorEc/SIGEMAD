using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.Evoluciones.Commands.DeleteEvolucionesByIdRegistro;
using DGPCE.Sigemad.Application.Features.Evoluciones.Vms;
using DGPCE.Sigemad.Application.Specifications.Registros;
using DGPCE.Sigemad.Application.Specifications.RegistrosActualizaciones;
using DGPCE.Sigemad.Domain.Common;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Registros.DeleteRegistros;
public class DeleteRegistroCommandHandler : IRequestHandler<DeleteRegistroCommand>
{
    private readonly ILogger<DeleteRegistroCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly List<int> _idsEstadosCreados = new()
    {
        (int)EstadoRegistroEnum.Creado,
        (int)EstadoRegistroEnum.CreadoYModificado
    };

    public DeleteRegistroCommandHandler(
        ILogger<DeleteRegistroCommandHandler> logger,
        IUnitOfWork unitOfWork
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }


    public async Task<Unit> Handle(DeleteRegistroCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"BEGIN - {nameof(DeleteEvolucionByIdRegistroCommandHandler)}");

        RegistroActualizacion registroActualizacion = await ObtenerRegistroActualizacion(request.IdRegistroActualizacion);
        Registro registro = await ObtenerRegistro(registroActualizacion.IdReferencia);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            EliminarEntidadesRelacionadas(registro, registroActualizacion);
            await EliminarRegistroActualizacion(registroActualizacion);

            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error en la transacción de DeleteRegistroCommandHandler");
            throw;
        }

        _logger.LogInformation($"END - {nameof(DeleteRegistroCommandHandler)}");
        return Unit.Value;
    }

    private async Task EliminarRegistroActualizacion(RegistroActualizacion registro)
    {
        foreach (var detalle in registro.DetallesRegistro)
        {
            await _unitOfWork.Repository<DetalleRegistroActualizacion>().DeleteAsync(detalle);
        }
        await _unitOfWork.Repository<RegistroActualizacion>().DeleteAsync(registro);
    }


    private async Task<Registro> ObtenerRegistro(int idReferencia)
    {
        var registro = await _unitOfWork.Repository<Registro>()
             .GetByIdWithSpec(new RegistroWithFilteredDataSpecification(idReferencia));

        if (registro is null || registro.Borrado)
        {
            _logger.LogWarning($"Registro no encontrada o inválida | IdReferencia: {idReferencia}");
            throw new NotFoundException(nameof(Registro), idReferencia);
        }

        return registro;
    }

    private async Task<RegistroActualizacion> ObtenerRegistroActualizacion(int idRegistroActualizacion)
    {
        var spec = new RegistroActualizacionSpecification(new RegistroActualizacionSpecificationParams { Id = idRegistroActualizacion });
        var registro = await _unitOfWork.Repository<RegistroActualizacion>().GetByIdWithSpec(spec);

        if (registro is null || registro.Borrado || registro.IdTipoRegistroActualizacion != (int)TipoRegistroActualizacionEnum.Registro)
        {
            _logger.LogWarning($"RegistroActualizacion no encontrado o inválido | IdRegistroActualizacion: {idRegistroActualizacion}");
            throw new NotFoundException(nameof(RegistroActualizacion), idRegistroActualizacion);
        }

        return registro;
    }

    private bool DebeEliminar(RegistroActualizacion registro, ApartadoRegistroEnum apartado, int? idElemento)
    {
        return idElemento.HasValue && registro.DetallesRegistro
            .Any(dr => dr.IdApartadoRegistro == (int)apartado && _idsEstadosCreados.Contains((int)dr.IdEstadoRegistro) && dr.IdReferencia == idElemento.Value);
    }

    private void EliminarElementos<T>(List<T> lista, RegistroActualizacion registro, ApartadoRegistroEnum apartado, bool eliminarDetalles = false)
        where T : BaseDomainModel<int>
    {
        var idsAEliminar = registro.DetallesRegistro
            .Where(dr => dr.IdApartadoRegistro == (int)apartado && _idsEstadosCreados.Contains((int)dr.IdEstadoRegistro))
            .Select(dr => dr.IdReferencia)
            .ToList();

        var elementosEliminar = lista.Where(e => idsAEliminar.Contains(e.Id)).ToList();

        foreach (var elemento in elementosEliminar)
        {
            if (eliminarDetalles && elemento is IntervencionMedio intervencion)
            {
                foreach (var detalle in intervencion.DetalleIntervencionMedios)
                {
                    _unitOfWork.Repository<DetalleIntervencionMedio>().DeleteEntity(detalle);
                }
            }
            _unitOfWork.Repository<T>().DeleteEntity(elemento);
        }
    }

    private void EliminarEntidadesRelacionadas(Registro registro, RegistroActualizacion registroActualizacion)
    {
        EliminarElementos(registro.AreaAfectadas, registroActualizacion, ApartadoRegistroEnum.AreaAfectada);
        EliminarElementos(registro.Parametros, registroActualizacion, ApartadoRegistroEnum.Parametro);
        EliminarElementos(registro.TipoImpactosEvoluciones, registroActualizacion, ApartadoRegistroEnum.ConsecuenciaActuacion);
        EliminarElementos(registro.IntervencionMedios, registroActualizacion, ApartadoRegistroEnum.IntervencionMedios, true);
        EliminarElementos(registro.Direcciones, registroActualizacion, ApartadoRegistroEnum.Direccion);
        EliminarElementos(registro.CoordinacionesCecopi, registroActualizacion, ApartadoRegistroEnum.CoordinacionCECOPI);
        EliminarElementos(registro.CoordinacionesPMA, registroActualizacion, ApartadoRegistroEnum.CoordinacionPMA);
        EliminarElementos(registro.ActivacionPlanEmergencias, registroActualizacion, ApartadoRegistroEnum.ActivacionDePlanes);
        EliminarElementos(registro.ActivacionSistemas, registroActualizacion, ApartadoRegistroEnum.ActivacionDeSistemas);
        _unitOfWork.Repository<Registro>().DeleteEntity(registro);
    }
}
