using DGPCE.Sigemad.Domain.Constracts;
using NetTopologySuite.Geometries;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;


namespace DGPCE.Sigemad.Infrastructure.Services;

public class CoordinateTransformationService : ICoordinateTransformationService
{
    public (double UTM_X, double UTM_Y, int Huso) ConvertToUTM(double latitude, double longitude)
    {
        // Determinar el huso UTM bas�ndonos en la longitud
        int huso = (int)Math.Floor((longitude + 180) / 6) + 1;

        // Crear el sistema de coordenadas UTM
        var wgs84 = GeographicCoordinateSystem.WGS84;
        var utm = ProjectedCoordinateSystem.WGS84_UTM(huso, latitude >= 0);

        // Crear la transformaci�n
        var transformation = new CoordinateTransformationFactory().CreateFromCoordinateSystems(wgs84, utm);

        // Realizar la transformaci�n
        double[] utmCoordinates = transformation.MathTransform.Transform(new double[] { longitude, latitude });

        return (utmCoordinates[0], utmCoordinates[1], huso);
    }

    public (double UTM_X, double UTM_Y, int Huso) ConvertToUTM(Geometry geometry)
    {
        // Obtener la coordenada representativa seg�n el tipo de geometr�a
        Coordinate representativeCoordinate = GetRepresentativeCoordinate(geometry);

        if (representativeCoordinate == null)
        {
            return (0, 0, 0);
        }

        // Crear la transformaci�n de WGS84 a UTM
        int zone = GetUtmZone(representativeCoordinate.X);
        bool isNorthernHemisphere = representativeCoordinate.Y >= 0;

        var wgs84 = GeographicCoordinateSystem.WGS84;
        var utm = ProjectedCoordinateSystem.WGS84_UTM(zone, isNorthernHemisphere);

        var transform = new CoordinateTransformationFactory().CreateFromCoordinateSystems(wgs84, utm);

        // Transformar la coordenada representativa a UTM
        var utmCoord = transform.MathTransform.Transform(new double[] { representativeCoordinate.X, representativeCoordinate.Y });
        return (utmCoord[0], utmCoord[1], zone);
    }

    private  Coordinate GetRepresentativeCoordinate(Geometry geometry)
    {
        if (geometry is Point point)
        {
            return point.Coordinate;
        }
        else if (geometry is LineString lineString)
        {
            return lineString.Coordinate; // Primera coordenada de la l�nea
        }
        else if (geometry is Polygon polygon)
        {
            return polygon.Centroid.Coordinate; // Centroide del pol�gono
        }
        else if (geometry is MultiPolygon multiPolygon)
        {
            return multiPolygon.Centroid.Coordinate; // Centroide del multipol�gono
        }
        else if (geometry is MultiLineString multiLineString)
        {
            return multiLineString.Coordinates.FirstOrDefault(); // Primera coordenada de la primera l�nea
        }
        else if (geometry is GeometryCollection geometryCollection && geometryCollection.NumGeometries > 0)
        {
            return GetRepresentativeCoordinate(geometryCollection.GetGeometryN(0)); // Representativa del primer elemento
        }
        // Otros tipos de geometr�a pueden ser manejados aqu�

        return null;
    }

    private int GetUtmZone(double longitude)
    {
        return (int)((longitude + 180) / 6) + 1;
    }

}