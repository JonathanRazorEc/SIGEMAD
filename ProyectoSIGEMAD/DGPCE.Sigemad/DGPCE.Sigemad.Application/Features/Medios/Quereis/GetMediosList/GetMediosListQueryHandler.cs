
using DGPCE.Sigemad.Application.Contracts.Caching;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Medios.Quereis.GetMediosList;

public class GetMediosListQueryHandler : IRequestHandler<GetMediosListQuery, IReadOnlyList<Medio>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISIGEMemoryCache _cache;

    public GetMediosListQueryHandler(IUnitOfWork unitOfWork, ISIGEMemoryCache cache)
    {
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task<IReadOnlyList<Medio>> Handle(GetMediosListQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = "Medios";
        var medios = await _cache.SetCacheIfEmptyAsync
            (
            cacheKey,
            async () =>
            {
                return await _unitOfWork.Repository<Medio>().GetAllNoTrackingAsync();
            },
            cancellationToken
            );

        return medios == null ? new List<Medio>() : medios;
    }
}

