using DGPCE.Sigemad.Domain.Modelos;
using NetTopologySuite.Geometries;

namespace DGPCE.Sigemad.Application.Dtos.Impactos;
public class ManageImpactoDto : IEquatable<ManageImpactoDto>
{
    public int? Id { get; set; }
    public int IdTipoImpactoEvolucion { get; set; }
    public int IdImpactoClasificado { get; set; }
    public int? IdAlteracionInterrupcion { get; set; }
    public string? Causa { get; set; }
    public DateTime? Fecha { get; set; }
    public DateTime? FechaHora { get; set; }
    public DateTime? FechaHoraInicio { get; set; }
    public DateTime? FechaHoraFin { get; set; }
    public int? Numero { get; set; }
    public int? NumeroGraves { get; set; }
    public int? NumeroIntervinientes { get; set; }
    public int? NumeroLocalidades { get; set; }
    public int? NumeroServicios { get; set; }
    public int? NumeroUsuarios { get; set; }
    public string? Observaciones { get; set; }
    public int? IdTipoDanio { get; set; }
    public int? IdProvincia { get; set; }
    public int? IdMunicipio { get; set; }

    public DateTime? ExtraFechaHora1 { get; set; }
    public DateTime? ExtraFechaHora2 { get; set; }

    public int? ExtraNumerico1 { get; set; }
    public int? ExtraNumerico2 { get; set; }
    public int? ExtraNumerico3 { get; set; }

    public string? ExtraCaracter1 { get; set; }

    public int? IdZonaPlanificacion { get; set; }

    public bool Equals(ManageImpactoDto? other)
    {
        if (other is null)
        {
            return false;
        }

        return Id == other.Id &&
            IdImpactoClasificado == other.IdImpactoClasificado &&
            Numero == other.Numero &&
            string.Equals(Observaciones, other.Observaciones) &&
            Fecha == other.Fecha &&
            FechaHora == other.FechaHora &&
            FechaHoraInicio == other.FechaHoraInicio &&
            FechaHoraFin == other.FechaHoraFin &&
            IdAlteracionInterrupcion == other.IdAlteracionInterrupcion &&
            string.Equals(Causa, other.Causa) &&
            NumeroGraves == other.NumeroGraves &&
            IdTipoDanio == other.IdTipoDanio &&
            IdZonaPlanificacion == other.IdZonaPlanificacion &&
            NumeroUsuarios == other.NumeroUsuarios &&
            NumeroIntervinientes == other.NumeroIntervinientes &&
            NumeroServicios == other.NumeroServicios &&
            NumeroLocalidades == other.NumeroLocalidades;
    }

    public override bool Equals(object? obj)
    {
        if (obj is ManageImpactoDto other)
        {
            return Equals(other);
        }
        return false;
    }

    public override int GetHashCode()
    {
        int hash = HashCode.Combine(
            Id,
            IdImpactoClasificado,
            Numero,
            Observaciones ?? string.Empty,
            Fecha ?? default);

        hash = HashCode.Combine(
            hash,
            FechaHora ?? default,
            FechaHoraInicio ?? default,
            FechaHoraFin ?? default,
            IdAlteracionInterrupcion ?? default,
            Causa ?? string.Empty,
            NumeroGraves,
            IdTipoDanio);

        hash = HashCode.Combine(
            hash,
            IdZonaPlanificacion ?? default,
            NumeroUsuarios,
            NumeroIntervinientes,
            NumeroServicios,
            NumeroLocalidades);

        return hash;
    }
}
