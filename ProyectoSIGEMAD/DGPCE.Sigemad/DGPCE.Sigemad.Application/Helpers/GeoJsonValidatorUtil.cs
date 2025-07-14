using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Application.Helpers;
public static class GeoJsonValidatorUtil
{
    public static bool IsGeometryInWgs84(Geometry geometry)
    {
        if (geometry == null)
        {
            return false;
        }

        foreach (var coordinate in geometry.Coordinates)
        {
            if (coordinate.X < -180 || coordinate.X > 180 || coordinate.Y < -90 || coordinate.Y > 90)
            {
                return false;
            }
        }

        return true;
    }

    public static bool AreGeometriesEqual(Geometry geo1, Geometry geo2)
    {
        if (geo1 == null && geo2 == null)
            return true; // Ambas son nulas, se consideran iguales

        if (geo1 == null || geo2 == null)
            return false; // Solo una es nula, no son iguales

        return geo1.EqualsExact(geo2);
    }
}