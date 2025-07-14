using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;

public class TipoDanio : EditableCatalogModel
{
    public int Id { get; set; }

    public string Descripcion { get; set; }
}
