using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Domain.Constracts
{
    public interface IGeometryValidator
    {
        bool IsGeometryValidAndInEPSG4326(string wkt);
        bool IsGeometryValidAndInEPSG4326(Geometry geometry);
        Geometry ConvertFromWkt(string wkt);
        Geometry ConvertFromGeoJson(string geoJson);
    }
}
