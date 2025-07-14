using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePorcentsOcAE.Commands.DeleteOpePorcentajesOcupacionAreasEstacionamiento;

public class DeleteOpePorcentOcAECommandHandler : IRequestHandler<DeleteOpePorcentOcAECommand>
{
    private readonly ILogger<DeleteOpePorcentOcAECommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteOpePorcentOcAECommandHandler(
        ILogger<DeleteOpePorcentOcAECommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(DeleteOpePorcentOcAECommand request, CancellationToken cancellationToken)
    {
        var opePorcentajeOcupacionAreaEstacionamientoToDelete = await _unitOfWork.Repository<OpePorcentajeOcupacionAreaEstacionamiento>().GetByIdAsync(request.Id);
        if (opePorcentajeOcupacionAreaEstacionamientoToDelete is null || opePorcentajeOcupacionAreaEstacionamientoToDelete.Borrado)
        {
            _logger.LogWarning($"El ope porcentaje de ocupación área estacionamiento con id:{request.Id}, no existe en la base de datos");
            throw new NotFoundException(nameof(OpePorcentajeOcupacionAreaEstacionamiento), request.Id);
        }

        _unitOfWork.Repository<OpePorcentajeOcupacionAreaEstacionamiento>().DeleteEntity(opePorcentajeOcupacionAreaEstacionamientoToDelete);

        await _unitOfWork.Complete();
        _logger.LogInformation($"El ope porcentaje de ocupación área estacionamiento con id: {request.Id}, se ha borrado con éxito");

        return Unit.Value;
    }
}
