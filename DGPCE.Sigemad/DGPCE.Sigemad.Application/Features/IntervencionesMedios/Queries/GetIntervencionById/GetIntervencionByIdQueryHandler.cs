using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Intervenciones;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.IntervencionesMedios.Queries.GetIntervencionById;
public class GetIntervencionByIdQueryHandler : IRequestHandler<GetIntervencionByIdQuery, IntervencionMedio>
{
    private readonly ILogger<GetIntervencionByIdQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetIntervencionByIdQueryHandler(
        ILogger<GetIntervencionByIdQueryHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<IntervencionMedio> Handle(GetIntervencionByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(GetIntervencionByIdQueryHandler)} - BEGIN");

        var intervencionSpec = new IntervencionActiveByIdSpecification(request.Id);
        var intervencion = await _unitOfWork.Repository<IntervencionMedio>().GetByIdWithSpec(intervencionSpec);
        if (intervencion == null)
        {
            _logger.LogWarning($"No se encontro intervencion con id: {request.Id}");
            throw new NotFoundException(nameof(IntervencionMedio), request.Id);
        }

        _logger.LogInformation($"{nameof(GetIntervencionByIdQueryHandler)} - END");
        return intervencion;
    }
}
