using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.Documentaciones.Vms;
using DGPCE.Sigemad.Application.Features.OtrasInformaciones.Commands.DeleteOtrasInformaciones;
using DGPCE.Sigemad.Application.Features.OtrasInformaciones.Queries.GetOtrasInformacionesById;
using DGPCE.Sigemad.Application.Specifications.OtrasInformaciones;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.OtrasInformaciones.Commands.DeleteOtraInformacion;
public class DeleteOtraInformationQueryHandler : IRequestHandler<DeleteOtraInformationQuery>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteDetalleOtraInformacionCommandHandler> _logger;

    public DeleteOtraInformationQueryHandler(IUnitOfWork unitOfWork, ILogger<DeleteDetalleOtraInformacionCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteOtraInformationQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(GetOtraInformacionByIdQueryHandler)} - BEGIN");

        var spec = new OtraInformacionActiveByIdSpecification(request.Id);
        var otraInformacion = await _unitOfWork.Repository<OtraInformacion>().GetByIdWithSpec(spec);

        if (otraInformacion is null)
        {
            _logger.LogWarning($"No se encontró otra información con id: {request.Id}");
            throw new NotFoundException(nameof(OtraInformacion), request.Id);
        }

        // Verificar si es el último registro por fecha de creación
        var ultimoRegistro = await _unitOfWork.Repository<OtraInformacion>()
                .GetAsync(d => d.FechaCreacion > otraInformacion.FechaCreacion && d.Id != otraInformacion.Id  && !d.Borrado);

        if (ultimoRegistro.Any())
        {
            // No es el último registro
            _logger.LogWarning($"El registro: {request.Id} de otra información no es el último");
            throw new LastRegistrationException(nameof(OtraInformacion), request.Id);
        }


        await _unitOfWork.Repository<OtraInformacion>().DeleteAsync(otraInformacion);

        _logger.LogInformation($"{nameof(GetOtraInformacionByIdQueryHandler)} - END");
        return Unit.Value;
    }
}
