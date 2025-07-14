using DGPCE.Sigemad.Application.Features.Menus.Vms;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Menus.Queries.GetOpeMenusList
{
    public class GetOpeMenusListQuery : IRequest<IReadOnlyList<MenuItemVm>>
    {
    }
}
