using DGPCE.Sigemad.Application.Contracts.Caching;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.ClasesSucesos.Quereis.GetClaseSucesosList;


public class GetClaseSucesosListQueryHandler : IRequestHandler<GetClaseSucesosListQuery, IReadOnlyList<ClaseSuceso>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISIGEMemoryCache _cache;

    public GetClaseSucesosListQueryHandler(
        IUnitOfWork unitOfWork,
        ISIGEMemoryCache cache
        )
    {
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task<IReadOnlyList<ClaseSuceso>> Handle(GetClaseSucesosListQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"ClaseSucesos";

        var claseSucesos = await _cache.SetCacheIfEmptyAsync(
            cacheKey,
            async () =>
            {
                return await _unitOfWork.Repository<ClaseSuceso>().GetAllNoTrackingAsync();
            },
            cancellationToken
            );

        return claseSucesos == null ? new List<ClaseSuceso>() : claseSucesos;
    }
}


