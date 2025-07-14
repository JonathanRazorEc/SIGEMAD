using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Documentos;
using DGPCE.Sigemad.Application.Specifications.RegistrosActualizaciones;
using DGPCE.Sigemad.Domain.Common;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Documentaciones.Commands.DeleteDocumentacionByRegistro;
public class DeleteDocumentacionByIdRegistroCommandHandler : IRequestHandler<DeleteDocumentacionByIdRegistroCommand>
{
    private readonly ILogger<DeleteDocumentacionByIdRegistroCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly List<int> _idsEstadosCreados = new()
    {
        (int)EstadoRegistroEnum.Creado,
        (int)EstadoRegistroEnum.CreadoYModificado
    };

    public DeleteDocumentacionByIdRegistroCommandHandler(
        ILogger<DeleteDocumentacionByIdRegistroCommandHandler> logger,
        IUnitOfWork unitOfWork
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteDocumentacionByIdRegistroCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"BEGIN - {nameof(DeleteDocumentacionByIdRegistroCommandHandler)}");

        RegistroActualizacion registro = await ObtenerRegistroActualizacion(request.IdRegistroActualizacion);
        Documentacion direccionCoordinacion = await ObtenerDocumentacion(registro.IdSuceso);

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            EliminarEntidadesRelacionadas(direccionCoordinacion, registro);
            await EliminarRegistroActualizacion(registro);

            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error en la transacción de DeleteDocumentacionByIdRegistroCommandHandler");
            throw;
        }

        _logger.LogInformation($"END - {nameof(DeleteDocumentacionByIdRegistroCommandHandler)}");
        return Unit.Value;
    }

    private async Task<RegistroActualizacion> ObtenerRegistroActualizacion(int idRegistroActualizacion)
    {
        var spec = new RegistroActualizacionSpecification(new RegistroActualizacionSpecificationParams { Id = idRegistroActualizacion });
        var registro = await _unitOfWork.Repository<RegistroActualizacion>().GetByIdWithSpec(spec);

        if (registro is null || registro.Borrado || registro.IdTipoRegistroActualizacion != (int)TipoRegistroActualizacionEnum.Documentacion)
        {
            _logger.LogWarning($"RegistroActualizacion no encontrado o inválido | IdRegistroActualizacion: {idRegistroActualizacion}");
            throw new NotFoundException(nameof(RegistroActualizacion), idRegistroActualizacion);
        }

        return registro;
    }

    private async Task<Documentacion> ObtenerDocumentacion(int idSuceso)
    {
        var spec = new DetalleDocumentacionById(new DocumentacionParams { IdSuceso = idSuceso });
        var documentacion = await _unitOfWork.Repository<Documentacion>().GetByIdWithSpec(spec);

        if (documentacion is null || documentacion.Borrado)
        {
            _logger.LogWarning($"Documentacion no encontrada o inválida | IdSuceso: {idSuceso}");
            throw new NotFoundException(nameof(Documentacion), idSuceso);
        }

        return documentacion;
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
            if (eliminarDetalles && elemento is DetalleDocumentacion detalleDocumentacion)
            {
                foreach (var detalle in detalleDocumentacion.DocumentacionProcedenciaDestinos)
                {
                    _unitOfWork.Repository<DocumentacionProcedenciaDestino>().DeleteEntity(detalle);
                }
            }
            _unitOfWork.Repository<T>().DeleteEntity(elemento);
        }
    }

    private void EliminarEntidadesRelacionadas(Documentacion documentacion, RegistroActualizacion registro)
    {
        EliminarElementos(documentacion.DetallesDocumentacion, registro, ApartadoRegistroEnum.Documentacion, eliminarDetalles: true);

        _unitOfWork.Repository<Documentacion>().UpdateEntity(documentacion);
    }
}
