using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Ope.Datos.OpeDatosEmbarquesDiarios;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosEmbarquesDiarios.Vms;
using DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeDatosEmbarquesDiarios;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosEmbarquesDiarios.Commands.UpdateOpeDatosEmbarquesDiarios;

public class UpdateOpeDatoEmbarqueDiarioCommandHandler : IRequestHandler<UpdateOpeDatoEmbarqueDiarioCommand, OpeDatoEmbarqueDiarioVm>
{
    private readonly ILogger<UpdateOpeDatoEmbarqueDiarioCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IOpeDatoEmbarqueDiarioService _opeDatoEmbarqueDiarioService;

    public UpdateOpeDatoEmbarqueDiarioCommandHandler(
        ILogger<UpdateOpeDatoEmbarqueDiarioCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IOpeDatoEmbarqueDiarioService opeDatoEmbarqueDiarioService
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _opeDatoEmbarqueDiarioService = opeDatoEmbarqueDiarioService;
    }

    public async Task<OpeDatoEmbarqueDiarioVm> Handle(UpdateOpeDatoEmbarqueDiarioCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(UpdateOpeDatoEmbarqueDiarioCommandHandler) + " - BEGIN");

        var opeDatoEmbarqueDiarioSpec = new OpeDatoEmbarqueDiarioActiveByIdSpecification(request.Id);
        var opeDatoEmbarqueDiarioToUpdate = await _unitOfWork.Repository<OpeDatoEmbarqueDiario>().GetByIdWithSpec(opeDatoEmbarqueDiarioSpec);


        if (opeDatoEmbarqueDiarioToUpdate == null)
        {
            _logger.LogWarning($"No se encontro ope dato de embarque diario con id: {request.Id}");
            throw new NotFoundException(nameof(OpeDatoEmbarqueDiario), request.Id);
        }

        // VALIDACIONES
        await _opeDatoEmbarqueDiarioService.ValidarRegistrosDuplicados(request.Id, request.IdOpeLineaMaritima, request.Fecha);
        // FIN VALIDACIONES

       _mapper.Map(request, opeDatoEmbarqueDiarioToUpdate, typeof(UpdateOpeDatoEmbarqueDiarioCommand), typeof(OpeDatoEmbarqueDiario));

        _unitOfWork.Repository<OpeDatoEmbarqueDiario>().UpdateEntity(opeDatoEmbarqueDiarioToUpdate);
        await _unitOfWork.Complete();

        _logger.LogInformation($"Se actualizo correctamente el ope dato de embarque diario con id: {request.Id}");
        _logger.LogInformation(nameof(UpdateOpeDatoEmbarqueDiarioCommandHandler) + " - END");

        //return Unit.Value;
        // Mapear la entidad actualizada a DTO para devolverla
        var opeDatoEmbarqueDiarioVm = _mapper.Map<OpeDatoEmbarqueDiarioVm>(opeDatoEmbarqueDiarioToUpdate);

        return opeDatoEmbarqueDiarioVm;
    }
}
