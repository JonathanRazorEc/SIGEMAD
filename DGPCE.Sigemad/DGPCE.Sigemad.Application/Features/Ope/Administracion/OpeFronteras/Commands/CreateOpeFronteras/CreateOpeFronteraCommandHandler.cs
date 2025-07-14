using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeFronteras.Commands.CreateOpeFronteras;

public class CreateOpeFronteraCommandHandler : IRequestHandler<CreateOpeFronteraCommand, CreateOpeFronteraResponse>
{
    private readonly ILogger<CreateOpeFronteraCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateOpeFronteraCommandHandler(
        ILogger<CreateOpeFronteraCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CreateOpeFronteraResponse> Handle(CreateOpeFronteraCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(CreateOpeFronteraCommandHandler) + " - BEGIN");

        var opeFronteraEntity = _mapper.Map<OpeFrontera>(request);
        _unitOfWork.Repository<OpeFrontera>().AddEntity(opeFronteraEntity);

        var result = await _unitOfWork.Complete();
        if (result <= 0)
        {
            throw new Exception("No se pudo insertar nueva ope frontera");
        }

        _logger.LogInformation($"La ope frontera {opeFronteraEntity.Id} fue creado correctamente");

        _logger.LogInformation(nameof(CreateOpeFronteraCommandHandler) + " - END");
        return new CreateOpeFronteraResponse { Id = opeFronteraEntity.Id };
    }
}
