using DGPCE.Sigemad.Application.Dtos.EntidadesMenor;
using DGPCE.Sigemad.Application.Dtos.Municipios;
using DGPCE.Sigemad.Application.Dtos.Provincias;
using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Application.Dtos.AreasAfectadas;
public class AreaAfectadaDto
{
    public int Id { get; set; }

    public int idRegistro { get; set; }
    public DateTime FechaHora { get; set; }
    public ProvinciaDto Provincia { get; set; }
    public MunicipioDto Municipio { get; set; }
    public EntidadMenorDto? EntidadMenor { get; set; }
    public string Observaciones { get; set; }
    public Geometry GeoPosicion { get; set; }
    public decimal? SuperficieAfectadaHectarea { get; set; }
    public bool EsEliminable { get; set; }

    // NUEVOS INDICADORES
    // AUDITORÍA REAL
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaModificacion { get; set; }
    public bool EsNuevo { get; set; }
    public bool EsModificado { get; set; }
}
