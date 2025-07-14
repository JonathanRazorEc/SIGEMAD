using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeAreasEstacionamiento;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeAreasEstacionamiento.Commands.UpdateOpeAreasEstacionamiento;

public class UpdateOpeAreaEstacionamientoCommandHandler : IRequestHandler<UpdateOpeAreaEstacionamientoCommand>
{
    private readonly ILogger<UpdateOpeAreaEstacionamientoCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateOpeAreaEstacionamientoCommandHandler(
        ILogger<UpdateOpeAreaEstacionamientoCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateOpeAreaEstacionamientoCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(UpdateOpeAreaEstacionamientoCommandHandler) + " - BEGIN");

        var opeAreaEstacionamientoSpec = new OpeAreaEstacionamientoActiveByIdSpecification(request.Id);
        var opeAreaEstacionamientoToUpdate = await _unitOfWork.Repository<OpeAreaEstacionamiento>().GetByIdWithSpec(opeAreaEstacionamientoSpec);


        if (opeAreaEstacionamientoToUpdate == null)
        {
            _logger.LogWarning($"No se encontro ope área de estacionamiento con id: {request.Id}");
            throw new NotFoundException(nameof(OpeAreaEstacionamiento), request.Id);
        }

        // TEST
        //Auditoria auditoria = new Auditoria("Ope_Frontera", AuditoriaConstantes.OperacionModificacion, null);
        //auditoria.ValoresAntiguos = opeAreaEstacionamientoToUpdate.ToAuditoria();
        // FIN TEST

        _mapper.Map(request, opeAreaEstacionamientoToUpdate, typeof(UpdateOpeAreaEstacionamientoCommand), typeof(OpeAreaEstacionamiento));

        // TEST
        //auditoria.ValoresNuevos = opeAreaEstacionamientoToUpdate.ToAuditoria();
        // FIN TEST

        _unitOfWork.Repository<OpeAreaEstacionamiento>().UpdateEntity(opeAreaEstacionamientoToUpdate);
        await _unitOfWork.Complete();

        _logger.LogInformation($"Se actualizo correctamente la ope área de estacionamiento con id: {request.Id}");
        _logger.LogInformation(nameof(UpdateOpeAreaEstacionamientoCommandHandler) + " - END");

        return Unit.Value;
    }
}
