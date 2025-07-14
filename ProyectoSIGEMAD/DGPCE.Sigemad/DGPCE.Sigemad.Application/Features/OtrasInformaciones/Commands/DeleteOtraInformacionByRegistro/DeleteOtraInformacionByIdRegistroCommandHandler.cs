using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.OtrasInformaciones;
using DGPCE.Sigemad.Application.Specifications.RegistrosActualizaciones;
using DGPCE.Sigemad.Domain.Common;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.OtrasInformaciones.Commands.DeleteOtraInformacionByRegistro;
public class DeleteOtraInformacionByIdRegistroCommandHandler : IRequestHandler<DeleteOtraInformacionByIdRegistroCommand>
{
    private readonly ILogger<DeleteOtraInformacionByIdRegistroCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly List<int> _idsEstadosCreados = new()
    {
        (int)EstadoRegistroEnum.Creado,
        (int)EstadoRegistroEnum.CreadoYModificado
    };

    public DeleteOtraInformacionByIdRegistroCommandHandler(
        ILogger<DeleteOtraInformacionByIdRegistroCommandHandler> logger,
        IUnitOfWork unitOfWork
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteOtraInformacionByIdRegistroCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"BEGIN - {nameof(DeleteOtraInformacionByIdRegistroCommandHandler)}");

        RegistroActualizacion registro = await ObtenerRegistroActualizacion(request.IdRegistroActualizacion);
        OtraInformacion otraInformacion = await ObtenerOtraInformacion(registro.IdSuceso);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            EliminarEntidadesRelacionadas(otraInformacion, registro);
            await EliminarRegistroActualizacion(registro);

            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error en la transacción de DeleteOtraInformacionByIdRegistroCommandHandler");
            throw;
        }

        _logger.LogInformation($"END - {nameof(DeleteOtraInformacionByIdRegistroCommandHandler)}");
        return Unit.Value;
    }

    private async Task<RegistroActualizacion> ObtenerRegistroActualizacion(int idRegistroActualizacion)
    {
        var spec = new RegistroActualizacionSpecification(new RegistroActualizacionSpecificationParams { Id = idRegistroActualizacion });
        var registro = await _unitOfWork.Repository<RegistroActualizacion>().GetByIdWithSpec(spec);

        if (registro is null || registro.Borrado || registro.IdTipoRegistroActualizacion != (int)TipoRegistroActualizacionEnum.OtraInformacion)
        {
            _logger.LogWarning($"RegistroActualizacion no encontrado o inválido | IdRegistroActualizacion: {idRegistroActualizacion}");
            throw new NotFoundException(nameof(RegistroActualizacion), idRegistroActualizacion);
        }
        return registro;
    }

    private async Task<OtraInformacion> ObtenerOtraInformacion(int idSuceso)
    {
        var spec = new GetOtraInformacionWithParams(new OtraInformacionParams { IdSuceso = idSuceso });
        var otraInformacion = await _unitOfWork.Repository<OtraInformacion>().GetByIdWithSpec(spec);

        if (otraInformacion is null || otraInformacion.Borrado)
        {
            _logger.LogWarning($"Otra información no encontrada o inválida | IdSuceso: {idSuceso}");
            throw new NotFoundException(nameof(OtraInformacion), idSuceso);
        }

        return otraInformacion;
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
            if (eliminarDetalles && elemento is DetalleOtraInformacion detalleOtraInformacion)
            {
                foreach (var detalleProcedenciaDestino in detalleOtraInformacion.ProcedenciasDestinos)
                {
                    _unitOfWork.Repository<DetalleOtraInformacion_ProcedenciaDestino>().DeleteEntity(detalleProcedenciaDestino);
                }
            }
            _unitOfWork.Repository<T>().DeleteEntity(elemento);
        }
    }

    private void EliminarEntidadesRelacionadas(OtraInformacion otraInformacion, RegistroActualizacion registro)
    {
        EliminarElementos(otraInformacion.DetallesOtraInformacion, registro, ApartadoRegistroEnum.OtraInformacion, true);

        _unitOfWork.Repository<OtraInformacion>().UpdateEntity(otraInformacion);
    }
}
