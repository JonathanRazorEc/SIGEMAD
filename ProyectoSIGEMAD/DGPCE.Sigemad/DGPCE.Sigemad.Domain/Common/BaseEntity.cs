namespace DGPCE.Sigemad.Domain.Common;
public class BaseEntity
{
    public DateTime FechaCreacion { get; set; }
    public String? CreadoPor { get; set; }
    public DateTime? FechaModificacion { get; set; }
    public String? ModificadoPor { get; set; }
    public DateTime? FechaEliminacion { get; set; }
    public String? EliminadoPor { get; set; }
    public bool Borrado { get; set; } = false;
}
