using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;
public class DetalleSucesoRelacionado : BaseEntity
{
    public int IdCabeceraSuceso { get; set; }
    public int IdSucesoAsociado { get; set; }

    // Relaciones
    public virtual SucesoRelacionado SucesoRelacionado { get; set; } = null!;
    public virtual Suceso SucesoAsociado { get; set; } = null!;
}
