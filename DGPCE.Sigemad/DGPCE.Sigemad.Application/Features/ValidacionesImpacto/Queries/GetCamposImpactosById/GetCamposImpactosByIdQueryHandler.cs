using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Caching;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.ValidacionesImpacto.Vms;
using DGPCE.Sigemad.Application.Specifications.ValidacionImpactosClasificados;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.ValidacionesImpacto.Queries.GetCamposImpactosById;
public class GetCamposImpactosByIdQueryHandler : IRequestHandler<GetCamposImpactosByIdQuery, IReadOnlyList<ValidacionImpactoClasificadoVm>>
{
    private readonly ILogger<GetCamposImpactosByIdQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISIGEMemoryCache _cache;
    private readonly IMapper _mapper;

    public GetCamposImpactosByIdQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetCamposImpactosByIdQueryHandler> logger,
        ISIGEMemoryCache cache,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<IReadOnlyList<ValidacionImpactoClasificadoVm>> Handle(GetCamposImpactosByIdQuery request, CancellationToken cancellationToken)
    {
        var idImpactoClasificado = await _unitOfWork.Repository<ImpactoClasificado>().GetByIdAsync(request.idImpactoClasificado);
        if (idImpactoClasificado is null)
        {
            _logger.LogWarning($"request.idImpactoClasificado: {request.idImpactoClasificado}, no encontrado");
            throw new NotFoundException(nameof(ImpactoClasificado), request.idImpactoClasificado);
        }

        var ValidacionImpactosClasificados = await _unitOfWork.Repository<ValidacionImpactoClasificado>()
        .GetAllWithSpec(new ValidacionImpactosClasificadosSpecifications(request.idImpactoClasificado));

        var validacionimpactosClasificadosVM = _mapper.Map<IReadOnlyList<ValidacionImpactoClasificado>, IReadOnlyList<ValidacionImpactoClasificadoVm>>(ValidacionImpactosClasificados);

        return validacionimpactosClasificadosVM;
    }
}
