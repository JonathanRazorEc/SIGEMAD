using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Ope.Administracion.OpePeriodos;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpePeriodos.Commands.CreateOpePeriodos;

public class CreateOpePeriodoCommandHandler : IRequestHandler<CreateOpePeriodoCommand, CreateOpePeriodoResponse>
{
    private readonly ILogger<CreateOpePeriodoCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IOpePeriodoService _opePeriodoService;

    public CreateOpePeriodoCommandHandler(
        ILogger<CreateOpePeriodoCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IOpePeriodoService opePeriodoService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _opePeriodoService = opePeriodoService;
    }

    public async Task<CreateOpePeriodoResponse> Handle(CreateOpePeriodoCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(CreateOpePeriodoCommandHandler) + " - BEGIN");

        // VALIDACIONES
        await _opePeriodoService.ValidarRegistrosDuplicados(null, request.Nombre, request.FechaInicioFaseSalida, request.FechaFinFaseSalida, request.FechaInicioFaseRetorno, request.FechaFinFaseRetorno);
        // FIN VALIDACIONES

        var opePeriodoEntity = _mapper.Map<OpePeriodo>(request);
        _unitOfWork.Repository<OpePeriodo>().AddEntity(opePeriodoEntity);

        var result = await _unitOfWork.Complete();
        if (result <= 0)
        {
            throw new Exception("No se pudo insertar nuevo ope periodo");
        }

        _logger.LogInformation($"El ope periodo {opePeriodoEntity.Id} fue creado correctamente");

        _logger.LogInformation(nameof(CreateOpePeriodoCommandHandler) + " - END");
        return new CreateOpePeriodoResponse { Id = opePeriodoEntity.Id };
    }
}
