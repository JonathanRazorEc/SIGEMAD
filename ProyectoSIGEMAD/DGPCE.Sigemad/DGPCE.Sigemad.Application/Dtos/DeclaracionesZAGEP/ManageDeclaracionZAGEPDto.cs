
namespace DGPCE.Sigemad.Application.Dtos.DeclaracionesZAGEP;
public class ManageDeclaracionZAGEPDto : IEquatable<ManageDeclaracionZAGEPDto>
{

    public int? Id { get; set; }
    public DateOnly FechaSolicitud { get; set; }
    public string Denominacion { get; set; }
    public string? Observaciones { get; set; } = null;
    public bool Equals(ManageDeclaracionZAGEPDto? other)
    {
        if (other is null)
        {
            return false;
        }
        return Id == other.Id &&
            string.Equals(Denominacion, other.Denominacion) &&
            string.Equals(Observaciones, other.Observaciones) &&
            FechaSolicitud == other.FechaSolicitud;
    }

    public override bool Equals(object? obj)
    {
        if (obj is ManageDeclaracionZAGEPDto other)
        {
            return Equals(other);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            Id,
            FechaSolicitud,
            Denominacion,
            Observaciones ?? string.Empty);
    }
}
