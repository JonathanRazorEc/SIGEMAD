using DGPCE.Sigemad.Application.Contracts.Caching;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.CCAA.Vms;
using DGPCE.Sigemad.Application.Features.Provincias.Vms;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using System.Linq.Expressions;

namespace DGPCE.Sigemad.Application.Features.CCAA.Queries.GetComunidadesAutonomasList;

public class GetComunidadesAutonomasListQueryHandler : IRequestHandler<GetComunidadesAutonomasListQuery, IReadOnlyList<ComunidadesAutonomasVm>>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly ISIGEMemoryCache _cache;

    public GetComunidadesAutonomasListQueryHandler(
        IUnitOfWork unitOfWork,
        ISIGEMemoryCache cache
        )
    {
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task<IReadOnlyList<ComunidadesAutonomasVm>> Handle(GetComunidadesAutonomasListQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"GetComunidadesAutonomas";

        var includes = new List<Expression<Func<Ccaa, object>>>();
        includes.Add(c => c.Provincia);

        IReadOnlyList<ComunidadesAutonomasVm>? comunidadesAutonomasVm = await _cache.SetCacheIfEmptyAsync(
            cacheKey,
            async () =>
            {
                return await _unitOfWork.Repository<Ccaa>().GetAsync(
                    predicate: null,
                    includes: includes,
                    selector: c => new ComunidadesAutonomasVm
                    {
                        Id = c.Id,
                        Descripcion = c.Descripcion,
                        Provincia = c.Provincia.OrderBy(p => p.Descripcion).Select(Provincia => new ProvinciaSinMunicipiosVm
                        {
                            Id = Provincia.Id,
                            Descripcion = Provincia.Descripcion,
                            Huso = Provincia.Huso,
                            GeoPosicion = Provincia.GeoPosicion,
                            UtmX = Provincia.UtmX,
                            UtmY = Provincia.UtmY
                        }).ToList()
                    },
                    orderBy: q => q.OrderBy(c => c.Descripcion),
                    disableTracking: true
                    );
            },
            cancellationToken
            );

        return comunidadesAutonomasVm == null ? new List<ComunidadesAutonomasVm>() : comunidadesAutonomasVm;

    }
}
