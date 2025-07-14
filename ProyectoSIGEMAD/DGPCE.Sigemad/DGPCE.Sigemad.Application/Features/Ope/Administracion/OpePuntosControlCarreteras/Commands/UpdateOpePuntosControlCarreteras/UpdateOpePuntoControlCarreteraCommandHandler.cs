using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePuntosControlCarreteras;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePuntosControlCarreteras.Commands.UpdateOpePuntosControlCarreteras;

public class UpdateOpePuntoControlCarreteraCommandHandler : IRequestHandler<UpdateOpePuntoControlCarreteraCommand>
{
    private readonly ILogger<UpdateOpePuntoControlCarreteraCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateOpePuntoControlCarreteraCommandHandler(
        ILogger<UpdateOpePuntoControlCarreteraCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateOpePuntoControlCarreteraCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(UpdateOpePuntoControlCarreteraCommandHandler) + " - BEGIN");

        var opePuntoControlCarreteraSpec = new OpePuntoControlCarreteraActiveByIdSpecification(request.Id);
        var opePuntoControlCarreteraToUpdate = await _unitOfWork.Repository<OpePuntoControlCarretera>().GetByIdWithSpec(opePuntoControlCarreteraSpec);


        if (opePuntoControlCarreteraToUpdate == null)
        {
            _logger.LogWarning($"No se encontro ope punto control de carretera con id: {request.Id}");
            throw new NotFoundException(nameof(OpePuntoControlCarretera), request.Id);
        }

        // TEST
        //Auditoria auditoria = new Auditoria("Ope_PuntoControlCarretera", AuditoriaConstantes.OperacionModificacion, null);
        //auditoria.ValoresAntiguos = opePuntoControlCarreteraToUpdate.ToAuditoria();
        // FIN TEST

        _mapper.Map(request, opePuntoControlCarreteraToUpdate, typeof(UpdateOpePuntoControlCarreteraCommand), typeof(OpePuntoControlCarretera));

        // TEST
        //auditoria.ValoresNuevos = opePuntoControlCarreteraToUpdate.ToAuditoria();
        // FIN TEST

        _unitOfWork.Repository<OpePuntoControlCarretera>().UpdateEntity(opePuntoControlCarreteraToUpdate);
        await _unitOfWork.Complete();

        _logger.LogInformation($"Se actualizo correctamente el ope punto control de carretera con id: {request.Id}");
        _logger.LogInformation(nameof(UpdateOpePuntoControlCarreteraCommandHandler) + " - END");

        return Unit.Value;
    }
}
