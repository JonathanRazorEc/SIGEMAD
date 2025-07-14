using DGPCE.Sigemad.Domain.Common;
using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Domain.Modelos;
public class CoordinacionPMA : BaseDomainModel<int>
{
    public int IdRegistro { get; set; }
    public virtual Registro Registro { get; set; } = null!;

    //public int IdTipoGestionDireccion { get; set; }
    //public virtual TipoGestionDireccion TipoGestionDireccion { get; set; }

    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }

    public int IdProvincia { get; set; }
    public Provincia Provincia { get; set; }

    public int IdMunicipio { get; set; }
    public Municipio Municipio { get; set; }

    public string Lugar { get; set; }
    public string? Observaciones { get; set; }
    public Geometry? GeoPosicion { get; set; }

    public Guid? IdArchivo { get; set; }
    public Archivo? Archivo { get; set; }
}
