using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Ope.Administracion.OpePorcentsOcAE;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePorcentsOcAE.Commands.CreateOpePorcentajesOcupacionAreasEstacionamiento;

public class CreateOpePorcentOcAECommandHandler : IRequestHandler<CreateOpePorcentOcAECommand, CreateOpePorcentOcAEResponse>
{
    private readonly ILogger<CreateOpePorcentOcAECommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IOpePorcentOcAEService _opePorcentOcAEService;

    public CreateOpePorcentOcAECommandHandler(
        ILogger<CreateOpePorcentOcAECommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IOpePorcentOcAEService opePorcentOcAEService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _opePorcentOcAEService = opePorcentOcAEService;
    }

    public async Task<CreateOpePorcentOcAEResponse> Handle(CreateOpePorcentOcAECommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(CreateOpePorcentOcAECommandHandler) + " - BEGIN");

        // VALIDACIONES
        await _opePorcentOcAEService.ValidarRegistrosDuplicados(null, request.IdOpeOcupacion);
        // FIN VALIDACIONES

        var opePorcentajeOcupacionAreaEstacionamientoEntity = _mapper.Map<OpePorcentajeOcupacionAreaEstacionamiento>(request);
        _unitOfWork.Repository<OpePorcentajeOcupacionAreaEstacionamiento>().AddEntity(opePorcentajeOcupacionAreaEstacionamientoEntity);

        var result = await _unitOfWork.Complete();
        if (result <= 0)
        {
            throw new Exception("No se pudo insertar nuevo ope porcentaje de ocupación área estacionamiento");
        }

        _logger.LogInformation($"El ope porcentaje de ocupación área estacionamiento {opePorcentajeOcupacionAreaEstacionamientoEntity.Id} fue creado correctamente");

        _logger.LogInformation(nameof(CreateOpePorcentOcAECommandHandler) + " - END");
        return new CreateOpePorcentOcAEResponse { Id = opePorcentajeOcupacionAreaEstacionamientoEntity.Id };
    }
}
