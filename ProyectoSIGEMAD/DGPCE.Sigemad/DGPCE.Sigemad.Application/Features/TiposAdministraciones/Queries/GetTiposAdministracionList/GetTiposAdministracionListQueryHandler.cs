using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.TiposAdministraciones.Queries.GetTiposAdministracionList;
public class GetTiposAdministracionListQueryHandler : IRequestHandler<GetTiposAdministracionListQuery, IReadOnlyList<TipoAdministracionDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTiposAdministracionListQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<TipoAdministracionDto>> Handle(GetTiposAdministracionListQuery request, CancellationToken cancellationToken)
    {
        var lista = await _unitOfWork.Repository<TipoAdministracion>().GetAllNoTrackingAsync();
        return _mapper.Map<IReadOnlyList<TipoAdministracionDto>>(lista);
    }
}
