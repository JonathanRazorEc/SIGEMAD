using DGPCE.Sigemad.Application.Helpers;
using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Application.Dtos.AreasAfectadas;
public class CreateOrUpdateAreaAfectadaDto : IEquatable<CreateOrUpdateAreaAfectadaDto>
{
    public int? Id { get; set; }
    public DateTime FechaHora { get; set; }
    public int IdProvincia { get; set; }
    public int IdMunicipio { get; set; }
    public int? IdEntidadMenor { get; set; }
    public string? Observaciones { get; set; }
    public Geometry? GeoPosicion { get; set; }
    public decimal? SuperficieAfectadaHectarea { get; set; }

    public bool Equals(CreateOrUpdateAreaAfectadaDto? other)
    {
        if (other is null)
        {
            return false;
        }

        return Id == other.Id &&
            FechaHora == other.FechaHora &&
            IdProvincia == other.IdProvincia &&
            IdMunicipio == other.IdMunicipio &&
            IdEntidadMenor == other.IdEntidadMenor &&
            SuperficieAfectadaHectarea == other.SuperficieAfectadaHectarea &&
            string.Equals(Observaciones, other.Observaciones) &&
            GeoJsonValidatorUtil.AreGeometriesEqual(GeoPosicion, other.GeoPosicion);
    }

    public override bool Equals(object? obj)
    {
        if (obj is CreateOrUpdateAreaAfectadaDto other)
        {
            return Equals(other);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            Id,
            FechaHora,
            IdProvincia,
            IdMunicipio,
            SuperficieAfectadaHectarea,
            IdEntidadMenor ?? default,
            Observaciones ?? string.Empty,
            GeoPosicion ?? default);
    }
}
