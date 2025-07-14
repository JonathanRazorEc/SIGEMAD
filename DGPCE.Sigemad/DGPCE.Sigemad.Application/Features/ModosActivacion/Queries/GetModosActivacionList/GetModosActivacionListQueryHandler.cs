using DGPCE.Sigemad.Application.Contracts.Caching;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.ModosActivacion.Queries.GetModosActivacionList;

public class GetModosActivacionListQueryHandler : IRequestHandler<GetModosActivacionListQuery, IReadOnlyList<ModoActivacion>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISIGEMemoryCache _cache;

    public GetModosActivacionListQueryHandler(IUnitOfWork unitOfWork, ISIGEMemoryCache cache)
    {
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task<IReadOnlyList<ModoActivacion>> Handle(GetModosActivacionListQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = "ModosActivacion";

        var modosActivacion = await _cache.SetCacheIfEmptyAsync
            (
            cacheKey,
            async () =>
            {
                return await _unitOfWork.Repository<ModoActivacion>().GetAllNoTrackingAsync();
            },
            cancellationToken
            );

        return modosActivacion == null ? new List<ModoActivacion>() : modosActivacion;
    }
}
