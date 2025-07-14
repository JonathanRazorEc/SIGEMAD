using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeFronteras;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeFronteras.Commands.UpdateOpeFronteras;

public class UpdateOpeFronteraCommandHandler : IRequestHandler<UpdateOpeFronteraCommand>
{
    private readonly ILogger<UpdateOpeFronteraCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateOpeFronteraCommandHandler(
        ILogger<UpdateOpeFronteraCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateOpeFronteraCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(UpdateOpeFronteraCommandHandler) + " - BEGIN");

        var opeFronteraSpec = new OpeFronteraActiveByIdSpecification(request.Id);
        var opeFronteraToUpdate = await _unitOfWork.Repository<OpeFrontera>().GetByIdWithSpec(opeFronteraSpec);

        if (opeFronteraToUpdate == null)
        {
            _logger.LogWarning($"No se encontro ope frontera con id: {request.Id}");
            throw new NotFoundException(nameof(OpeFrontera), request.Id);
        }

        _mapper.Map(request, opeFronteraToUpdate, typeof(UpdateOpeFronteraCommand), typeof(OpeFrontera));

        _unitOfWork.Repository<OpeFrontera>().UpdateEntity(opeFronteraToUpdate);
        await _unitOfWork.Complete();

        _logger.LogInformation($"Se actualizo correctamente la ope frontera con id: {request.Id}");
        _logger.LogInformation(nameof(UpdateOpeFronteraCommandHandler) + " - END");

        return Unit.Value;
    }
}
