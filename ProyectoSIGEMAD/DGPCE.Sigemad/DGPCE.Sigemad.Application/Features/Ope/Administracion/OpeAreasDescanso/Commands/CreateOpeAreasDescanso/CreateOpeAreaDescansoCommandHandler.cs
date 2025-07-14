using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeAreasDescanso.Commands.CreateOpeAreasDescanso;

public class CreateOpeAreaDescansoCommandHandler : IRequestHandler<CreateOpeAreaDescansoCommand, CreateOpeAreaDescansoResponse>
{
    private readonly ILogger<CreateOpeAreaDescansoCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateOpeAreaDescansoCommandHandler(
        ILogger<CreateOpeAreaDescansoCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CreateOpeAreaDescansoResponse> Handle(CreateOpeAreaDescansoCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(CreateOpeAreaDescansoCommandHandler) + " - BEGIN");

        var opeAreaDescansoEntity = _mapper.Map<OpeAreaDescanso>(request);
        _unitOfWork.Repository<OpeAreaDescanso>().AddEntity(opeAreaDescansoEntity);

        var result = await _unitOfWork.Complete();
        if (result <= 0)
        {
            throw new Exception("No se pudo insertar nueva ope área descanso");
        }

        _logger.LogInformation($"La ope área descanso {opeAreaDescansoEntity.Id} fue creado correctamente");

        _logger.LogInformation(nameof(CreateOpeAreaDescansoCommandHandler) + " - END");
        return new CreateOpeAreaDescansoResponse { Id = opeAreaDescansoEntity.Id };
    }
}
