using DGPCE.Sigemad.Application.Dtos.Common;

namespace DGPCE.Sigemad.Application.Dtos.ActivacionesPlanes;
public class ManageActivacionPlanEmergenciaDto : IEquatable<ManageActivacionPlanEmergenciaDto>
{
    public int? Id { get; set; }
    public int? IdTipoPlan { get; set; }
    public string? TipoPlanPersonalizado { get; set; }
    public int? IdPlanEmergencia { get; set; }
    public string? PlanEmergenciaPersonalizado { get; set; }
    public DateTimeOffset FechaHoraInicio { get; set; }
    public DateTimeOffset? FechaHoraFin { get; set; }
    public string Autoridad { get; set; }
    public string? Observaciones { get; set; }
    public Guid? IdArchivo { get; set; }
    public FileDto? Archivo { get; set; }

    public bool ActualizarFichero { get; set; }

    public bool Equals(ManageActivacionPlanEmergenciaDto? other)
    {
        if (other is null)
        {
            return false;
        }
        return Id == other.Id &&
            IdTipoPlan == other.IdTipoPlan &&
            IdPlanEmergencia == other.IdPlanEmergencia &&
            IdArchivo == other.IdArchivo &&
            string.Equals(TipoPlanPersonalizado, other.TipoPlanPersonalizado) &&
            string.Equals(PlanEmergenciaPersonalizado, other.PlanEmergenciaPersonalizado) &&
            string.Equals(Autoridad, other.Autoridad) &&
            string.Equals(Observaciones, other.Observaciones) &&
            FechaHoraInicio == other.FechaHoraInicio &&
            FechaHoraFin == other.FechaHoraFin &&
            (Archivo == null && other.Archivo == null ||
            Archivo != null && other.Archivo != null &&
            string.Equals(Archivo.FileName, other.Archivo.FileName));
    }


    public override bool Equals(object? obj)
    {
        if (obj is ManageActivacionPlanEmergenciaDto other)
        {
            return Equals(other);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            Id,
            IdTipoPlan,
            IdPlanEmergencia,
            IdArchivo);
    }

}
