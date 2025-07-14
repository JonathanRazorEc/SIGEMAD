using DGPCE.Sigemad.Application.Dtos.Common;
using DGPCE.Sigemad.Application.Dtos.CoordinacionCecopis;
using DGPCE.Sigemad.Application.Helpers;
using NetTopologySuite.Geometries;
using System;

namespace DGPCE.Sigemad.Application.Dtos.CoordinacionesPMA;
public class CreateOrUpdateCoordinacionPmaDto: IEquatable<CreateOrUpdateCoordinacionPmaDto>
{
    public int? Id { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public int IdProvincia { get; set; }
    public int IdMunicipio { get; set; }
    public string Lugar { get; set; } = string.Empty;

    public decimal? UTM_X { get; set; }
    public decimal? UTM_Y { get; set; }
    public int? Huso { get; set; }
    public Geometry? GeoPosicion { get; set; }
    public string? Observaciones { get; set; }

    public FileDto? Archivo { get; set; }

    public bool ActualizarFichero { get; set; }



    public bool Equals(CreateOrUpdateCoordinacionPmaDto? other)
    {
        if (other is null)
        {
            return false;
        }

        return Id == other.Id &&
            FechaInicio == other.FechaInicio &&
            FechaFin == other.FechaFin &&
            IdProvincia == other.IdProvincia &&
            IdMunicipio == other.IdMunicipio &&
            string.Equals(Lugar, other.Lugar) &&
            string.Equals(Observaciones, other.Observaciones) &&
            GeoJsonValidatorUtil.AreGeometriesEqual(GeoPosicion, other.GeoPosicion) &&
            (Archivo == null && other.Archivo == null ||
            Archivo != null && other.Archivo != null &&
            string.Equals(Archivo.FileName, other.Archivo.FileName));
    }

    public override bool Equals(object? obj)
    {
        if (obj is CreateOrUpdateCoordinacionPmaDto other)
        {
            return Equals(other);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            Id,
            FechaInicio,
            FechaFin ?? default,
            IdProvincia,
            IdMunicipio,
            Lugar ?? string.Empty,
            Observaciones ?? string.Empty,
            GeoPosicion ?? default);
    }
}
