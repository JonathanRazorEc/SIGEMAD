using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePuntosControlCarreteras.Commands.CreateOpePuntosControlCarreteras;

public class CreateOpePuntoControlCarreteraCommandHandler : IRequestHandler<CreateOpePuntoControlCarreteraCommand, CreateOpePuntoControlCarreteraResponse>
{
    private readonly ILogger<CreateOpePuntoControlCarreteraCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateOpePuntoControlCarreteraCommandHandler(
        ILogger<CreateOpePuntoControlCarreteraCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CreateOpePuntoControlCarreteraResponse> Handle(CreateOpePuntoControlCarreteraCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(CreateOpePuntoControlCarreteraCommandHandler) + " - BEGIN");

        var opePuntoControlCarreteraEntity = _mapper.Map<OpePuntoControlCarretera>(request);
        _unitOfWork.Repository<OpePuntoControlCarretera>().AddEntity(opePuntoControlCarreteraEntity);

        var result = await _unitOfWork.Complete();
        if (result <= 0)
        {
            throw new Exception("No se pudo insertar nuevo ope punto control de carretera");
        }

        _logger.LogInformation($"El ope punto control de carretera {opePuntoControlCarreteraEntity.Id} fue creado correctamente");

        _logger.LogInformation(nameof(CreateOpePuntoControlCarreteraCommandHandler) + " - END");
        return new CreateOpePuntoControlCarreteraResponse { Id = opePuntoControlCarreteraEntity.Id };
    }
}
