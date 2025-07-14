using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Documentaciones.Commands.DeleteDocumentaciones;
public class DeleteDocumentacionCommandHandler : IRequestHandler<DeleteDocumentacionCommand>
{
    private readonly ILogger<DeleteDocumentacionCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDocumentacionCommandHandler(
        ILogger<DeleteDocumentacionCommandHandler> logger,
        IUnitOfWork unitOfWork
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteDocumentacionCommand request, CancellationToken cancellationToken)
    {
        var documentacion = await _unitOfWork.Repository<Documentacion>().GetByIdAsync(request.Id);
        if (documentacion == null || documentacion.Borrado)
        {
            _logger.LogError($"La documentacion con id:{request.Id}, no existe en la base de datos");
            throw new NotFoundException(nameof(Documentacion), request.Id);
        }

        _unitOfWork.Repository<Documentacion>().DeleteEntity(documentacion);
        var result = await _unitOfWork.Complete();

        if (result <= 0)
        {
            _logger.LogError($"Error al completar la eliminación de la documentacion con id: {request.Id}");
            throw new Exception("No se pudo insertar/actualizar los detalles de la documentacion");
        }

        _logger.LogInformation($"La documentacion con id: {request.Id}, fue eliminado con exito");
        return Unit.Value;
    }
}
