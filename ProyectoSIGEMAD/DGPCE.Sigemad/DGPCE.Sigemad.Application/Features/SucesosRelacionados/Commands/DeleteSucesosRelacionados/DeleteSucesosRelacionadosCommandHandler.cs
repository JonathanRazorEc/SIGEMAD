using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.SucesosRelacionados.Commands.DeleteSucesosRelacionados;
public class DeleteSucesosRelacionadosCommandHandler : IRequestHandler<DeleteSucesosRelacionadosCommand>
{
    private readonly ILogger<DeleteSucesosRelacionadosCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteSucesosRelacionadosCommandHandler(
        ILogger<DeleteSucesosRelacionadosCommandHandler> logger,
        IUnitOfWork unitOfWork
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteSucesosRelacionadosCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(DeleteSucesosRelacionadosCommandHandler)} - BEGIN");

        var sucesosRelacionadosToDelete = await _unitOfWork.Repository<SucesoRelacionado>().GetByIdAsync(request.Id);
        if (sucesosRelacionadosToDelete is null || sucesosRelacionadosToDelete.Borrado)
        {
            _logger.LogWarning($"El suceso relacionado con id:{request.Id}, no existe en la base de datos");
            throw new NotFoundException(nameof(SucesoRelacionado), request.Id);
        }

        _unitOfWork.Repository<SucesoRelacionado>().DeleteEntity(sucesosRelacionadosToDelete);
        await _unitOfWork.Complete();

        _logger.LogInformation($"El suceso relacionado con id: {request.Id}, fue eliminado con exito");
        _logger.LogInformation($"{nameof(DeleteSucesosRelacionadosCommandHandler)} - END");

        return Unit.Value;
    }
}
