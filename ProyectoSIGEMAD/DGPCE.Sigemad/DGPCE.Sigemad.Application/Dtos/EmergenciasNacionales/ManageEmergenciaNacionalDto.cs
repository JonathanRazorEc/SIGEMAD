
namespace DGPCE.Sigemad.Application.Dtos.EmergenciasNacionales;
public class ManageEmergenciaNacionalDto : IEquatable<ManageEmergenciaNacionalDto>
{
    public string Autoridad { get; set; }
    public string DescripcionSolicitud { get; set; }
    public DateTime FechaHoraSolicitud { get; set; }
    public DateTime? FechaHoraDeclaracion { get; set; }
    public string? DescripcionDeclaracion { get; set; }
    public DateTime? FechaHoraDireccion { get; set; }
    public string? Observaciones { get; set; }

    public bool Equals(ManageEmergenciaNacionalDto? other)
    {
        if (other is null)
        {
            return false;
        }
        return string.Equals(Autoridad, other.Autoridad) &&
            string.Equals(DescripcionSolicitud, other.DescripcionSolicitud) &&
            string.Equals(DescripcionDeclaracion, other.DescripcionDeclaracion) &&
            string.Equals(Observaciones, other.Observaciones) &&
            FechaHoraSolicitud == other.FechaHoraSolicitud &&
            FechaHoraDeclaracion == other.FechaHoraDeclaracion &&
            FechaHoraDireccion == other.FechaHoraDireccion;
    }

    public override bool Equals(object? obj)
    {
        if (obj is ManageEmergenciaNacionalDto other)
        {
            return Equals(other);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            Autoridad,
            DescripcionSolicitud,
            DescripcionDeclaracion ?? string.Empty,
            Observaciones ?? string.Empty,
            FechaHoraSolicitud,
            FechaHoraDeclaracion ?? default,
            FechaHoraDireccion ?? default);
    }
}
