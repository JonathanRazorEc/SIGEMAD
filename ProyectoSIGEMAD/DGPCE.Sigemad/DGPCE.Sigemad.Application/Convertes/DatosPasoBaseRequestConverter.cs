using DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios.Pasos;
using DGPCE.Sigemad.Domain.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class DatosPasoBaseRequestConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(DatosPasoBase);
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var jsonObject = JObject.Load(reader);

        // Obtener el valor de TipoPaso
        if (!jsonObject.TryGetValue("TipoPaso", out var tipoPasoToken))
            throw new JsonSerializationException("El campo 'TipoPaso' es obligatorio.");

        var tipoPaso = tipoPasoToken.ToObject<TipoPaso>();

        DatosPasoBase paso = tipoPaso switch
        {
            TipoPaso.Solicitud => new ManageSolicitudMedioDto(),
            TipoPaso.Tramitacion => new ManageTramitacionMedioDto(),
            TipoPaso.Cancelacion => new ManageCancelacionMedioDto(),
            TipoPaso.Aportacion => new ManageAportacionMedioDto(),
            TipoPaso.Despliegue => new ManageDespliegueMedioDto(),
            TipoPaso.FinIntervencion => new ManageFinIntervencionMedioDto(),
            TipoPaso.LlegadaBase => new ManageLlegadaBaseMedioDto(),
            TipoPaso.Ofrecimiento => new ManageOfrecimientoMedioDto(),
            // Puedes agregar más tipos de pasos aquí
            _ => throw new JsonSerializationException($"Tipo de paso no reconocido: {tipoPaso}")
        };

        serializer.Populate(jsonObject.CreateReader(), paso);
        return paso;
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        serializer.Serialize(writer, value);
    }
}
