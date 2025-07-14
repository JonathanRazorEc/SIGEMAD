using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;

public class ClaseSuceso : EditableCatalogModel
{
    public int Id { get; set; }

    public string? Descripcion { get; set; }
}
