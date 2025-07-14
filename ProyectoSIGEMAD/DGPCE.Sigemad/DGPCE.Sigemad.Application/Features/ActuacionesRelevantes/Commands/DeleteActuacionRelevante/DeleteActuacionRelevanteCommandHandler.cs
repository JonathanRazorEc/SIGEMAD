using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.OtrasInformaciones.Commands.DeleteOtrasInformaciones;
using DGPCE.Sigemad.Application.Features.OtrasInformaciones.Queries.GetOtrasInformacionesById;
using DGPCE.Sigemad.Application.Specifications.ActuacionesRelevantesDGPCE;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.ActuacionesRelevantes.Commands.DeleteActuacionRelevante;
public class DeleteActuacionRelevanteCommandHandler : IRequestHandler<DeleteActuacionRelevanteCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteDetalleOtraInformacionCommandHandler> _logger;

    public DeleteActuacionRelevanteCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteDetalleOtraInformacionCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteActuacionRelevanteCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(GetOtraInformacionByIdQueryHandler)} - BEGIN");

       // var actuacionesRelevantesSpec = new ActuacionRelevanteDGPCEActiveByIdSpecification(new ActuacionRelevanteDGPCESpecificationParams { Id = request.Id });
        var actuacionRelevante =  _unitOfWork.Repository<ActuacionRelevanteDGPCE>().GetByIdAsync(request.Id).Result;

        if (actuacionRelevante is null)
        {
            _logger.LogWarning($"No se encontró actuacion Relevante con id: {request.Id}");
            throw new NotFoundException(nameof(ActuacionRelevanteDGPCE), request.Id);
        }

        // Verificar si es el último registro por fecha de creación
        var ultimoRegistro = await _unitOfWork.Repository<ActuacionRelevanteDGPCE>()
                .GetAsync(d => d.FechaCreacion > actuacionRelevante.FechaCreacion && d.Id != actuacionRelevante.Id && !d.Borrado);

        if (ultimoRegistro.Any())
        {
            // No es el último registro
            _logger.LogWarning($"El registro: {request.Id} de actuacion relevante no es el último");
            throw new LastRegistrationException(nameof(ActuacionRelevanteDGPCE), request.Id);
        }


        await _unitOfWork.Repository<ActuacionRelevanteDGPCE>().DeleteAsync(actuacionRelevante);

        _logger.LogInformation($"{nameof(GetOtraInformacionByIdQueryHandler)} - END");
        return Unit.Value;
    }

}
