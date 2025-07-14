using DGPCE.Sigemad.Application.Contracts.Caching;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Specifications.Paises;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Paises.Queries.GetPaisesList;

public class GetPaisesListQueryHandler : IRequestHandler<GetPaisesListQuery, IReadOnlyList<Pais>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISIGEMemoryCache _cache;

    public GetPaisesListQueryHandler(IUnitOfWork unitOfWork, ISIGEMemoryCache cache)
    {
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task<IReadOnlyList<Pais>> Handle(GetPaisesListQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"Paises_MostrarNacional_{request.MostrarNacional}";

        var specification = new PaisesSpecification(request.MostrarNacional);
        //var paises = await _unitOfWork.Repository<Pais>().GetAllWithSpec(specification);

        var paises = await _cache.SetCacheIfEmptyAsync(
            cacheKey,
            async () =>
            {
                return await _unitOfWork.Repository<Pais>().GetAllWithSpec(specification);
            },
            cancellationToken
        );

        return paises;
    }
}
