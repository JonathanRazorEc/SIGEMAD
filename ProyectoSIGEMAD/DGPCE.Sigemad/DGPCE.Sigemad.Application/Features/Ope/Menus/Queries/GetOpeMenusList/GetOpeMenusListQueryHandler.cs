using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Menus.Vms;
using DGPCE.Sigemad.Domain.Modelos.Ope.Menu;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Menus.Queries.GetOpeMenusList;

public class GetOpeMenusListQueryHandler : IRequestHandler<GetOpeMenusListQuery, IReadOnlyList<MenuItemVm>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private const string Grupo = "Grupo";

    public GetOpeMenusListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<MenuItemVm>> Handle(GetOpeMenusListQuery request, CancellationToken cancellationToken)
    {
        var menus = await _unitOfWork.Repository<OpeMenu>().GetAllNoTrackingAsync();
        IReadOnlyList<MenuItemVm> groupedMenus = menus
            .Where(m => m.Tipo.Equals(Grupo, StringComparison.CurrentCultureIgnoreCase))
            .OrderBy(m => m.NumOrden)
            .Select(grupo => MapMenuItemVm(grupo, menus))
            .ToList()
            .AsReadOnly();

        return groupedMenus;
    }

    private MenuItemVm MapMenuItemVm(OpeMenu grupo, IReadOnlyList<OpeMenu> allMenus)
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
