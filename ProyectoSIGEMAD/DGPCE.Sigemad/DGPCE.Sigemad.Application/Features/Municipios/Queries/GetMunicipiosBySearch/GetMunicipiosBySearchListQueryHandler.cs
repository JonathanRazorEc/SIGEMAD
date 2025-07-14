using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Caching;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Municipios.Queries.GetMunicipioByIdProvincia;
using DGPCE.Sigemad.Application.Features.Municipios.Vms;
using DGPCE.Sigemad.Application.Specifications.Municipios;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Municipios.Queries.GetMunicipiosBySearch;

public class GetMunicipiosBySearchListQueryHandler : IRequestHandler<GetMunicipiosBySearchListQuery, IReadOnlyList<MunicipiosConProvinciaVM>>
{
    private readonly ILogger<GetMunicipioByIdProvinciaQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISIGEMemoryCache _cache;
    private readonly IMapper _mapper;

    public GetMunicipiosBySearchListQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetMunicipioByIdProvinciaQueryHandler> logger,
        ISIGEMemoryCache cache,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _cache = cache;
        _mapper = mapper;
    }
    public async Task<IReadOnlyList<MunicipiosConProvinciaVM>> Handle(GetMunicipiosBySearchListQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"Municipios_BySearch_{request.IdProvincia}_{request.busqueda}";
        IReadOnlyList<MunicipiosConProvinciaVM>? municipioVms = new List<MunicipiosConProvinciaVM>();

        if (request.busqueda != null || request.IdProvincia != null)
        {
         var spec = new MunicipiosBySearchSpecification(new MunicipiosSpecificationParams
         {
               IdProvincia = request.IdProvincia,
               busqueda = request.busqueda
             });

         var municipios = await _unitOfWork.Repository<Municipio>().GetAllWithSpec(spec);

          municipioVms = _mapper.Map<IReadOnlyList<MunicipiosConProvinciaVM>>(municipios);
        
        }

        return municipioVms;
    }
}
