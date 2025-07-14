using DGPCE.Sigemad.Application.Contracts.Caching;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Specifications.Paises;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Paises.Queries.GetPaisesBySearch;

public class GetPaisesBySearchQueryHandler : IRequestHandler<GetPaisesBySearchQuery, IReadOnlyList<Pais>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISIGEMemoryCache _cache;

    public GetPaisesBySearchQueryHandler(IUnitOfWork unitOfWork, ISIGEMemoryCache cache)
    {
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task<IReadOnlyList<Pais>> Handle(GetPaisesBySearchQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"GetPaisesBySearchQueryHandler_{request.MostrarNacional}";

        var spec = new PaisesBySearchSpecifications(new PaisesBySearchSpecificationsParams
        {
            mostrarNacional = request.MostrarNacional,
            busqueda = request.busqueda
        });

        return await _unitOfWork.Repository<Pais>().GetAllWithSpec(spec);



    }
}
