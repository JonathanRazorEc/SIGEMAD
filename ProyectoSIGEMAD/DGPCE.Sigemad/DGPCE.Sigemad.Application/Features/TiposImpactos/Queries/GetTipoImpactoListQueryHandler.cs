
using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Caching;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Specifications.Impactos;
using DGPCE.Sigemad.Domain.Modelos;
using DGPCE.Sigemad.Domain;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DGPCE.Sigemad.Application.Features.TiposImpactos.Queries;
public class GetTipoImpactoListQueryHandler : IRequestHandler<GetTipoImpactoListQuery, IReadOnlyList<TipoImpacto>>
{
    private readonly ILogger<GetTipoImpactoListQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISIGEMemoryCache _cache;
    private readonly IMapper _mapper;

    public GetTipoImpactoListQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetTipoImpactoListQueryHandler> logger,
        ISIGEMemoryCache cache,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<IReadOnlyList<TipoImpacto>> Handle(GetTipoImpactoListQuery request, CancellationToken cancellationToken)
    {

        var impactosClasificados = await _unitOfWork.Repository<ImpactoClasificado>()
        .GetAllWithSpec(new TipoImpactoSpecifications(request.nuclear));


        var tipoImpactosUnicos = impactosClasificados
         .Where(ic => ic.TipoImpacto != null)
         .Select(ic => ic.TipoImpacto)
         .DistinctBy(ti => ti.Id)
         .ToList();

        return tipoImpactosUnicos;
    }
}
