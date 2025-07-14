using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePuntosControlCarreteras.Commands.DeleteOpePuntosControlCarreteras;

public class DeleteOpePuntoControlCarreteraCommandHandler : IRequestHandler<DeleteOpePuntoControlCarreteraCommand>
{
    private readonly ILogger<DeleteOpePuntoControlCarreteraCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteOpePuntoControlCarreteraCommandHandler(
        ILogger<DeleteOpePuntoControlCarreteraCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(DeleteOpePuntoControlCarreteraCommand request, CancellationToken cancellationToken)
    {
        var opePuntoControlCarreteraToDelete = await _unitOfWork.Repository<OpePuntoControlCarretera>().GetByIdAsync(request.Id);
        if (opePuntoControlCarreteraToDelete is null || opePuntoControlCarreteraToDelete.Borrado)
        {
            _logger.LogWarning($"El ope punto control de carretera con id:{request.Id}, no existe en la base de datos");
            throw new NotFoundException(nameof(OpePuntoControlCarretera), request.Id);
        }

        _unitOfWork.Repository<OpePuntoControlCarretera>().DeleteEntity(opePuntoControlCarreteraToDelete);

        await _unitOfWork.Complete();
        _logger.LogInformation($"El ope punto control de carretera con id: {request.Id}, se ha borrado con éxito");

        return Unit.Value;
    }
}
