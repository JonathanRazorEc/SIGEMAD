using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeAreasDescanso.Commands.DeleteOpeAreasDescanso;

public class DeleteOpeAreaDescansoCommandHandler : IRequestHandler<DeleteOpeAreaDescansoCommand>
{
    private readonly ILogger<DeleteOpeAreaDescansoCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteOpeAreaDescansoCommandHandler(
        ILogger<DeleteOpeAreaDescansoCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(DeleteOpeAreaDescansoCommand request, CancellationToken cancellationToken)
    {
        var opeAreaDescansoToDelete = await _unitOfWork.Repository<OpeAreaDescanso>().GetByIdAsync(request.Id);
        if (opeAreaDescansoToDelete is null || opeAreaDescansoToDelete.Borrado)
        {
            _logger.LogWarning($"La ope área descanso con id:{request.Id}, no existe en la base de datos");
            throw new NotFoundException(nameof(OpeAreaDescanso), request.Id);
        }

        _unitOfWork.Repository<OpeAreaDescanso>().DeleteEntity(opeAreaDescansoToDelete);

        await _unitOfWork.Complete();
        _logger.LogInformation($"La ope área descanso con id: {request.Id}, se ha borrado con éxito");

        return Unit.Value;
    }
}
