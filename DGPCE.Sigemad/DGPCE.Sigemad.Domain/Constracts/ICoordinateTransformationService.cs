using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Domain.Constracts
{
    public interface ICoordinateTransformationService
    {
        (double UTM_X, double UTM_Y, int Huso) ConvertToUTM(double latitude, double longitude);
        (double UTM_X, double UTM_Y, int Huso) ConvertToUTM(Geometry geometry);



        // Nuevos métodos para conversión bidireccional
        Geometry? ConvertUTMToGeometry(decimal? utmX, decimal? utmY, int? huso);
        (decimal? utmX, decimal? utmY, int? huso) ConvertGeometryToUTMDecimal(Geometry? geometry);
    }
}
