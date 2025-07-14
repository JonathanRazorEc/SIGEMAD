using DGPCE.Sigemad.Domain.Common;


namespace DGPCE.Sigemad.Domain.Modelos;
public class DatoPrincipal : BaseDomainModel<int>
{
    public DateTime? FechaHora { get; set; }

    public string? Observaciones { get; set; }

    public string? Prevision { get; set; }

    public int IdEvolucion { get; set; }
    public virtual Evolucion Evolucion { get; set; } = null!;
}
