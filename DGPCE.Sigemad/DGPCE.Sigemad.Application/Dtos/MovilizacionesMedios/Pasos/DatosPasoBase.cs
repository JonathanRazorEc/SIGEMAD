using DGPCE.Sigemad.Domain.Enums;
using Newtonsoft.Json;

namespace DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios.Pasos;

[JsonConverter(typeof(DatosPasoBaseRequestConverter))]
public class DatosPasoBase
{
    public int? Id { get; set; }
    public TipoPaso TipoPaso { get; set; }
}
