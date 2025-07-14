using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Ope.Administracion.OpePuertos;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePuertos;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePuertos.Commands.UpdateOpePuertos;

public class UpdateOpePuertoCommandHandler : IRequestHandler<UpdateOpePuertoCommand>
{
    private readonly ILogger<UpdateOpePuertoCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IOpePuertoService _opePuertoService;

    public UpdateOpePuertoCommandHandler(
        ILogger<UpdateOpePuertoCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IOpePuertoService opePuertoService
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _opePuertoService = opePuertoService;
    }

    public async Task<Unit> Handle(UpdateOpePuertoCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(UpdateOpePuertoCommandHandler) + " - BEGIN");

        var opePuertoSpec = new OpePuertoActiveByIdSpecification(request.Id);
        var opePuertoToUpdate = await _unitOfWork.Repository<OpePuerto>().GetByIdWithSpec(opePuertoSpec);


        if (opePuertoToUpdate == null)
        {
            _logger.LogWarning($"No se encontro ope puerto con id: {request.Id}");
            throw new NotFoundException(nameof(OpePuerto), request.Id);
        }

        // VALIDACIONES
        await _opePuertoService.ValidarRegistrosDuplicados(request.Id, request.Nombre, request.FechaValidezDesde, request.FechaValidezHasta);
        // FIN VALIDACIONES

        _mapper.Map(request, opePuertoToUpdate, typeof(UpdateOpePuertoCommand), typeof(OpePuerto));

        _unitOfWork.Repository<OpePuerto>().UpdateEntity(opePuertoToUpdate);
        await _unitOfWork.Complete();

        _logger.LogInformation($"Se actualizo correctamente el ope puerto con id: {request.Id}");
        _logger.LogInformation(nameof(UpdateOpePuertoCommandHandler) + " - END");

        return Unit.Value;
    }
}
