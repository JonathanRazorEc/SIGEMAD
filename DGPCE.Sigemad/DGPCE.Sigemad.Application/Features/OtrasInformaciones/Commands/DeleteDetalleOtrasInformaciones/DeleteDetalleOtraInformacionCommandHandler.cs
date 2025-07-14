using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.OtrasInformaciones;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.OtrasInformaciones.Commands.DeleteOtrasInformaciones;
public class DeleteDetalleOtraInformacionCommandHandler : IRequestHandler<DeleteDetalleOtraInformacionCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteDetalleOtraInformacionCommandHandler> _logger;

    public DeleteDetalleOtraInformacionCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteDetalleOtraInformacionCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteDetalleOtraInformacionCommand request, CancellationToken cancellationToken)
    {
        var specDetalle = new DetalleOtraInformacionByIdSpecification(request.IdDetalleOtraInformacion);
        var detalle = await _unitOfWork.Repository<DetalleOtraInformacion>().GetByIdWithSpec(specDetalle);
        
        if (detalle == null)
        {
            _logger.LogWarning($"DetalleOtraInformacion con Id {request.IdDetalleOtraInformacion} no encontrado");
            throw new NotFoundException(nameof(DetalleOtraInformacion), request.IdDetalleOtraInformacion);
        }

        detalle.Borrado = true;
        foreach (var pd in detalle.ProcedenciasDestinos)
        {
            pd.Borrado = true;
        }

        _unitOfWork.Repository<DetalleOtraInformacion>().UpdateEntity(detalle);
        var result = await _unitOfWork.Complete();

        if (result == 0)
        {
            _logger.LogError("No se pudo eliminar el DetalleOtraInformacion");
            throw new Exception("No se pudo eliminar el DetalleOtraInformacion");
        }

        _logger.LogInformation($"DetalleOtraInformacion con Id {request.IdDetalleOtraInformacion} eliminado lógico correcto");

        return Unit.Value;
    }
}
