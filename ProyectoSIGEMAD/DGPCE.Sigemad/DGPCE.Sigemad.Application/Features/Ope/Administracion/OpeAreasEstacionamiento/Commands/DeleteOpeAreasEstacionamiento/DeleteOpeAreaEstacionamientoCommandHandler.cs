using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeAreasEstacionamiento.Commands.DeleteOpeAreasEstacionamiento;

public class DeleteOpeAreaEstacionamientoCommandHandler : IRequestHandler<DeleteOpeAreaEstacionamientoCommand>
{
    private readonly ILogger<DeleteOpeAreaEstacionamientoCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteOpeAreaEstacionamientoCommandHandler(
        ILogger<DeleteOpeAreaEstacionamientoCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(DeleteOpeAreaEstacionamientoCommand request, CancellationToken cancellationToken)
    {
        var opeAreaEstacionamientoToDelete = await _unitOfWork.Repository<OpeAreaEstacionamiento>().GetByIdAsync(request.Id);
        if (opeAreaEstacionamientoToDelete is null || opeAreaEstacionamientoToDelete.Borrado)
        {
            _logger.LogWarning($"La ope área de estacionamiento con id:{request.Id}, no existe en la base de datos");
            throw new NotFoundException(nameof(OpeAreaEstacionamiento), request.Id);
        }

        _unitOfWork.Repository<OpeAreaEstacionamiento>().DeleteEntity(opeAreaEstacionamientoToDelete);

        await _unitOfWork.Complete();
        _logger.LogInformation($"La ope área de estacionamiento con id: {request.Id}, se ha borrado con éxito");

        return Unit.Value;
    }
}
