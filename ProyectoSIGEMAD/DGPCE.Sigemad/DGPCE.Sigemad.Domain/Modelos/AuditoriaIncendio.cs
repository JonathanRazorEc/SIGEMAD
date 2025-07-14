using DGPCE.Sigemad.Domain.Common;
using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations.Schema;

namespace DGPCE.Sigemad.Domain.Modelos;

public class Auditoria_Incendio : BaseDomainModel<int>
{
    public DateTime FechaRegistro { get; set; }
    public string TipoMovimiento { get; set; } = null!;

    public int IdIncendio { get; set; }
    public int IdSuceso { get; set; }
    public int IdTerritorio { get; set; }
    public int IdClaseSuceso { get; set; }
    public int? IdEstadoSuceso { get; set; }
    public int IdPais { get; set; }
    public bool EsLimitrofe { get; set; }
    public int? IdDistrito { get; set; }
    public int? IdMunicipioExtranjero { get; set; }
    public int? IdProvincia { get; set; }
    public int? IdMunicipio { get; set; }

    public DateTime FechaInicio { get; set; }
    public string? Ubicacion { get; set; }
    public string Denominacion { get; set; } = null!;
    public string? NotaGeneral { get; set; }
    public string? RutaMapaRiesgo { get; set; }

    public decimal? UTM_X { get; set; }
    public decimal? UTM_Y { get; set; }

    public int? Huso { get; set; }
    //public Geometry? GeoPosicion { get; set; }

    public bool Borrado { get; set; }

    public virtual Incendio Incendio { get; set; } = null!;
    
    public virtual AuditoriaSuceso Suceso { get; set; } = null!;
    public virtual Territorio Territorio { get; set; } = null!;
    public virtual ClaseSuceso ClaseSuceso { get; set; } = null!;
    public virtual EstadoSuceso? EstadoSuceso { get; set; }
    public virtual Pais Pais { get; set; } = null!;
    public virtual Distrito? Distrito { get; set; }
    public virtual MunicipioExtranjero? MunicipioExtranjero { get; set; }
    public virtual Provincia? Provincia { get; set; }
    public virtual Municipio? Municipio { get; set; }
    public virtual Parametro? Parametro { get; set; }

}

//    // Relaciones de navegación
//    public virtual Incendio Incendio { get; set; } = null!;
//    public virtual Suceso Suceso { get; set; } = null!;
//    public virtual Territorio Territorio { get; set; } = null!;
//    public virtual ClaseSuceso ClaseSuceso { get; set; } = null!;
//    public virtual EstadoSuceso? EstadoSuceso { get; set; }
//    public virtual Pais Pais { get; set; } = null!;
//    public virtual Distrito? Distrito { get; set; }
//    public virtual MunicipioExtranjero? MunicipioExtranjero { get; set; }
//    public virtual Provincia? Provincia { get; set; }
//    public virtual Municipio? Municipio { get; set; }
//}
