using DGPCE.Sigemad.Application.Contracts.Caching;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Specifications.EstadosIncendios;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.EstadosIncendio.Queries.GetEstadosIncendioList;

public class GetEstadosIncendioListQueryHandler : IRequestHandler<GetEstadosIncendioListQuery, IReadOnlyList<EstadoIncendio>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISIGEMemoryCache _cache;

    public GetEstadosIncendioListQueryHandler(IUnitOfWork unitOfWork, ISIGEMemoryCache cache)
    {
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task<IReadOnlyList<EstadoIncendio>> Handle(GetEstadosIncendioListQuery request, CancellationToken cancellationToken)
    {

        var estadosIncendio = await _unitOfWork.Repository<EstadoIncendio>().GetAllWithSpec(new EstadosIncendiosSpecifications());
        return estadosIncendio == null ? new List<EstadoIncendio>() : estadosIncendio;
    }
}
