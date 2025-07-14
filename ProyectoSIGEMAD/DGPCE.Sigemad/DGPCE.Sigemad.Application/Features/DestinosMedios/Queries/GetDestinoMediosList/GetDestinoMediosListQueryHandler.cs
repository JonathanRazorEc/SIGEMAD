using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Caching;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.DestinosMedios.Queries.GetDestinoMediosList;
public class GetDestinoMediosListQueryHandler : IRequestHandler<GetDestinoMediosListQuery, IReadOnlyList<DestinoMedioDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ISIGEMemoryCache _cache;

    public GetDestinoMediosListQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ISIGEMemoryCache cache
        )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<IReadOnlyList<DestinoMedioDto>> Handle(GetDestinoMediosListQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"Destino_Medio";

        var lista = await _cache.SetCacheIfEmptyAsync
            (
            cacheKey,
            async () =>
            {
                return await _unitOfWork.Repository<DestinoMedio>().GetAllNoTrackingAsync();
            },
            cancellationToken
            );

        return _mapper.Map<IReadOnlyList<DestinoMedioDto>>(lista);
    }
}
