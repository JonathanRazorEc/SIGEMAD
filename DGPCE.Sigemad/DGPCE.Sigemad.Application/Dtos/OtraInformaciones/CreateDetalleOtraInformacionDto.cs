using DGPCE.Sigemad.Application.Dtos.Direcciones;
using DGPCE.Sigemad.Application.Dtos.Documentaciones;
using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Dtos.OtraInformaciones;
public class CreateDetalleOtraInformacionDto : IEquatable<CreateDetalleOtraInformacionDto>
{
    public int? Id { get; set; }
    public DateTime FechaHora { get; set; }
    public int IdMedio { get; set; }
    public string Asunto { get; set; }
    public string Observaciones { get; set; }
    public List<int> IdsProcedenciasDestinos { get; set; }

    public bool Equals(CreateDetalleOtraInformacionDto? other)
    {
        if (other is null)
        {
            return false;
        }
        return Id == other.Id &&
            IdMedio == other.IdMedio &&
            string.Equals(Observaciones, other.Observaciones) &&
            string.Equals(Asunto, other.Asunto) &&
            FechaHora == other.FechaHora &&
            (IdsProcedenciasDestinos == null && other.IdsProcedenciasDestinos == null ||
            IdsProcedenciasDestinos != null && other.IdsProcedenciasDestinos != null &&
            IdsProcedenciasDestinos.SequenceEqual(other.IdsProcedenciasDestinos));
    }


    public override bool Equals(object? obj)
    {
        if (obj is CreateDetalleOtraInformacionDto other)
        {
            return Equals(other);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            Id,
            IdMedio,
            FechaHora,
            Asunto,
            Observaciones);
    }
}
