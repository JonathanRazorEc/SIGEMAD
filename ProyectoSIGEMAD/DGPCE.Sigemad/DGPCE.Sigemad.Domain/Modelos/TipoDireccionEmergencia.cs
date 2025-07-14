

using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;
public class TipoDireccionEmergencia: EditableCatalogModel
{
    public int Id { get; set; }
    public string Descripcion { get; set; } = null!;
    public int? IdTipoSuceso { get; set; }
    public virtual TipoSuceso? TipoSuceso { get; set; }
}
