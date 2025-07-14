using DGPCE.Sigemad.Application.Contracts.Caching;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.EstadosSucesos.Queries.GetEstadosSucesosList;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.EstadosEvolucion.Queries.GetEstadosSucesosList;

public class GetEstadosSucesosListQueryHandler : IRequestHandler<GetEstadosSucesosListQuery, IReadOnlyList<EstadoSuceso>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISIGEMemoryCache _cache;

    public GetEstadosSucesosListQueryHandler(IUnitOfWork unitOfWork, ISIGEMemoryCache cache)
    {
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task<IReadOnlyList<EstadoSuceso>> Handle(GetEstadosSucesosListQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = "Estados_Sucesos";

        var estadosSucesos = await _cache.SetCacheIfEmptyAsync
            (
            cacheKey,
            async () =>
            {
                return await _unitOfWork.Repository<EstadoSuceso>().GetAllNoTrackingAsync();
            },
            cancellationToken
            );

        return estadosSucesos == null ? new List<EstadoSuceso>() : estadosSucesos;
    }
}
