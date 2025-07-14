namespace DGPCE.Sigemad.Application.Features.DatosPrincipales.Commands;
public class CreateDatoPrincipalCommand: IEquatable<CreateDatoPrincipalCommand>
{
    public DateTime FechaHora { get; set; }
    public string? Observaciones { get; set; }
    public string? Prevision { get; set; }

    public bool Equals(CreateDatoPrincipalCommand? other)
    {
        if (other is null)
        {
            return false;
        }

        return FechaHora == other.FechaHora &&
            string.Equals(Observaciones, other.Observaciones) &&
            string.Equals(Prevision, other.Prevision);
    }

    public override bool Equals(object? obj)
    {
        if (obj is CreateDatoPrincipalCommand other)
        {
            return Equals(other);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            FechaHora,
            Observaciones ?? string.Empty,
            Prevision ?? string.Empty);
    }
}
