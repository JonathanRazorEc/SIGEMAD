using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;
public class TipoAdministracion : EditableCatalogModel
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Codigo { get; set; }
}
