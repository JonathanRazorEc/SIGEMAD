using DGPCE.Sigemad.Application.Dtos.Common;
using DGPCE.Sigemad.Application.Helpers;
using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Application.Dtos.CoordinacionCecopis;
public class CreateOrUpdateCoordinacionCecopiDto : IEquatable<CreateOrUpdateCoordinacionCecopiDto>
{
    public int? Id { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public int IdProvincia { get; set; }
    public int IdMunicipio { get; set; }
    public string Lugar { get; set; }
    public string? Observaciones { get; set; }

    // Campos UTM agregados
    public decimal? UTM_X { get; set; }
    public decimal? UTM_Y { get; set; }
    public int? Huso { get; set; }
    public Geometry? GeoPosicion { get; set; }

    public FileDto? Archivo { get; set; }
    public bool ActualizarFichero { get; set; }

    public bool Equals(CreateOrUpdateCoordinacionCecopiDto? other)
    {
        if (other is null) return false;

        return Id == other.Id &&
            FechaInicio == other.FechaInicio &&
            FechaFin == other.FechaFin &&
            IdProvincia == other.IdProvincia &&
            IdMunicipio == other.IdMunicipio &&
            string.Equals(Lugar, other.Lugar) &&
            string.Equals(Observaciones, other.Observaciones) &&
            UTM_X == other.UTM_X &&
            UTM_Y == other.UTM_Y &&
            Huso == other.Huso &&
            GeoJsonValidatorUtil.AreGeometriesEqual(GeoPosicion, other.GeoPosicion) &&
            (Archivo == null && other.Archivo == null ||
            Archivo != null && other.Archivo != null &&
            string.Equals(Archivo.FileName, other.Archivo.FileName));
    }


    public override int GetHashCode()
    {
        return HashCode.Combine(
            HashCode.Combine(
                Id,
                FechaInicio,
                FechaFin ?? default,
                IdProvincia,
                IdMunicipio,
                Lugar ?? string.Empty,
                Observaciones ?? string.Empty
            ),
            HashCode.Combine(
                UTM_X ?? 0,
                UTM_Y ?? 0,
                Huso ?? 0,
                GeoPosicion ?? default
            )
        );
    }


}