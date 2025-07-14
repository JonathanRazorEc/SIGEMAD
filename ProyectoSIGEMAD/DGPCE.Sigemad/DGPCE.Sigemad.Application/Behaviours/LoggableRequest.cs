using NetTopologySuite.Geometries;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DGPCE.Sigemad.Application.Behaviours;

public class LoggableRequest<TRequest>
{
    private readonly TRequest _request;
    private readonly JsonSerializerSettings _serializerSettings;

    public LoggableRequest(TRequest request)
    {
        _request = request;
        _serializerSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
    }

    public override string ToString()
    {
        var loggableObject = CreateLoggableObject(_request);
        return JsonConvert.SerializeObject(loggableObject, _serializerSettings);
    }

    private object CreateLoggableObject(TRequest request)
    {
        var geoJsonWriter = new NetTopologySuite.IO.GeoJsonWriter();

        // Crear un diccionario para almacenar las propiedades del request
        var properties = new Dictionary<string, object>();

        foreach (var property in request.GetType().GetProperties())
        {
            var value = property.GetValue(request);
            if (value is Geometry geometryValue)
            {
                // Convertir Geometry a GeoJSON
                var geoJson = geoJsonWriter.Write(geometryValue);
                properties[property.Name] = JObject.Parse(geoJson);
            }
            else
            {
                properties[property.Name] = value;
            }
        }

        return properties;
    }
}