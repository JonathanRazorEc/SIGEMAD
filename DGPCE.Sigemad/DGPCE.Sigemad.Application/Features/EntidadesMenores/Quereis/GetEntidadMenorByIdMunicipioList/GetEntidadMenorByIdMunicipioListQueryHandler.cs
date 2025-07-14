using DGPCE.Sigemad.Application.Contracts.Caching;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.EntidadesMenores.Vms;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DGPCE.Sigemad.Application.Features.EntidadesMenores.Quereis.GetEntidadMenorByIdMunicipioList;


public class GetEntidadMenorByIdMunicipioListQueryHandler : IRequestHandler<GetEntidadMenorByIdMunicipioListQuery, IReadOnlyList<EntidadMenorVm>>
{
    private readonly ILogger<GetEntidadMenorByIdMunicipioListQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISIGEMemoryCache _cache;

    public GetEntidadMenorByIdMunicipioListQueryHandler(
        IUnitOfWork unitOfWork, 
        ILogger<GetEntidadMenorByIdMunicipioListQueryHandler> logger,
        ISIGEMemoryCache cache
        )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _cache = cache;
    }

    public async Task<IReadOnlyList<EntidadMenorVm>> Handle(GetEntidadMenorByIdMunicipioListQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"EntidadMenor_ByIdMunicipio_{request.IdMunicipio}";

        IReadOnlyList<EntidadMenorVm>? entidadMenorVms = await _cache.SetCacheIfEmptyAsync
            (
            cacheKey,
            async () =>
            {
                return await _unitOfWork.Repository<EntidadMenor>().GetAsync
                (
                    predicate: e => e.IdMunicipio == request.IdMunicipio && e.Borrado == false,
                    selector: e => new EntidadMenorVm
                    {
                        Id = e.Id,
                        Descripcion = e.Descripcion,
                        IdMunicipio = e.Municipio.Id,
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

        return entidadMenorVms == null ? new List<EntidadMenorVm>() : entidadMenorVms;

    }
}
