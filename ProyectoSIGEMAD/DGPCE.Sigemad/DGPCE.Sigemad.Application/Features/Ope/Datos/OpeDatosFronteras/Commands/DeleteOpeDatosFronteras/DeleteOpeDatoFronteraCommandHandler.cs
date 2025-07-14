using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronteras.Commands.DeleteOpeDatosFronteras;

public class DeleteOpeDatoFronteraCommandHandler : IRequestHandler<DeleteOpeDatoFronteraCommand>
{
    private readonly ILogger<DeleteOpeDatoFronteraCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteOpeDatoFronteraCommandHandler(
        ILogger<DeleteOpeDatoFronteraCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(DeleteOpeDatoFronteraCommand request, CancellationToken cancellationToken)
    {
        var opeDatoFronteraToDelete = await _unitOfWork.Repository<OpeDatoFrontera>().GetByIdAsync(request.Id);
        if (opeDatoFronteraToDelete is null || opeDatoFronteraToDelete.Borrado)
        {
            _logger.LogWarning($"El ope dato de frontera con id:{request.Id}, no existe en la base de datos");
            throw new NotFoundException(nameof(OpeDatoFrontera), request.Id);
        }

        _unitOfWork.Repository<OpeDatoFrontera>().DeleteEntity(opeDatoFronteraToDelete);

        await _unitOfWork.Complete();
        _logger.LogInformation($"El ope dato de frontera con id: {request.Id}, se ha borrado con éxito");

        return Unit.Value;
    }
}
