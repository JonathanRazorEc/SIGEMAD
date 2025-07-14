using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.OtrasInformaciones;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.OtrasInformaciones.Commands.UpdateOtrasInformaciones;
public class UpdateDetalleOtraInformacionCommandHandler : IRequestHandler<UpdateDetalleOtraInformacionCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateDetalleOtraInformacionCommandHandler> _logger;

    public UpdateDetalleOtraInformacionCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateDetalleOtraInformacionCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateDetalleOtraInformacionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(UpdateDetalleOtraInformacionCommandHandler) + " - BEGIN");

        var specDetalle = new DetalleOtraInformacionByIdSpecification(request.Id);
        var detalle = await _unitOfWork.Repository<DetalleOtraInformacion>().GetByIdWithSpec(specDetalle);

        if (detalle == null)
        {
            _logger.LogWarning($"DetalleOtraInformacion con Id {request.Id} no encontrado");
            throw new NotFoundException(nameof(DetalleOtraInformacion), request.Id);
        }

        OtraInformacion otraInformacion;
        if (request.IdOtraInformacion.HasValue)
        {
            otraInformacion = await _unitOfWork.Repository<OtraInformacion>().GetByIdAsync(request.IdOtraInformacion.Value);
            if (otraInformacion == null)
            {
                _logger.LogWarning($"OtraInformacion con Id {request.IdOtraInformacion.Value} no encontrado");
                throw new NotFoundException(nameof(OtraInformacion), request.IdOtraInformacion.Value);
            }
        }
        else
        {
            var suceso = await _unitOfWork.Repository<Suceso>().GetByIdAsync(request.IdSuceso);
            if (suceso == null)
            {
                _logger.LogWarning($"request.IdSuceso: {request.IdSuceso}, no encontrado");
                throw new NotFoundException(nameof(Suceso), request.IdSuceso);
            }

            otraInformacion = new OtraInformacion 
            {
                IdSuceso = request.IdSuceso
            };

            _unitOfWork.Repository<OtraInformacion>().AddEntity(otraInformacion);
            await _unitOfWork.Complete();
        }

        var medio = await _unitOfWork.Repository<Medio>().GetByIdAsync(request.IdMedio);
        if (medio == null)
        {
            _logger.LogWarning($"IdMedio: {request.IdMedio}, no encontrado");
            throw new NotFoundException(nameof(Medio), request.IdMedio);
        }
        
        detalle.IdOtraInformacion = otraInformacion.Id;
        detalle.FechaHora = request.FechaHora;
        detalle.IdMedio = request.IdMedio;
        detalle.Asunto = request.Asunto;
        detalle.Observaciones = request.Observaciones;

        foreach (var pd in detalle.ProcedenciasDestinos)
        {
            pd.Borrado = true;
        }
        
        foreach (var idProcedenciaDestino in request.IdsProcedenciaDestino)
        {
            var procedenciaDestino = await _unitOfWork.Repository<ProcedenciaDestino>().GetByIdAsync(idProcedenciaDestino);
            if (procedenciaDestino == null)
            {
                _logger.LogWarning($"ProcedenciaDestino con Id {idProcedenciaDestino} no encontrado");
                throw new NotFoundException(nameof(ProcedenciaDestino), idProcedenciaDestino);
            }

            detalle.ProcedenciasDestinos.Add(new DetalleOtraInformacion_ProcedenciaDestino
            {
                IdProcedenciaDestino = idProcedenciaDestino
            });
        }
        
        _unitOfWork.Repository<DetalleOtraInformacion>().UpdateEntity(detalle);
        await _unitOfWork.Complete();

        _logger.LogInformation($"Se actualizo correctamente el detalle de otra información con id: {request.Id}");
        _logger.LogInformation(nameof(UpdateDetalleOtraInformacionCommandHandler) + " - END");

        return Unit.Value;
    }
}
