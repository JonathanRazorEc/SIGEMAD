using DGPCE.Sigemad.Domain.Constracts;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

namespace DGPCE.Sigemad.Infrastructure.Services;

public class GeometryValidator : IGeometryValidator
{
    private readonly GeometryFactory _geometryFactory;
    private const int EPSG_4326 = 4326;

    public GeometryValidator(GeometryFactory geometryFactory)
    {
        _geometryFactory = geometryFactory;
    }

    public bool IsGeometryValidAndInEPSG4326(string wkt)
    {
        var wktReader = new WKTReader(_geometryFactory);
        Geometry geometry = wktReader.Read(wkt);

        if (!geometry.IsValid) return false;

        if (geometry.SRID == -1)
        {
            geometry.SRID = EPSG_4326;
        }

        if (geometry.SRID != EPSG_4326)
        {
            return false;
        }

        return true;
    }

    public Geometry ConvertFromWkt(string wkt)
    {
        var wktReader = new WKTReader(_geometryFactory);
        Geometry geometry = wktReader.Read(wkt);
        if (geometry.SRID == -1)
        {
            geometry.SRID = EPSG_4326;
        }
        return geometry;
    }

    public Geometry ConvertFromGeoJson(string geoJson)
    {
        var geoJsonReader = new GeoJsonReader();
        Geometry geometry = geoJsonReader.Read<Geometry>(geoJson);
        return geometry;
    }

    public bool IsGeometryValidAndInEPSG4326(Geometry geometry)
    {
        if (geometry == null) return false;

        foreach (var coordinate in geometry.Coordinates)
        {
            if (coordinate.X < -180 || coordinate.X > 180 || coordinate.Y < -90 || coordinate.Y > 90)
            {
                return false;
            }
        }

        if (!geometry.IsValid) return false;

        if (geometry.SRID == -1)
        {
            geometry.SRID = EPSG_4326;
        }

        if (geometry.SRID != EPSG_4326)
        {
            return false;
        }

        return true;
    }
}