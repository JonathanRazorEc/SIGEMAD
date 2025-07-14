using DGPCE.Sigemad.Domain.Enums;
using MediatR;
using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Application.Features.Incendios.Commands.CreateIncendios;

public class CreateIncendioCommand : IRequest<CreateIncendioResponse>
{
    public TipoTerritorio IdTerritorio { get; set; }
    public int IdClaseSuceso { get; set; }
    public int IdEstadoSuceso { get; set; }
    public DateTimeOffset FechaInicio { get; set; }
    public string Denominacion { get; set; }
    public string? NotaGeneral { get; set; }
    public string? RutaMapaRiesgo { get; set; }
    public Geometry GeoPosicion { get; set; }

    public int? IdProvincia { get; set; }
    public int? IdMunicipio { get; set; }

    public int? IdPais { get; set; }
    //public int? IdDistrito { get; set; }
    public int? IdMunicipioExtranjero { get; set; }
    public string? Ubicacion { get; set; }
    public bool EsLimitrofe { get; set; }
}
