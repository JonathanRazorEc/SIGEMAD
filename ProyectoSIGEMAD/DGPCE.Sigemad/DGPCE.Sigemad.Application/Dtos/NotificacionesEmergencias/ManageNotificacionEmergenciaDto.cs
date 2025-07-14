
namespace DGPCE.Sigemad.Application.Dtos.NotificacionesEmergencias;
public class ManageNotificacionEmergenciaDto : IEquatable<ManageNotificacionEmergenciaDto>
{
    public int? Id { get; set; }
    public int IdTipoNotificacion { get; set; }
    public DateTime FechaHoraNotificacion { get; set; }
    public string OrganosNotificados { get; set; }
    public string? UCPM { get; set; }
    public string? OrganismoInternacional { get; set; }
    public string? OtrosPaises { get; set; }
    public string? Observaciones { get; set; }

    public bool Equals(ManageNotificacionEmergenciaDto? other)
    {
        if (other is null)
        {
            return false;
        }
        return Id == other.Id &&
            IdTipoNotificacion == other.IdTipoNotificacion &&
            string.Equals(OrganosNotificados, other.OrganosNotificados) &&
            string.Equals(UCPM, other.UCPM) &&
            string.Equals(OrganismoInternacional, other.OrganismoInternacional) &&
            string.Equals(OtrosPaises, other.OtrosPaises) &&
            string.Equals(Observaciones, other.Observaciones) &&
            FechaHoraNotificacion == other.FechaHoraNotificacion;
    }

    public override bool Equals(object? obj)
    {
        if (obj is ManageNotificacionEmergenciaDto other)
        {
            return Equals(other);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            Id,
            IdTipoNotificacion,
            OrganosNotificados,
            UCPM ?? string.Empty,
            OrganismoInternacional ?? string.Empty,
            OtrosPaises ?? string.Empty,
            Observaciones ?? string.Empty);
    }
}
