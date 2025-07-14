using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Territorios.Vms;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Territorios.Queries.GetTerritoriosCrearList;
public class GetTerritorioCrearListQueryHandler : IRequestHandler<GetTerritorioCrearListQuery, IReadOnlyList<TerritorioVm>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTerritorioCrearListQueryHandler(
        IUnitOfWork unitOfWork
        )
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<TerritorioVm>> Handle(GetTerritorioCrearListQuery request, CancellationToken cancellationToken)
    {
        IReadOnlyList<TerritorioVm> territorios = await _unitOfWork.Repository<Territorio>().GetAsync(
            predicate: t => t.Comun == true,
            selector: t => new TerritorioVm
            {
                Id = t.Id,
                Descripcion = t.Descripcion,
            },
            disableTracking: true
            );

        return territorios;
    }
}
