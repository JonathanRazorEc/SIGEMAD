
using DGPCE.Sigemad.Domain.Common;
using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Domain.Modelos;
public class AreaAfectada : BaseDomainModel<int>
{
    public int IdRegistro { get; set; }
    public DateTime FechaHora { get; set; }
    public int IdProvincia { get; set; }
    public int IdMunicipio { get; set; }
    public int? IdEntidadMenor { get; set; }
    public Geometry? GeoPosicion { get; set; }
    public string? Observaciones { get; set; }

    public decimal? SuperficieAfectadaHectarea { get; set; }

    public virtual Municipio Municipio { get; set; } = null!;
    public virtual Registro Registro { get; set; } = null!;
    public virtual Provincia Provincia { get; set; } = null!;
    public virtual EntidadMenor? EntidadMenor { get; set; }

}
