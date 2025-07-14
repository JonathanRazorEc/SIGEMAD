using DGPCE.Sigemad.Application.Features.Municipios.Vms;
using DGPCE.Sigemad.Application.Features.Provincias.Vms;
using DGPCE.Sigemad.Domain.Common;
using DGPCE.Sigemad.Domain.Modelos;
using NetTopologySuite.Geometries;


namespace DGPCE.Sigemad.Application.Features.Incendios.Vms;

public class IncendioVm : BaseDomainModel<int>
{
    public int IdSuceso { get; set; }
    public int IdTerritorio { get; set; }
    public int IdClaseSuceso { get; set; }
    public int IdEstadoSuceso { get; set; }
    public int IdPais { get; set; }
    public bool EsLimitrofe { get; set; }
    public int? IdDistrito { get; set; }
    public int? IdMunicipioExtranjero { get; set; }
    public int? IdProvincia { get; set; }
    public int? IdMunicipio { get; set; }
    public DateTimeOffset FechaInicio { get; set; }
    public DateTimeOffset? FechaUltimoRegistro { get; set; }
    public string? Ubicacion { get; set; }
    public string Denominacion { get; set; } = null!;
    public string? NotaGeneral { get; set; }
    public string? RutaMapaRiesgo { get; set; }
    public decimal? UtmX { get; set; }
    public decimal? UtmY { get; set; }
    public int? Huso { get; set; }
    public Geometry? GeoPosicion { get; set; }

    public string? Sop { get; set; }

    public string? MaxSop { get; set; }
    public string? EstadoIncendio { get; set; }




    public virtual Suceso Suceso { get; set; } = null!;
    public virtual Territorio Territorio { get; set; } = null!;
    public virtual ClaseSuceso ClaseSuceso { get; set; } = null!;
    public virtual EstadoSuceso EstadoSuceso { get; set; } = null!;
    public virtual Pais Pais { get; set; } = null!;
    public virtual Distrito Distrito { get; set; } = null!;
    public virtual MunicipioExtranjero MunicipioExtranjero { get; set; } = null!;
    public virtual ProvinciaSinMunicipiosVm Provincia { get; set; } = null!;
    public virtual MunicipioSinIdProvinciaVm Municipio { get; set; } = null!;
}
