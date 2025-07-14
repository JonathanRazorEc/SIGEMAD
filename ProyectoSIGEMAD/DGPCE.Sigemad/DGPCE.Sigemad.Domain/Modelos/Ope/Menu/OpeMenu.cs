namespace DGPCE.Sigemad.Domain.Modelos.Ope.Menu;

public class OpeMenu
{
    public OpeMenu() { }

    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Tipo { get; set; } = null!;

    public int IdGrupo { get; set; }

    public int NumOrden { get; set; }
    public string? Ruta { get; set; }

    public string? Icono { get; set; }

    public string? ColorRgb { get; set; }

}
