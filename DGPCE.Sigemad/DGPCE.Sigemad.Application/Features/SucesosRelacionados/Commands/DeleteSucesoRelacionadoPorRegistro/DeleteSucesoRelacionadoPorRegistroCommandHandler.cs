using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.RegistrosActualizaciones;
using DGPCE.Sigemad.Application.Specifications.SucesosRelacionados;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.SucesosRelacionados.Commands.DeleteSucesoRelacionadoPorRegistro;
public class DeleteSucesoRelacionadoPorRegistroCommandHandler : IRequestHandler<DeleteSucesoRelacionadoPorRegistroCommand>
{
    private readonly ILogger<DeleteSucesoRelacionadoPorRegistroCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly List<int> _idsEstadosCreados = new()
    {
        (int)EstadoRegistroEnum.Creado,
        (int)EstadoRegistroEnum.CreadoYModificado
    };

    public DeleteSucesoRelacionadoPorRegistroCommandHandler(
        ILogger<DeleteSucesoRelacionadoPorRegistroCommandHandler> logger,
        IUnitOfWork unitOfWork
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteSucesoRelacionadoPorRegistroCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"BEGIN - {nameof(DeleteSucesoRelacionadoPorRegistroCommandHandler)}");

        RegistroActualizacion registro = await ObtenerRegistroActualizacion(request.IdRegistroActualizacion);
        SucesoRelacionado sucesoRelacionado = await ObtenerSucesoRelacionado(registro.IdSuceso);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            EliminarEntidadesRelacionadas(sucesoRelacionado, registro);
            await EliminarRegistroActualizacion(registro);

            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error en la transacción de DeleteSucesoRelacionadoPorRegistroCommandHandler");
            throw;
        }

        _logger.LogInformation($"END - {nameof(DeleteSucesoRelacionadoPorRegistroCommandHandler)}");
        return Unit.Value;
    }

    private async Task<RegistroActualizacion> ObtenerRegistroActualizacion(int idRegistroActualizacion)
    {
        var spec = new RegistroActualizacionSpecification(new RegistroActualizacionSpecificationParams { Id = idRegistroActualizacion });
        var registro = await _unitOfWork.Repository<RegistroActualizacion>().GetByIdWithSpec(spec);

        if (registro is null || registro.Borrado || registro.IdTipoRegistroActualizacion != (int)TipoRegistroActualizacionEnum.SucesosRelacionados)
        {
            _logger.LogWarning($"RegistroActualizacion no encontrado o inválido | IdRegistroActualizacion: {idRegistroActualizacion}");
            throw new NotFoundException(nameof(RegistroActualizacion), idRegistroActualizacion);
        }
        return registro;
    }

    private async Task<SucesoRelacionado> ObtenerSucesoRelacionado(int idSuceso)
    {
        var spec = new SucesosRelacionadosWithDetails(new SucesoRelacionadoParams { IdSucesoPrincipal = idSuceso });
        var sucesoRelacionado = await _unitOfWork.Repository<SucesoRelacionado>().GetByIdWithSpec(spec);

        if (sucesoRelacionado is null || sucesoRelacionado.Borrado)
        {
            _logger.LogWarning($"SucesoRelacionado no encontrada o inválida | IdSuceso: {idSuceso}");
            throw new NotFoundException(nameof(SucesoRelacionado), idSuceso);
        }

        return sucesoRelacionado;
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

    private void EliminarElementos(List<SucesoRelacionado> lista, RegistroActualizacion registro, ApartadoRegistroEnum apartado, bool eliminarDetalles = false)
    {
        var idsAEliminar = registro.DetallesRegistro
            .Where(dr => dr.IdApartadoRegistro == (int)apartado && _idsEstadosCreados.Contains((int)dr.IdEstadoRegistro))
            .Select(dr => dr.IdReferencia)
            .ToList();

        var elementosEliminar = lista.Where(e => idsAEliminar.Contains(e.Id)).ToList();

        foreach (var elemento in elementosEliminar)
        {
            if (eliminarDetalles && elemento is SucesoRelacionado sucesoRelacionado)
            {
                foreach (var detalleSuceso in sucesoRelacionado.DetalleSucesoRelacionados)
                {
                    _unitOfWork.Repository<DetalleSucesoRelacionado>().DeleteEntity(detalleSuceso);
                }
            }
            _unitOfWork.Repository<SucesoRelacionado>().DeleteEntity(elemento);
        }
    }

    private void EliminarEntidadesRelacionadas(SucesoRelacionado sucesoRelacionado, RegistroActualizacion registro)
    {
        var idsAEliminar = registro.DetallesRegistro
            .Where(dr => dr.IdApartadoRegistro == (int)ApartadoRegistroEnum.SucesosRelacionados && _idsEstadosCreados.Contains((int)dr.IdEstadoRegistro))
            .Select(dr => dr.IdReferencia)
            .ToList();

        var elementosEliminar = sucesoRelacionado.DetalleSucesoRelacionados.Where(e => idsAEliminar.Contains(e.IdSucesoAsociado)).ToList();

        foreach (DetalleSucesoRelacionado detalleSuceso in elementosEliminar)
        {
            _unitOfWork.Repository<DetalleSucesoRelacionado>().DeleteEntity(detalleSuceso);
        }
    }
}
