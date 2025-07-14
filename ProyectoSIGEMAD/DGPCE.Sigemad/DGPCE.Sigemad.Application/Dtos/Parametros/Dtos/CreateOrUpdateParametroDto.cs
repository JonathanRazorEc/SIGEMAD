namespace DGPCE.Sigemad.Application.Dtos.Parametros.Dtos;
public class CreateOrUpdateParametroDto : IEquatable<CreateOrUpdateParametroDto>
{
    public int? Id { get; set; }
    public int? IdEstadoIncendio { get; set; }
    public DateTime? FechaFinal { get; set; }
    public int? IdPlanEmergencia { get; set; }

    public int? IdFaseEmergencia { get; set; }

    public int? IdPlanSituacion { get; set; }
    public int? IdSituacionEquivalente { get; set; }

    public DateTime? FechaHoraActualizacion { get; set; }
    public string? Observaciones { get; set; }

    public string? Prevision { get; set; }


    public bool Equals(CreateOrUpdateParametroDto? other)
    {
        if (other is null)
        {
            return false;
        }

        return Id == other.Id &&
            IdEstadoIncendio == other.IdEstadoIncendio &&
            FechaFinal == other.FechaFinal &&
            FechaHoraActualizacion == other.FechaHoraActualizacion &&
            Observaciones == other.Observaciones &&
            Prevision == other.Prevision &&
            IdPlanEmergencia == other.IdPlanEmergencia &&
            IdFaseEmergencia == other.IdFaseEmergencia &&
            IdPlanSituacion == other.IdPlanSituacion &&
            IdSituacionEquivalente == other.IdSituacionEquivalente;
    }

    public override bool Equals(object? obj)
    {
        if (obj is CreateOrUpdateParametroDto other)
        {
            return Equals(other);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            Id,
            IdEstadoIncendio,
            FechaFinal ?? default,
            Observaciones ?? default,
            IdPlanEmergencia ?? default,
            IdFaseEmergencia ?? default,
            IdPlanSituacion ?? default,
            IdSituacionEquivalente ?? default);
    }

}
