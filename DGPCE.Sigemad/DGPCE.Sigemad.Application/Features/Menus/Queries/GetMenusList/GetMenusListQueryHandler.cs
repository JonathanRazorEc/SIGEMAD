using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Menus.Vms;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Menus.Queries.GetMenusList;

public class GetMenusListQueryHandler : IRequestHandler<GetMenusListQuery, IReadOnlyList<MenuItemVm>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private const string Grupo = "Grupo";

    public GetMenusListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<MenuItemVm>> Handle(GetMenusListQuery request, CancellationToken cancellationToken)
    {
        var menus = await _unitOfWork.Repository<Menu>().GetAllNoTrackingAsync();
        IReadOnlyList<MenuItemVm> groupedMenus = menus
            .Where(m => m.Tipo.Equals(Grupo, StringComparison.CurrentCultureIgnoreCase))
            .OrderBy(m => m.NumOrden)
            .Select(grupo => MapMenuItemVm(grupo, menus))
            .ToList()
            .AsReadOnly();

        return groupedMenus;
    }

    private MenuItemVm MapMenuItemVm(Menu grupo, IReadOnlyList<Menu> allMenus)
    {
        return new MenuItemVm
        {
            Id = grupo.Id,
            Nombre = grupo.Nombre,
            Icono = grupo.Icono,
            ColorRgb = grupo.ColorRgb,
            Ruta = grupo.Ruta,
            SubItems = allMenus
                .Where(item => item.IdGrupo == grupo.Id)
                .OrderBy(item => item.NumOrden)
                .Select(item => MapMenuItemVm(item, allMenus))
                .ToList()
        };
    }
}
