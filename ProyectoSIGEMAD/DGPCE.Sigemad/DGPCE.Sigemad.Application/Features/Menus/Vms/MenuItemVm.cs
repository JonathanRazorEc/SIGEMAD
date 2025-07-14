namespace DGPCE.Sigemad.Application.Features.Menus.Vms;

public class MenuItemVm
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string? Icono { get; set; }
    public string? ColorRgb { get; set; }
    public string? Ruta { get; set; }
    public List<MenuItemVm> SubItems { get; set; }
}
