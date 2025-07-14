using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Domain.Modelos;

public class Pais
{
    public Pais() { }
    public int Id { get; set; }

    public string Descripcion { get; set; } = null!;
    public bool Fronterizo { get; set; }

    public decimal? X { get; set; }

    public decimal? Y { get; set; }

    public Geometry? GeoPosicion { get; set; }
}
