using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Ope.Datos.OpeDatosEmbarquesDiarios;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosEmbarquesDiarios.Commands.CreateOpeDatosEmbarquesDiarios;

public class CreateOpeDatoEmbarqueDiarioCommandHandler : IRequestHandler<CreateOpeDatoEmbarqueDiarioCommand, CreateOpeDatoEmbarqueDiarioResponse>
{
    private readonly ILogger<CreateOpeDatoEmbarqueDiarioCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IOpeDatoEmbarqueDiarioService _opeDatoEmbarqueDiarioService;

    public CreateOpeDatoEmbarqueDiarioCommandHandler(
        ILogger<CreateOpeDatoEmbarqueDiarioCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper,
         IOpeDatoEmbarqueDiarioService opeDatoEmbarqueDiarioService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _opeDatoEmbarqueDiarioService = opeDatoEmbarqueDiarioService;
    }

    public async Task<CreateOpeDatoEmbarqueDiarioResponse> Handle(CreateOpeDatoEmbarqueDiarioCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(CreateOpeDatoEmbarqueDiarioCommandHandler) + " - BEGIN");

        // VALIDACIONES
        await _opeDatoEmbarqueDiarioService.ValidarRegistrosDuplicados(null, request.IdOpeLineaMaritima, request.Fecha);
        // FIN VALIDACIONES

        var opeDatoEmbarqueDiarioEntity = _mapper.Map<OpeDatoEmbarqueDiario>(request);
        _unitOfWork.Repository<OpeDatoEmbarqueDiario>().AddEntity(opeDatoEmbarqueDiarioEntity);

        var result = await _unitOfWork.Complete();
        if (result <= 0)
        {
            throw new Exception("No se pudo insertar nuevo ope dato de embarque diario");
        }

        _logger.LogInformation($"El ope dato de embarque diario {opeDatoEmbarqueDiarioEntity.Id} fue creado correctamente");

        _logger.LogInformation(nameof(CreateOpeDatoEmbarqueDiarioCommandHandler) + " - END");
        //return new CreateOpeDatoEmbarqueDiarioResponse { Id = opeDatoEmbarqueDiarioEntity.Id };

        return new CreateOpeDatoEmbarqueDiarioResponse
        {
            Id = opeDatoEmbarqueDiarioEntity.Id,
            FechaCreacion = opeDatoEmbarqueDiarioEntity.FechaCreacion,
            CreadoPor = opeDatoEmbarqueDiarioEntity.CreadoPor
        };
    }
}
