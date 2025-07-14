using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Ope.Administracion.OpeLineasMaritimas;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeLineasMaritimas;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeLineasMaritimas.Commands.UpdateOpeLineasMaritimas;

public class UpdateOpeLineaMaritimaCommandHandler : IRequestHandler<UpdateOpeLineaMaritimaCommand>
{
    private readonly ILogger<UpdateOpeLineaMaritimaCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IOpeLineaMaritimaService _opeLineaMaritimaService;

    public UpdateOpeLineaMaritimaCommandHandler(
        ILogger<UpdateOpeLineaMaritimaCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IOpeLineaMaritimaService opeLineaMaritimaService
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _opeLineaMaritimaService = opeLineaMaritimaService;
    }

    public async Task<Unit> Handle(UpdateOpeLineaMaritimaCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(UpdateOpeLineaMaritimaCommandHandler) + " - BEGIN");

        var opeLineaMaritimaSpec = new OpeLineaMaritimaActiveByIdSpecification(request.Id);
        var opeLineaMaritimaToUpdate = await _unitOfWork.Repository<OpeLineaMaritima>().GetByIdWithSpec(opeLineaMaritimaSpec);


        if (opeLineaMaritimaToUpdate == null)
        {
            _logger.LogWarning($"No se encontro ope línea marítima con id: {request.Id}");
            throw new NotFoundException(nameof(OpeLineaMaritima), request.Id);
        }

        // VALIDACIONES
        await _opeLineaMaritimaService.ValidarRegistrosDuplicados(request.Id, request.IdOpePuertoOrigen, request.IdOpePuertoDestino, request.FechaValidezDesde, request.FechaValidezHasta);
        // FIN VALIDACIONES

        _mapper.Map(request, opeLineaMaritimaToUpdate, typeof(UpdateOpeLineaMaritimaCommand), typeof(OpeLineaMaritima));

        _unitOfWork.Repository<OpeLineaMaritima>().UpdateEntity(opeLineaMaritimaToUpdate);
        await _unitOfWork.Complete();

        _logger.LogInformation($"Se actualizo correctamente la ope línea marítima con id: {request.Id}");
        _logger.LogInformation(nameof(UpdateOpeLineaMaritimaCommandHandler) + " - END");

        return Unit.Value;
    }
}
