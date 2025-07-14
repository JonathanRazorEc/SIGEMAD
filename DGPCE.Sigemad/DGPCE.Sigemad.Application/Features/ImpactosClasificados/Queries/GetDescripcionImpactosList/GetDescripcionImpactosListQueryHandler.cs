using DGPCE.Sigemad.Application.Contracts.Caching;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.ImpactosClasificados.Vms;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.ImpactosClasificados.Queries.GetDescripcionImpactosList;
public class GetDescripcionImpactosListQueryHandler : IRequestHandler<GetDescripcionImpactosListQuery, IReadOnlyList<ImpactoVm>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISIGEMemoryCache _cache;

    public GetDescripcionImpactosListQueryHandler(IUnitOfWork unitOfWork, ISIGEMemoryCache cache)
    {
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task<IReadOnlyList<ImpactoVm>> Handle(GetDescripcionImpactosListQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"DescripcionImpactos_tipo_{request.Tipo}_grupo_{request.Grupo}";

        //IReadOnlyList<ImpactoVm>? impactoVms = await _cache.SetCacheIfEmptyAsync
        //    (
        //    cacheKey,
        //    async () =>
        //    {
        //        //return await _unitOfWork.Repository<ImpactoClasificado>().GetAsync(
        //        //    predicate: i => (
        //        //        (string.IsNullOrEmpty(request.Tipo) || i.TipoImpacto.Contains(request.Tipo)) &&
        //        //        (string.IsNullOrEmpty(request.Grupo) || i.GrupoImpacto.Contains(request.Grupo))
        //        //        ),
        //        //    selector: i => new ImpactoVm
        //        //    {
        //        //        Id = i.Id,
        //        //        Descripcion = i.Descripcion,
        //        //    },
        //        //    disableTracking: true
        //        //    );
        //    },
        //    cancellationToken
        //    );

        return new List<ImpactoVm>() ;
    }
}
