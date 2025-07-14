using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;
public class Capacidad : EditableCatalogModel
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public bool Gestionado { get; set; }

    public TipoCapacidad? TipoCapacidad { get; set; }
    public int? IdTipoCapacidad { get; set; }

    public Entidad? Entidad { get; set; }
    public int? IdEntidad { get; set; }
}
