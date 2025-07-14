using DGPCE.Sigemad.Application.Contracts.Caching;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.ClasificacionMedios.Quereis.GetClasificacionMediosList;
public class GetClasificacionMediosListQueryHandler : IRequestHandler<GetClasificacionMediosListQuery, IReadOnlyList<ClasificacionMedio>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISIGEMemoryCache _cache;

    public GetClasificacionMediosListQueryHandler(IUnitOfWork unitOfWork, ISIGEMemoryCache cache)
    {
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task<IReadOnlyList<ClasificacionMedio>> Handle(GetClasificacionMediosListQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"Clasificacion_Medios";

        var clasificacionMedios = await _cache.SetCacheIfEmptyAsync
            (
            cacheKey,
            async () =>
            {
                return await _unitOfWork.Repository<ClasificacionMedio>().GetAllNoTrackingAsync();
            },
            cancellationToken
            );

        return clasificacionMedios == null ? new List<ClasificacionMedio>() : clasificacionMedios;
    }
}
