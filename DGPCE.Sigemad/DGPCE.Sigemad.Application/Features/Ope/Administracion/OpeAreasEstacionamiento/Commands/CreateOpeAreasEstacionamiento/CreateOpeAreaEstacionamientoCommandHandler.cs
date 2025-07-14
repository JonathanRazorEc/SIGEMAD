using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeAreasEstacionamiento.Commands.CreateOpeAreasEstacionamiento;

public class CreateOpeAreaEstacionamientoCommandHandler : IRequestHandler<CreateOpeAreaEstacionamientoCommand, CreateOpeAreaEstacionamientoResponse>
{
    private readonly ILogger<CreateOpeAreaEstacionamientoCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateOpeAreaEstacionamientoCommandHandler(
        ILogger<CreateOpeAreaEstacionamientoCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CreateOpeAreaEstacionamientoResponse> Handle(CreateOpeAreaEstacionamientoCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(CreateOpeAreaEstacionamientoCommandHandler) + " - BEGIN");

        var opeAreaEstacionamientoEntity = _mapper.Map<OpeAreaEstacionamiento>(request);
        _unitOfWork.Repository<OpeAreaEstacionamiento>().AddEntity(opeAreaEstacionamientoEntity);

        var result = await _unitOfWork.Complete();
        if (result <= 0)
        {
            throw new Exception("No se pudo insertar nueva ope área de estacionamiento");
        }

        _logger.LogInformation($"La ope área de estacionamiento{opeAreaEstacionamientoEntity.Id} fue creada correctamente");

        _logger.LogInformation(nameof(CreateOpeAreaEstacionamientoCommandHandler) + " - END");
        return new CreateOpeAreaEstacionamientoResponse { Id = opeAreaEstacionamientoEntity.Id };
    }
}
