using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;

public class ImpactoEvolucion : BaseDomainModel<int>
{
    public ImpactoEvolucion() { }

    public int IdTipoImpactoEvolucion { get; set; }
    public int IdImpactoClasificado { get; set; }

    public int? IdAlteracionInterrupcion { get; set; }
    public virtual AlteracionInterrupcion? AlteracionInterrupcion { get; set; }

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
    public virtual TipoDanio? TipoDanio { get; set; }
    public int? IdProvincia { get; set; }
    public int? IdMunicipio { get; set; }

    public DateTime? ExtraFechaHora1 { get; set; }
    public DateTime? ExtraFechaHora2 { get; set; }

    public int? ExtraNumerico1 { get; set; }
    public int? ExtraNumerico2 { get; set; }
    public int? ExtraNumerico3 { get; set; }

    public string? ExtraCaracter1 { get; set; }

    public int? IdZonaPlanificacion { get; set; }
    public virtual ZonaPlanificacion? ZonaPlanificacion { get; set; }

    public virtual ImpactoClasificado ImpactoClasificado { get; set; }

    public virtual TipoImpactoEvolucion TipoImpactoEvolucion { get; set; }

    public virtual Provincia? Provincia { get; set; } 
    public virtual Municipio? Municipio { get; set; } 
}
