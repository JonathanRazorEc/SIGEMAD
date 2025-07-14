using DGPCE.Sigemad.Application.Dtos.Municipios;
using DGPCE.Sigemad.Application.Dtos.Provincias;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Application.Dtos.CoordinacionCecopis;
public class CoordinacionCecopiDto
{
    public int Id { get; set; }

    public int tipoFormulario = (int)TipoFormularioEnum.CoordinacionPMA_CECOPI;
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public ProvinciaDto Provincia { get; set; }
    public MunicipioDto Municipio { get; set; }
    public string Lugar { get; set; }
    public string? Observaciones { get; set; }

    public decimal? UTM_X { get; set; }
    public decimal? UTM_Y { get; set; }
    public int? Huso { get; set; }
    public Geometry? GeoPosicion { get; set; }
    public Archivo? Archivo { get; set; }
    public bool EsEliminable { get; set; }
}
