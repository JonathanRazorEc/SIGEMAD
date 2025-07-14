using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Caching;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Municipios.Queries.GetMunicipioByIdProvincia;
using DGPCE.Sigemad.Application.Features.Municipios.Queries.GetMunicipiosBySearch;
using DGPCE.Sigemad.Application.Features.Municipios.Vms;
using DGPCE.Sigemad.Application.Features.MunicipiosExtranjeros.Vms;
using DGPCE.Sigemad.Application.Specifications.Municipios;
using DGPCE.Sigemad.Application.Specifications.MunicipiosExtranjeros;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.MunicipiosExtranjeros.Queries.GetMunicipiosExtranjerosBySearch;
public class GetMunicipiosExtranjerosBySearchListQueryHandler : IRequestHandler<GetMunicipiosExtranjerosBySearchListQuery, IReadOnlyList<MunicipioExtranjeroVm>>
{
    private readonly ILogger<GetMunicipiosExtranjerosBySearchListQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISIGEMemoryCache _cache;
    private readonly IMapper _mapper;

    public GetMunicipiosExtranjerosBySearchListQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetMunicipiosExtranjerosBySearchListQueryHandler> logger,
        ISIGEMemoryCache cache,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _cache = cache;
        _mapper = mapper;
    }
    public async Task<IReadOnlyList<MunicipioExtranjeroVm>> Handle(GetMunicipiosExtranjerosBySearchListQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"Municipios_Extranjeros_BySearch_{request.IdDistrito}_{request.busqueda}";
        IReadOnlyList<MunicipioExtranjeroVm>? municipioExtranjerosVms = new List<MunicipioExtranjeroVm>();

        if (request.busqueda != null || request.IdDistrito != null)
        {
            var spec = new MunicipiosExtranjerosBySearchSpecification(new MunicipiosExtranjerosBySearchSpecificationParams
            {
                IdDistrito = request.IdDistrito,
                busqueda = request.busqueda,
                IdPais = request.IdPais
            });

            var municipiosExtranjeros = await _unitOfWork.Repository<MunicipioExtranjero>().GetAllWithSpec(spec);

            municipioExtranjerosVms = _mapper.Map<IReadOnlyList<MunicipioExtranjeroVm>>(municipiosExtranjeros);

        }

        return municipioExtranjerosVms;
    }
}
