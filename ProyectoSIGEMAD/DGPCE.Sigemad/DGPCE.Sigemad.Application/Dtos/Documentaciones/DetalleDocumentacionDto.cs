using DGPCE.Sigemad.Application.Dtos.Common;
using DGPCE.Sigemad.Application.Dtos.Direcciones;

namespace DGPCE.Sigemad.Application.Dtos.Documentaciones;
public class DetalleDocumentacionDto : IEquatable<DetalleDocumentacionDto>
{
    public int? Id { get; set; }
    public DateTime FechaHora { get; set; }
    public DateTime FechaHoraSolicitud { get; set; }
    public int IdTipoDocumento { get; set; }
    public string? Descripcion { get; set; }
    public FileDto? Archivo { get; set; }

    public List<int>? IdsProcedenciasDestinos { get; set; } = new();

    public bool Equals(DetalleDocumentacionDto? other)
    {
        if (other is null)
        {
            return false;
        }
        return Id == other.Id &&
            IdTipoDocumento == other.IdTipoDocumento &&
            string.Equals(Descripcion, other.Descripcion) &&
            FechaHora == other.FechaHora &&
            FechaHoraSolicitud == other.FechaHoraSolicitud &&
            (IdsProcedenciasDestinos == null && other.IdsProcedenciasDestinos == null ||
            IdsProcedenciasDestinos != null && other.IdsProcedenciasDestinos != null &&
            IdsProcedenciasDestinos.SequenceEqual(other.IdsProcedenciasDestinos)) &&
            (Archivo == null && other.Archivo == null ||
            Archivo != null && other.Archivo != null &&
            string.Equals(Archivo.FileName, other.Archivo.FileName));
    }


    public override bool Equals(object? obj)
    {
        if (obj is CreateOrUpdateDireccionDto other)
        {
            return Equals(other);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            Id,
            IdTipoDocumento,
            FechaHora,
            FechaHoraSolicitud);
    }
}