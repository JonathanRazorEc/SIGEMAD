using DGPCE.Sigemad.Application.Contracts.Caching;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.ComparativaFechas.Quereis.GetComparativaFechasList;

public class GetComparativaFechasListQueryHandler : IRequestHandler<GetComparativaFechasListQuery, IReadOnlyList<ComparativaFecha>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISIGEMemoryCache _cache;

    public GetComparativaFechasListQueryHandler(IUnitOfWork unitOfWork, ISIGEMemoryCache cache)
    {
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task<IReadOnlyList<ComparativaFecha>> Handle(GetComparativaFechasListQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"Comparativa_Fechas";

        var comparativaFechas = await _cache.SetCacheIfEmptyAsync
            (
            cacheKey,
            async () =>
            {
                return await _unitOfWork.Repository<ComparativaFecha>().GetAllNoTrackingAsync();
            },
            cancellationToken
            );

        return comparativaFechas == null ? new List<ComparativaFecha>() : comparativaFechas;
    }
}


