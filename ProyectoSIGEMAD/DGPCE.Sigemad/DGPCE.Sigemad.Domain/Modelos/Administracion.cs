using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;
public class Administracion : EditableCatalogModel
{
    public int Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }

    public TipoAdministracion TipoAdministracion { get; set; }
    public int IdTipoAdministracion { get; set; }
}
