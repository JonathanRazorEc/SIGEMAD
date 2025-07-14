
namespace DGPCE.Sigemad.Application.Dtos.ConvocatoriasCECOD;
public class ManageConvocatoriaCECODDto : IEquatable<ManageConvocatoriaCECODDto>
{
    public int? Id { get; set; }
    public DateOnly FechaInicio { get; set; }
    public DateOnly? FechaFin { get; set; }
    public string Lugar { get; set; }
    public string Convocados { get; set; }
    public string? Participantes { get; set; }
    public string? Observaciones { get; set; }

    public bool Equals(ManageConvocatoriaCECODDto? other)
    {
        if (other is null)
        {
            return false;
        }
        return Id == other.Id &&
            string.Equals(Lugar, other.Lugar) &&
            string.Equals(Convocados, other.Convocados) &&
            string.Equals(Participantes, other.Participantes) &&
            string.Equals(Observaciones, other.Observaciones) &&
            FechaInicio == other.FechaInicio &&
            FechaFin == other.FechaFin;
    }

    public override bool Equals(object? obj)
    {
        if (obj is ManageConvocatoriaCECODDto other)
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
            FechaFin,
            Lugar,
            Convocados,
            Participantes ?? string.Empty,
            Observaciones ?? string.Empty);
    }
}
