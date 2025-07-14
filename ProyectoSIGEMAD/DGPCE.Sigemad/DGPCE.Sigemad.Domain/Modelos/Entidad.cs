using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;
public class Entidad : EditableCatalogModel
{
    public int Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }

    public Organismo Organismo { get; set; }
    public int IdOrganismo { get; set; }
}
