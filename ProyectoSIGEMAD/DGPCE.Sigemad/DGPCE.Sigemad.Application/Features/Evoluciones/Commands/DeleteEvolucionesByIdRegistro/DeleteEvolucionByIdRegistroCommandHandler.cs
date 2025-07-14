using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Evoluciones;
using DGPCE.Sigemad.Application.Specifications.Registros;
using DGPCE.Sigemad.Application.Specifications.RegistrosActualizaciones;
using DGPCE.Sigemad.Domain.Common;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Evoluciones.Commands.DeleteEvolucionesByIdRegistro;
public class DeleteEvolucionByIdRegistroCommandHandler : IRequestHandler<DeleteEvolucionByIdRegistroCommand>
{
    private readonly ILogger<DeleteEvolucionByIdRegistroCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly List<int> _idsEstadosCreados = new()
    {
        (int)EstadoRegistroEnum.Creado,
        (int)EstadoRegistroEnum.CreadoYModificado
    };

    public DeleteEvolucionByIdRegistroCommandHandler(
        ILogger<DeleteEvolucionByIdRegistroCommandHandler> logger,
        IUnitOfWork unitOfWork
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteEvolucionByIdRegistroCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"BEGIN - {nameof(DeleteEvolucionByIdRegistroCommandHandler)}");

        RegistroActualizacion registro = await ObtenerRegistroActualizacion(request.IdRegistroActualizacion);
        Evolucion evolucion = await ObtenerEvolucion(registro.IdSuceso);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            EliminarEntidadesRelacionadas(evolucion, registro);
            await EliminarRegistroActualizacion(registro);

            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error en la transacción de CreateOrUpdateDireccionCommandHandler");
            throw;
        }

        _logger.LogInformation($"END - {nameof(DeleteEvolucionByIdRegistroCommandHandler)}");
        return Unit.Value;
    }

    private async Task<RegistroActualizacion> ObtenerRegistroActualizacion(int idRegistroActualizacion)
    {
        var spec = new RegistroActualizacionSpecification(new RegistroActualizacionSpecificationParams { Id = idRegistroActualizacion });
        var registro = await _unitOfWork.Repository<RegistroActualizacion>().GetByIdWithSpec(spec);

        if (registro is null || registro.Borrado || registro.IdTipoRegistroActualizacion != (int)TipoRegistroActualizacionEnum.Evolucion)
        {
            _logger.LogWarning($"RegistroActualizacion no encontrado o inválido | IdRegistroActualizacion: {idRegistroActualizacion}");
            throw new NotFoundException(nameof(RegistroActualizacion), idRegistroActualizacion);
        }

        return registro;
    }

    private async Task<Evolucion> ObtenerEvolucion(int idSuceso)
    {
        var spec = new EvolucionSpecification(new RegistroSpecificationParams { IdSuceso = idSuceso });
        var evolucion = await _unitOfWork.Repository<Evolucion>().GetByIdWithSpec(spec);

        if (evolucion is null || evolucion.Borrado)
        {
            _logger.LogWarning($"Evolución no encontrada o inválida | IdSuceso: {idSuceso}");
            throw new NotFoundException(nameof(Evolucion), idSuceso);
        }

        return evolucion;
    }

    private async Task EliminarRegistroActualizacion(RegistroActualizacion registro)
    {
        foreach (var detalle in registro.DetallesRegistro)
        {
            await _unitOfWork.Repository<DetalleRegistroActualizacion>().DeleteAsync(detalle);
        }
        await _unitOfWork.Repository<RegistroActualizacion>().DeleteAsync(registro);
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

    private void EliminarEntidadesRelacionadas(Evolucion evolucion, RegistroActualizacion registro)
    {
        //EliminarElementos(evolucion.IntervencionMedios, registro, ApartadoRegistroEnum.IntervencionMedios, true);
        //EliminarElementos(evolucion.Impactos, registro, ApartadoRegistroEnum.ConsecuenciaActuacion);
      //  EliminarElementos(evolucion.AreaAfectadas, registro, ApartadoRegistroEnum.AreaAfectada);
        //EliminarElementos(evolucion.Parametros, registro, ApartadoRegistroEnum.Parametro);

        //(EliminarElementos(evolucion.Registros, registro, ApartadoRegistroEnum.Registro);

        //EliminarElementos(evolucion.DatosPrincipales, registro, ApartadoRegistroEnum.DatoPrincipal);

        //if (DebeEliminar(registro, ApartadoRegistroEnum.DatoPrincipal, evolucion.DatoPrincipal?.Id))
        //    _unitOfWork.Repository<DatoPrincipal>().DeleteEntity(evolucion.DatoPrincipal);

        //if (DebeEliminar(registro, ApartadoRegistroEnum.Registro, evolucion.Registro?.Id))
        //    _unitOfWork.Repository<Registro>().DeleteEntity(evolucion.Registro);

        _unitOfWork.Repository<Evolucion>().UpdateEntity(evolucion);
    }
}
