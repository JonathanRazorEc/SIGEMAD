using DGPCE.Sigemad.Application.Contracts.Caching;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Municipios.Vms;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Municipios.Queries.GetMunicipioByIdProvincia;

public class GetMunicipioByIdProvinciaQueryHandler : IRequestHandler<GetMunicipioByIdProvinciaQuery, IReadOnlyList<MunicipioSinIdProvinciaVm>>
{
    private readonly ILogger<GetMunicipioByIdProvinciaQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISIGEMemoryCache _cache;

    public GetMunicipioByIdProvinciaQueryHandler(
        IUnitOfWork unitOfWork, 
        ILogger<GetMunicipioByIdProvinciaQueryHandler> logger,
        ISIGEMemoryCache cache)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _cache = cache;
    }

    public async Task<IReadOnlyList<MunicipioSinIdProvinciaVm>> Handle(GetMunicipioByIdProvinciaQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"Municipios_ByIdProvincia_{request.IdProvincia}";

        IReadOnlyList<MunicipioSinIdProvinciaVm>? municipioVms = await _cache.SetCacheIfEmptyAsync
            (
            cacheKey,
            async () =>
            {
                return await _unitOfWork.Repository<Municipio>().GetAsync
                (
                    predicate: e => e.IdProvincia == request.IdProvincia && e.Borrado == false,
                    selector: e => new MunicipioSinIdProvinciaVm
                    {
                        Id = e.Id,
                        Descripcion = e.Descripcion,
                        Huso = e.Huso,
                        GeoPosicion = e.GeoPosicion,
                        UtmX = e.UtmX,
                        UtmY = e.UtmY,
                    },
                    orderBy: e => e.OrderBy(e => e.Descripcion),
                    disableTracking: true
                );
            },
            cancellationToken
            );

        return municipioVms == null ? new List<MunicipioSinIdProvinciaVm>() : municipioVms;

    }
}
