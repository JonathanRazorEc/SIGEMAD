using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Evoluciones.Commands.DeleteEvoluciones;
public class DeleteEvolucionCommandHandler : IRequestHandler<DeleteEvolucionCommand>
{
    private readonly ILogger<DeleteEvolucionCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteEvolucionCommandHandler(
        ILogger<DeleteEvolucionCommandHandler> logger,
        IUnitOfWork unitOfWork
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteEvolucionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(DeleteEvolucionCommandHandler)} - BEGIN");

        var evolucionToDelete = _unitOfWork.Repository<Evolucion>().GetByIdAsync(request.Id).Result;
        if (evolucionToDelete is null || evolucionToDelete.Borrado)
        {
            _logger.LogWarning($"request.Id: {request.Id}, no encontrado");
            throw new NotFoundException(nameof(Evolucion), request.Id);
        }

        _unitOfWork.Repository<Evolucion>().DeleteEntity(evolucionToDelete);

        await _unitOfWork.Complete();

        _logger.LogInformation($"La evolucion con id: {request.Id}, fue eliminado con exito");

        _logger.LogInformation($"{nameof(DeleteEvolucionCommandHandler)} - END");

        return Unit.Value;
    }
}
