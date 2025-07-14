using DGPCE.Sigemad.Application.Dtos.Common;

namespace DGPCE.Sigemad.Application.Dtos.Direcciones;
public class CreateOrUpdateDireccionDto: IEquatable<CreateOrUpdateDireccionDto>
{
    public int? Id { get; set; }
    public int IdTipoDireccionEmergencia { get; set; }
    public string AutoridadQueDirige { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public FileDto? Archivo { get; set; }
    public bool ActualizarFichero { get; set; }

    public bool Equals(CreateOrUpdateDireccionDto? other)
    {
        if(other is null)
        {
            return false;
        }

        return Id == other.Id && 
            IdTipoDireccionEmergencia == other.IdTipoDireccionEmergencia &&
            string.Equals(AutoridadQueDirige, other.AutoridadQueDirige) && 
            FechaInicio == other.FechaInicio && 
            FechaFin == other.FechaFin &&
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
            IdTipoDireccionEmergencia, 
            AutoridadQueDirige ?? string.Empty, 
            FechaInicio, 
            FechaFin ?? default);
    }
}
