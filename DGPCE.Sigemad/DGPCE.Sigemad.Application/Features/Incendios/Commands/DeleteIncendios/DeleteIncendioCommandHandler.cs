using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Incendios.Commands.DeleteIncendios;

public class DeleteIncendioCommandHandler : IRequestHandler<DeleteIncendioCommand>
{
    private readonly ILogger<DeleteIncendioCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteIncendioCommandHandler(
        ILogger<DeleteIncendioCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(DeleteIncendioCommand request, CancellationToken cancellationToken)
    {
        var incendioToDelete = await _unitOfWork.Repository<Incendio>().GetByIdAsync(request.Id);
        if (incendioToDelete is null || incendioToDelete.Borrado)
        {
            _logger.LogWarning($"El incendio con id:{request.Id}, no existe en la base de datos");
            throw new NotFoundException(nameof(Incendio), request.Id);
        }

        _unitOfWork.Repository<Incendio>().DeleteEntity(incendioToDelete);

        await _unitOfWork.Complete();
        _logger.LogInformation($"El incendio con id: {request.Id}, se actualizo estado de borrado con éxito");

        return Unit.Value;
    }
}
