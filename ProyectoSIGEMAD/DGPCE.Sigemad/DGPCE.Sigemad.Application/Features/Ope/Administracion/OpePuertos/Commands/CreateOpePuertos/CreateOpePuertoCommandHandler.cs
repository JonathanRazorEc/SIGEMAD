using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Ope.Administracion.OpePuertos;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePuertos.Commands.CreateOpePuertos;

public class CreateOpePuertoCommandHandler : IRequestHandler<CreateOpePuertoCommand, CreateOpePuertoResponse>
{
    private readonly ILogger<CreateOpePuertoCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IOpePuertoService _opePuertoService;

    public CreateOpePuertoCommandHandler(
        ILogger<CreateOpePuertoCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IOpePuertoService opePuertoService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _opePuertoService = opePuertoService;
    }

    public async Task<CreateOpePuertoResponse> Handle(CreateOpePuertoCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(CreateOpePuertoCommandHandler) + " - BEGIN");

        // VALIDACIONES
        await _opePuertoService.ValidarRegistrosDuplicados(null, request.Nombre, request.FechaValidezDesde, request.FechaValidezHasta);
        // FIN VALIDACIONES

        var opePuertoEntity = _mapper.Map<OpePuerto>(request);
        _unitOfWork.Repository<OpePuerto>().AddEntity(opePuertoEntity);

        var result = await _unitOfWork.Complete();
        if (result <= 0)
        {
            throw new Exception("No se pudo insertar nuevo ope puerto");
        }

        _logger.LogInformation($"El ope puerto {opePuertoEntity.Id} fue creado correctamente");

        _logger.LogInformation(nameof(CreateOpePuertoCommandHandler) + " - END");
        return new CreateOpePuertoResponse { Id = opePuertoEntity.Id };
    }
}
