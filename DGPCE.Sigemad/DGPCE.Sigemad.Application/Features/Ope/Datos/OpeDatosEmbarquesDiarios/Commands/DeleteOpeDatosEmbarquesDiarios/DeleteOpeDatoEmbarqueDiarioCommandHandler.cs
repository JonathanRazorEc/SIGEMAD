using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosEmbarquesDiarios.Commands.DeleteOpeDatosEmbarquesDiarios;

public class DeleteOpeDatoEmbarqueDiarioCommandHandler : IRequestHandler<DeleteOpeDatoEmbarqueDiarioCommand>
{
    private readonly ILogger<DeleteOpeDatoEmbarqueDiarioCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteOpeDatoEmbarqueDiarioCommandHandler(
        ILogger<DeleteOpeDatoEmbarqueDiarioCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(DeleteOpeDatoEmbarqueDiarioCommand request, CancellationToken cancellationToken)
    {
        var opeDatoEmbarqueDiarioToDelete = await _unitOfWork.Repository<OpeDatoEmbarqueDiario>().GetByIdAsync(request.Id);
        if (opeDatoEmbarqueDiarioToDelete is null || opeDatoEmbarqueDiarioToDelete.Borrado)
        {
            _logger.LogWarning($"El ope dato de embarque diario con id:{request.Id}, no existe en la base de datos");
            throw new NotFoundException(nameof(OpeDatoEmbarqueDiario), request.Id);
        }

        _unitOfWork.Repository<OpeDatoEmbarqueDiario>().DeleteEntity(opeDatoEmbarqueDiarioToDelete);

        await _unitOfWork.Complete();
        _logger.LogInformation($"El ope dato de embarque diario con id: {request.Id}, se ha borrado con éxito");

        return Unit.Value;
    }
}
