using DGPCE.Sigemad.Application.Contracts.Caching;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.EntradasSalidas.Quereis.GetEntradaSalidaList;
public class GetEntradaSalidaListQueryHandler : IRequestHandler<GetEntradaSalidaListQuery, IReadOnlyList<EntradaSalida>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISIGEMemoryCache _cache;

    public GetEntradaSalidaListQueryHandler(IUnitOfWork unitOfWork, ISIGEMemoryCache cache)
    {
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task<IReadOnlyList<EntradaSalida>> Handle(GetEntradaSalidaListQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"Entradas_Salidas";

        var entradasSalidas = await _cache.SetCacheIfEmptyAsync
            (
            cacheKey,
            async () =>
            {
                return await _unitOfWork.Repository<EntradaSalida>().GetAllNoTrackingAsync();
            },
            cancellationToken
            );

        return entradasSalidas == null ? new List<EntradaSalida>() : entradasSalidas;
    }
}