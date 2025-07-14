using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeAreasDescanso;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeAreasDescanso.Commands.UpdateOpeAreasDescanso;

public class UpdateOpeAreaDescansoCommandHandler : IRequestHandler<UpdateOpeAreaDescansoCommand>
{
    private readonly ILogger<UpdateOpeAreaDescansoCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateOpeAreaDescansoCommandHandler(
        ILogger<UpdateOpeAreaDescansoCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateOpeAreaDescansoCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(UpdateOpeAreaDescansoCommandHandler) + " - BEGIN");

        var opeAreaDescansoSpec = new OpeAreaDescansoActiveByIdSpecification(request.Id);
        var opeAreaDescansoToUpdate = await _unitOfWork.Repository<OpeAreaDescanso>().GetByIdWithSpec(opeAreaDescansoSpec);


        if (opeAreaDescansoToUpdate == null)
        {
            _logger.LogWarning($"No se encontro ope área descanso con id: {request.Id}");
            throw new NotFoundException(nameof(OpeAreaDescanso), request.Id);
        }

        // TEST
        //Auditoria auditoria = new Auditoria("Ope_AreaDescanso", AuditoriaConstantes.OperacionModificacion, null);
        //auditoria.ValoresAntiguos = opeAreaDescansoToUpdate.ToAuditoria();
        // FIN TEST

        _mapper.Map(request, opeAreaDescansoToUpdate, typeof(UpdateOpeAreaDescansoCommand), typeof(OpeAreaDescanso));

        // TEST
        //auditoria.ValoresNuevos = opeAreaDescansoToUpdate.ToAuditoria();
        // FIN TEST

        _unitOfWork.Repository<OpeAreaDescanso>().UpdateEntity(opeAreaDescansoToUpdate);
        await _unitOfWork.Complete();

        _logger.LogInformation($"Se actualizo correctamente la ope área descanso con id: {request.Id}");
        _logger.LogInformation(nameof(UpdateOpeAreaDescansoCommandHandler) + " - END");

        return Unit.Value;
    }
}
