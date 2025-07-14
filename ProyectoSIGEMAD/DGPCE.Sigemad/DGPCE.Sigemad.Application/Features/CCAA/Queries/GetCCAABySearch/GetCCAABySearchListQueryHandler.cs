using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Caching;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.CCAA.Vms;
using DGPCE.Sigemad.Application.Features.Provincias.Queries.GetProvinciasBySearch;
using DGPCE.Sigemad.Application.Features.Provincias.Vms;
using DGPCE.Sigemad.Application.Specifications.ComunidadesAutonomas;
using DGPCE.Sigemad.Application.Specifications.Provincias;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.CCAA.Queries.GetCCAABySearch;
public class GetCCAABySearchListQueryHandler : IRequestHandler<GetCCAABySearchListQuery, IReadOnlyList<ComunidadesAutonomasConPaisVm>>
{
    private readonly ILogger<GetCCAABySearchListQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISIGEMemoryCache _cache;
    private readonly IMapper _mapper;

    public GetCCAABySearchListQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetCCAABySearchListQueryHandler> logger,
        ISIGEMemoryCache cache,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _cache = cache;
        _mapper = mapper;
    }
    public async Task<IReadOnlyList<ComunidadesAutonomasConPaisVm>> Handle(GetCCAABySearchListQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"CCAA_BySearch_{request.IdPais}_{request.busqueda}";
        IReadOnlyList<ComunidadesAutonomasConPaisVm>? comundiadesAutonomasVms = new List<ComunidadesAutonomasConPaisVm>();


        var spec = new CCAABySearchSpecification(new CCAABySearchSpecificationParams
        {
            IdPais = request.IdPais,
            busqueda = request.busqueda
        });

        var comundiadesAutonomas = await _unitOfWork.Repository<Ccaa>().GetAllWithSpec(spec);

        comundiadesAutonomasVms = _mapper.Map<IReadOnlyList<ComunidadesAutonomasConPaisVm>>(comundiadesAutonomas);

        return comundiadesAutonomasVms;
    }
}