using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Territorios.Vms;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Territorios.Queries.GetTerritoriosList;
public class GetTerritoriosListQueryHandler : IRequestHandler<GetTerritoriosListQuery, IReadOnlyList<TerritorioVm>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTerritoriosListQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<TerritorioVm>> Handle(GetTerritoriosListQuery request, CancellationToken cancellationToken)
    {
        var territorios = await _unitOfWork.Repository<Territorio>().GetAllNoTrackingAsync();
        return _mapper.Map<IReadOnlyList<TerritorioVm>>(territorios);
    }
}
