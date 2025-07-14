using DGPCE.Sigemad.Application.Contracts.Caching;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.EntidadesMenores.Vms;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.EntidadesMenores.Quereis.GetEntidadMenorList;

public class GetEntidadMenorListQueryHandler : IRequestHandler<GetEntidadMenorListQuery, IReadOnlyList<EntidadMenorVm>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISIGEMemoryCache _cache;

    public GetEntidadMenorListQueryHandler(IUnitOfWork unitOfWork, ISIGEMemoryCache cache)
    {
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task<IReadOnlyList<EntidadMenorVm>> Handle(GetEntidadMenorListQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"Entidades_Menores";

        IReadOnlyList<EntidadMenorVm>? entidadMenorVms = await _cache.SetCacheIfEmptyAsync
            (
            cacheKey,
            async () =>
            {
                return await _unitOfWork.Repository<EntidadMenor>().GetAsync
                (
                    predicate: e => e.Borrado == false,
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