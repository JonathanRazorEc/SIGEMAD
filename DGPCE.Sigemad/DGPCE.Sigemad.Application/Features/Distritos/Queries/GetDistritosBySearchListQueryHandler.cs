using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Caching;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Distritos.Vms;
using DGPCE.Sigemad.Application.Specifications.Distritos;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Distritos.Queries;
public class GetDistritosBySearchListQueryHandler : IRequestHandler<GetDistritosBySearchListQuery, IReadOnlyList<DistritoVm>>
{
    private readonly ILogger<GetDistritosBySearchListQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISIGEMemoryCache _cache;
    private readonly IMapper _mapper;

    public GetDistritosBySearchListQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetDistritosBySearchListQueryHandler> logger,
        ISIGEMemoryCache cache,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _cache = cache;
        _mapper = mapper;
    }
    public async Task<IReadOnlyList<DistritoVm>> Handle(GetDistritosBySearchListQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"Distritos_BySearch_{request.IdPais}_{request.busqueda}";
        IReadOnlyList<DistritoVm>? distritosVms = new List<DistritoVm>();


        var spec = new DistritosBySearchSpecification(new DistritosBySearchSpecificationParams
        {
            IdPais = request.IdPais,
            busqueda = request.busqueda
        });

        var distritos = await _unitOfWork.Repository<Distrito>().GetAllWithSpec(spec);

        distritosVms = _mapper.Map<IReadOnlyList<DistritoVm>>(distritos);

        return distritosVms;
    }
}
