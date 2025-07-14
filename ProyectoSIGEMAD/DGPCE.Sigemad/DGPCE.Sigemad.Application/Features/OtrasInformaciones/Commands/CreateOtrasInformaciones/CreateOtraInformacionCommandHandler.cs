using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.OtrasInformaciones.Commands.CreateOtrasInformaciones;
public class CreateOtraInformacionCommandHandler : IRequestHandler<CreateOtraInformacionCommand, CreateOtraInformacionResponse>
{
    private readonly ILogger<CreateOtraInformacionCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateOtraInformacionCommandHandler(
        IMapper mapper, 
        IUnitOfWork unitOfWork, 
        ILogger<CreateOtraInformacionCommandHandler> logger)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<CreateOtraInformacionResponse> Handle(CreateOtraInformacionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(CreateOtraInformacionCommand) + " - BEGIN");

        OtraInformacion otraInformacionEntity;

        if (request.IdOtraInformacion.HasValue)
        {
            otraInformacionEntity = await _unitOfWork.Repository<OtraInformacion>().GetByIdAsync((int)request.IdOtraInformacion);
            if (otraInformacionEntity == null)
            {
                _logger.LogWarning($"IdOtraInformacion: {request.IdOtraInformacion.Value}, no encontrado");
                throw new NotFoundException(nameof(OtraInformacion), request.IdOtraInformacion.Value);
            }
        }
        else
        { 
            var suceso = await _unitOfWork.Repository<Suceso>().GetByIdAsync((int)request.IdSuceso);
            if (suceso == null)
            {
                _logger.LogWarning($"request.IdSuceso: {request.IdSuceso}, no encontrado");
                throw new NotFoundException(nameof(Suceso), request.IdSuceso);
            }

            otraInformacionEntity = new OtraInformacion
            {
                IdSuceso = request.IdSuceso
            };

            _unitOfWork.Repository<OtraInformacion>().AddEntity(otraInformacionEntity);
        }

        var medio = await _unitOfWork.Repository<Medio>().GetByIdAsync(request.IdMedio);
        if (medio == null)
        {
            _logger.LogWarning($"IdMedio: {request.IdMedio}, no encontrado");
            throw new NotFoundException(nameof(Medio), request.IdMedio);
        }

        foreach (var idProcedenciaDestino in request.IdsProcedenciaDestino)
        {
            var procedenciaDestino = await _unitOfWork.Repository<ProcedenciaDestino>().GetByIdAsync(idProcedenciaDestino);
            if (procedenciaDestino == null)
            {
                _logger.LogWarning($"IdProcedenciaDestino: {idProcedenciaDestino}, no encontrado");
                throw new NotFoundException(nameof(ProcedenciaDestino), idProcedenciaDestino);
            }            
        }

        var procedenciasDestinos = new List<DetalleOtraInformacion_ProcedenciaDestino>();
        procedenciasDestinos = request.IdsProcedenciaDestino.Select(idProcedenciaDestino => new DetalleOtraInformacion_ProcedenciaDestino
        {
            IdProcedenciaDestino = idProcedenciaDestino
        }).ToList();        

        var detalleOtraInformacion = new DetalleOtraInformacion
        {            
            FechaHora = request.FechaHora,
            IdMedio = request.IdMedio,
            Asunto = request.Asunto,
            Observaciones = request.Observaciones,
            ProcedenciasDestinos = procedenciasDestinos
        };

        otraInformacionEntity.DetallesOtraInformacion.Add(detalleOtraInformacion);

        var result = await _unitOfWork.Complete();
        if (result == 0)
        {
            _logger.LogError("No se pudo insertar nueva otra información");
            throw new Exception("No se pudo insertar nueva otra información");
        }

        _logger.LogInformation($"El registro para Otra información {otraInformacionEntity.Id} fue creado correctamente");
        _logger.LogInformation(nameof(CreateOtraInformacionCommand) + " - END");
        return new CreateOtraInformacionResponse { Id = otraInformacionEntity.Id, DetalleOtraInformacionId = detalleOtraInformacion.Id };
    }
}
