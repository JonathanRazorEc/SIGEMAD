using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeFronteras.Commands.DeleteOpeFronteras;

public class DeleteOpeFronteraCommandHandler : IRequestHandler<DeleteOpeFronteraCommand>
{
    private readonly ILogger<DeleteOpeFronteraCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteOpeFronteraCommandHandler(
        ILogger<DeleteOpeFronteraCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(DeleteOpeFronteraCommand request, CancellationToken cancellationToken)
    {
        var opeFronteraToDelete = await _unitOfWork.Repository<OpeFrontera>().GetByIdAsync(request.Id);
        if (opeFronteraToDelete is null || opeFronteraToDelete.Borrado)
        {
            _logger.LogWarning($"La ope frontera con id:{request.Id}, no existe en la base de datos");
            throw new NotFoundException(nameof(OpeFrontera), request.Id);
        }

        _unitOfWork.Repository<OpeFrontera>().DeleteEntity(opeFronteraToDelete);

        await _unitOfWork.Complete();
        _logger.LogInformation($"La ope frontera con id: {request.Id}, se ha borrado con éxito");

        return Unit.Value;
    }
}
