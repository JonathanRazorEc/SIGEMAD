using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Caching;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Provincias.Vms;
using DGPCE.Sigemad.Application.Specifications.Provincias;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DGPCE.Sigemad.Application.Features.Provincias.Queries.GetProvinciasBySearch;

public class GetProvinciasBySearchListQueryHandler : IRequestHandler<GetProvinciasBySearchListQuery, IReadOnlyList<ProvinciasConCCAAVm>>
{
    private readonly ILogger<GetProvinciasBySearchListQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISIGEMemoryCache _cache;
    private readonly IMapper _mapper;

    public GetProvinciasBySearchListQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetProvinciasBySearchListQueryHandler> logger,
        ISIGEMemoryCache cache,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _cache = cache;
        _mapper = mapper;
    }
    public async Task<IReadOnlyList<ProvinciasConCCAAVm>> Handle(GetProvinciasBySearchListQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"Provincias_BySearch_{request.idComunidadAutonoma}_{request.busqueda}";
        IReadOnlyList<ProvinciasConCCAAVm>? provinciaVms = new List<ProvinciasConCCAAVm>();

    
         var spec = new ProvinciasBySearchSpecification(new ProvinciaSpecificationParams
         {
              IdCcaa = request.idComunidadAutonoma,
              busqueda = request.busqueda
          });

         var provincias = await _unitOfWork.Repository<Provincia>().GetAllWithSpec(spec);

         provinciaVms = _mapper.Map<IReadOnlyList<ProvinciasConCCAAVm>>(provincias);

        return provinciaVms;
    }
}
