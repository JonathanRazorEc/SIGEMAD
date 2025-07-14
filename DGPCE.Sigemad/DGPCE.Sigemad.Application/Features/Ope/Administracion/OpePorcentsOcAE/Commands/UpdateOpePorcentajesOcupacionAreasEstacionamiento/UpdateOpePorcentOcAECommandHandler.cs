using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Ope.Administracion.OpePorcentsOcAE;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePorcentajesOcupacionAreasEstacionamiento;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePorcentsOcAE.Commands.UpdateOpePorcentajesOcupacionAreasEstacionamiento;

public class UpdateOpePorcentOcAECommandHandler : IRequestHandler<UpdateOpePorcentOcAECommand>
{
    private readonly ILogger<UpdateOpePorcentOcAECommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IOpePorcentOcAEService _opePorcentOcAEService;

    public UpdateOpePorcentOcAECommandHandler(
        ILogger<UpdateOpePorcentOcAECommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IOpePorcentOcAEService opePorcentOcAEService
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _opePorcentOcAEService = opePorcentOcAEService;
    }

    public async Task<Unit> Handle(UpdateOpePorcentOcAECommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(UpdateOpePorcentOcAECommandHandler) + " - BEGIN");

        var opePorcentajeOcupacionAreaEstacionamientoSpec = new OpePorcentajeOcupacionAreaEstacionamientoActiveByIdSpecification(request.Id);
        var opePorcentajeOcupacionAreaEstacionamientoToUpdate = await _unitOfWork.Repository<OpePorcentajeOcupacionAreaEstacionamiento>().GetByIdWithSpec(opePorcentajeOcupacionAreaEstacionamientoSpec);


        if (opePorcentajeOcupacionAreaEstacionamientoToUpdate == null)
        {
            _logger.LogWarning($"No se encontro ope porcentaje de ocupación área estacionamiento con id: {request.Id}");
            throw new NotFoundException(nameof(OpePorcentajeOcupacionAreaEstacionamiento), request.Id);
        }

        // VALIDACIONES
        await _opePorcentOcAEService.ValidarRegistrosDuplicados(request.Id, request.IdOpeOcupacion);
        // FIN VALIDACIONES

        _mapper.Map(request, opePorcentajeOcupacionAreaEstacionamientoToUpdate, typeof(UpdateOpePorcentOcAECommand), typeof(OpePorcentajeOcupacionAreaEstacionamiento));

        _unitOfWork.Repository<OpePorcentajeOcupacionAreaEstacionamiento>().UpdateEntity(opePorcentajeOcupacionAreaEstacionamientoToUpdate);
        await _unitOfWork.Complete();

        _logger.LogInformation($"Se actualizo correctamente el ope porcentaje de ocupación área estacionamiento con id: {request.Id}");
        _logger.LogInformation(nameof(UpdateOpePorcentOcAECommandHandler) + " - END");

        return Unit.Value;
    }
}
