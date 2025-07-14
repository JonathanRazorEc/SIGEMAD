using DGPCE.Sigemad.Application.Features.Territorios.Vms;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Territorios.Queries.GetTerritoriosCrearList;
public class GetTerritorioCrearListQuery : IRequest<IReadOnlyList<TerritorioVm>>
{
}
