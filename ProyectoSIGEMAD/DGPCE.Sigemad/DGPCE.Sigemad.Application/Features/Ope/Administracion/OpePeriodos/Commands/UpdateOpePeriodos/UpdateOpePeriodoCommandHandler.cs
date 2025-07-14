using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Ope.Administracion.OpePeriodos;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePeriodos;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpePeriodos.Commands.UpdateOpePeriodos;

public class UpdateOpePeriodoCommandHandler : IRequestHandler<UpdateOpePeriodoCommand>
{
    private readonly ILogger<UpdateOpePeriodoCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IOpePeriodoService _opePeriodoService;

    public UpdateOpePeriodoCommandHandler(
        ILogger<UpdateOpePeriodoCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IOpePeriodoService opePeriodoService
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _opePeriodoService = opePeriodoService;
    }

    public async Task<Unit> Handle(UpdateOpePeriodoCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(UpdateOpePeriodoCommandHandler) + " - BEGIN");

        var opePeriodoSpec = new OpePeriodoActiveByIdSpecification(request.Id);
        var opePeriodoToUpdate = await _unitOfWork.Repository<OpePeriodo>().GetByIdWithSpec(opePeriodoSpec);


        if (opePeriodoToUpdate == null)
        {
            _logger.LogWarning($"No se encontro ope periodo con id: {request.Id}");
            throw new NotFoundException(nameof(OpePeriodo), request.Id);
        }

        // VALIDACIONES
        await _opePeriodoService.ValidarRegistrosDuplicados(request.Id, request.Nombre, request.FechaInicioFaseSalida, request.FechaFinFaseSalida, request.FechaInicioFaseRetorno, request.FechaFinFaseRetorno);
        // FIN VALIDACIONES


        _mapper.Map(request, opePeriodoToUpdate, typeof(UpdateOpePeriodoCommand), typeof(OpePeriodo));

        _unitOfWork.Repository<OpePeriodo>().UpdateEntity(opePeriodoToUpdate);
        await _unitOfWork.Complete();

        _logger.LogInformation($"Se actualizo correctamente el ope periodo con id: {request.Id}");
        _logger.LogInformation(nameof(UpdateOpePeriodoCommandHandler) + " - END");

        return Unit.Value;
    }
}
