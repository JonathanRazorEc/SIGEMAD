using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeLineasMaritimas;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeLineasMaritimas.Commands.DeleteOpeLineasMaritimas;

public class DeleteOpeLineaMaritimaCommandHandler : IRequestHandler<DeleteOpeLineaMaritimaCommand>
{
    private readonly ILogger<DeleteOpeLineaMaritimaCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteOpeLineaMaritimaCommandHandler(
        ILogger<DeleteOpeLineaMaritimaCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(DeleteOpeLineaMaritimaCommand request, CancellationToken cancellationToken)
    {
        var opeLineaMaritimaToDelete = await _unitOfWork.Repository<OpeLineaMaritima>().GetByIdAsync(request.Id);
        if (opeLineaMaritimaToDelete is null || opeLineaMaritimaToDelete.Borrado)
        {
            _logger.LogWarning($"La ope línea marítima con id:{request.Id}, no existe en la base de datos");
            throw new NotFoundException(nameof(OpeLineaMaritima), request.Id);
        }

        // VALIDACIONES
        // Comprobamos si hay datos de embarques diarios asociados a esa línea marítima
        var spec = new OpeDatosEmbarquesDiariosLineaMaritimaSpecification(request.Id);
        var opeDatosEmbarquesDiariosAsociados = await _unitOfWork.Repository<OpeDatoEmbarqueDiario>().GetAllWithSpec(spec);

        if (opeDatosEmbarquesDiariosAsociados.Any())
        {
            throw new InvalidOperationException("No se puede borrar la línea marítima porque tiene datos de embarque asociados.");
        }
        // FIN VALIDACIONES

        _unitOfWork.Repository<OpeLineaMaritima>().DeleteEntity(opeLineaMaritimaToDelete);

        await _unitOfWork.Complete();
        _logger.LogInformation($"La ope línea marítima con id: {request.Id}, se ha borrado con éxito");

        return Unit.Value;
    }
}
