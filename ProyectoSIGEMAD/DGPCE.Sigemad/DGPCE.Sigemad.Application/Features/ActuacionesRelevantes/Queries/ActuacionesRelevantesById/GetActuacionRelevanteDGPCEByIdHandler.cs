using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.ActuacionesRelevantes;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.ActuacionesRelevantesDGPCE;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DGPCE.Sigemad.Application.Features.ActuacionesRelevantes.Queries.ActuacionesRelevantesById;
public class GetActuacionRelevanteDGPCEByIdHandler : IRequestHandler<GetActuacionRelevanteDGPCEById, ActuacionRelevanteDGPCEDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetActuacionRelevanteDGPCEByIdHandler> _logger;
    private readonly IMapper _mapper;

    public GetActuacionRelevanteDGPCEByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetActuacionRelevanteDGPCEByIdHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;

    }

    public async Task<ActuacionRelevanteDGPCEDto> Handle(GetActuacionRelevanteDGPCEById request, CancellationToken cancellationToken)
    {

        _logger.LogInformation($"{nameof(GetActuacionRelevanteDGPCEById)} - BEGIN");

        var actuacionesRelevantesSpec = new ActuacionRelevanteDGPCEActiveByIdSpecification(new ActuacionRelevanteDGPCESpecificationParams { Id = request.Id });
        var actuacionRelevante = await _unitOfWork.Repository<ActuacionRelevanteDGPCE>().GetByIdWithSpec(actuacionesRelevantesSpec);
        if (actuacionRelevante == null)
        {
            _logger.LogWarning($"No se encontro actuacionRelevante con id: {request.Id}");
            throw new NotFoundException(nameof(ActuacionRelevanteDGPCE), request.Id);
        }
        if (actuacionRelevante != null && actuacionRelevante.EmergenciaNacional != null && actuacionRelevante.EmergenciaNacional.Borrado)
        {
            actuacionRelevante.EmergenciaNacional = null;
        }

        _logger.LogInformation($"{nameof(GetActuacionRelevanteDGPCEById)} - END");
        return _mapper.Map<ActuacionRelevanteDGPCEDto>(actuacionRelevante);

    }
}
