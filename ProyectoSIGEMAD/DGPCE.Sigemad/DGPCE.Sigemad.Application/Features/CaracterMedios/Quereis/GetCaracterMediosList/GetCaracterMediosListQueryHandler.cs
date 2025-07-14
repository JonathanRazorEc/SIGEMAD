using DGPCE.Sigemad.Application.Contracts.Caching;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.CaracterMedios;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.CaracterMedios.Quereis.GetCaracterMediosList;
public class GetCaracterMediosListQueryHandler : IRequestHandler<GetCaracterMediosListQuery, IReadOnlyList<CaracterMedioDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISIGEMemoryCache _cache;

    public GetCaracterMediosListQueryHandler(IUnitOfWork unitOfWork, ISIGEMemoryCache cache)
    {
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task<IReadOnlyList<CaracterMedioDto>> Handle(GetCaracterMediosListQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"CaracterMedios";

        IReadOnlyList<CaracterMedioDto> caracterMediosDto = await _cache.SetCacheIfEmptyAsync(
            cacheKey,
            async () =>
            {
                return await _unitOfWork.Repository<CaracterMedio>().GetAsync
                    (
                        predicate: s => s.Obsoleto == false,
                        selector: s => new CaracterMedioDto
                        {
                            Id = s.Id,
                            Descripcion = s.Descripcion,
                        },
                        disableTracking: true
                    );
            },
            cancellationToken
        );

        return caracterMediosDto;

    }
}

