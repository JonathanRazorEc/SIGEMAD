

namespace DGPCE.Sigemad.Application.Dtos.ActivacionSistema;
public class ManageActivacionSistemaDto : IEquatable<ManageActivacionSistemaDto>
{
    public int? Id { get; set; }
    public int IdTipoSistemaEmergencia { get; set; }

    public DateTime? FechaHoraActivacion { get; set; }
    public DateTime? FechaHoraActualizacion { get; set; }
    public string? Autoridad { get; set; }
    public string? DescripcionSolicitud { get; set; }
    public string? Observaciones { get; set; }
    public int? IdModoActivacion { get; set; }
    public DateOnly? FechaActivacion { get; set; }
    public string? Codigo { get; set; }

    public string? Nombre { get; set; }

    public string? UrlAcceso { get; set; }

    public DateTime? FechaHoraPeticion { get; set; }

    public DateOnly? FechaAceptacion { get; set; }

    public string? Peticiones { get; set; }

    public string? MediosCapacidades { get; set; }

    public bool Equals(ManageActivacionSistemaDto? other)
    {
        if (other is null)
        {
            return false;
        }
        return Id == other.Id &&
            IdTipoSistemaEmergencia == other.IdTipoSistemaEmergencia &&
            IdModoActivacion == other.IdModoActivacion &&
            string.Equals(Autoridad, other.Autoridad) &&
            string.Equals(DescripcionSolicitud ,other.DescripcionSolicitud) &&
            string.Equals(Observaciones, other.Observaciones) &&
            string.Equals(Codigo, other.Codigo) &&
            string.Equals(Nombre, other.Nombre) &&
            string.Equals(UrlAcceso, other.UrlAcceso) &&
            string.Equals(Peticiones, other.Peticiones) &&
            string.Equals(MediosCapacidades, other.MediosCapacidades) &&
            FechaHoraActivacion == other.FechaHoraActivacion &&
            FechaHoraActualizacion == other.FechaHoraActualizacion &&
            FechaActivacion == other.FechaActivacion &&
            FechaHoraPeticion == other.FechaHoraPeticion &&
            FechaAceptacion == other.FechaAceptacion;
    }

    public override bool Equals(object? obj)
    {
        if (obj is ManageActivacionSistemaDto other)
        {
            return Equals(other);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            Id,
            IdTipoSistemaEmergencia,
            IdModoActivacion,
            Autoridad ?? string.Empty,
            DescripcionSolicitud ?? string.Empty,
            Observaciones ?? string.Empty,
            Codigo ?? string.Empty,
            Nombre ?? string.Empty);
    }
}
