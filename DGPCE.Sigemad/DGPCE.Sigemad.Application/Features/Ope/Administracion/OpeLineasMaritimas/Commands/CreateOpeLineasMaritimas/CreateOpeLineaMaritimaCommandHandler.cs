using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Ope.Administracion.OpeLineasMaritimas;
using DGPCE.Sigemad.Application.Contracts.Ope.Datos.OpeDatosEmbarquesDiarios;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeLineasMaritimas.Commands.CreateOpeLineasMaritimas;

public class CreateOpeLineaMaritimaCommandHandler : IRequestHandler<CreateOpeLineaMaritimaCommand, CreateOpeLineaMaritimaResponse>
{
    private readonly ILogger<CreateOpeLineaMaritimaCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IOpeLineaMaritimaService _opeLineaMaritimaService;

    public CreateOpeLineaMaritimaCommandHandler(
        ILogger<CreateOpeLineaMaritimaCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IOpeLineaMaritimaService opeLineaMaritimaService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _opeLineaMaritimaService = opeLineaMaritimaService;
    }

    public async Task<CreateOpeLineaMaritimaResponse> Handle(CreateOpeLineaMaritimaCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(CreateOpeLineaMaritimaCommandHandler) + " - BEGIN");

        // VALIDACIONES
        await _opeLineaMaritimaService.ValidarRegistrosDuplicados(null, request.IdOpePuertoOrigen, request.IdOpePuertoDestino, request.FechaValidezDesde, request.FechaValidezHasta);
        // FIN VALIDACIONES

        var opeLineaMaritimaEntity = _mapper.Map<OpeLineaMaritima>(request);
        _unitOfWork.Repository<OpeLineaMaritima>().AddEntity(opeLineaMaritimaEntity);

        var result = await _unitOfWork.Complete();
        if (result <= 0)
        {
            throw new Exception("No se pudo insertar nueva ope línea marítima");
        }

        _logger.LogInformation($"El ope periodo {opeLineaMaritimaEntity.Id} fue creado correctamente");

        _logger.LogInformation(nameof(CreateOpeLineaMaritimaCommandHandler) + " - END");
        return new CreateOpeLineaMaritimaResponse { Id = opeLineaMaritimaEntity.Id };
    }
}
