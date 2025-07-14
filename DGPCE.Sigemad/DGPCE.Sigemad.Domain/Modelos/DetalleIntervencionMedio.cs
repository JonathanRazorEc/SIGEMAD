using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos;
public class DetalleIntervencionMedio : BaseEntity
{
    public int IdMediosCapacidad { get; set; }
    public MediosCapacidad MediosCapacidad { get; set; }

    public int NumeroIntervinientes { get; set; }

    public int IdIntervencionMedio { get; set; }
    public IntervencionMedio IntervencionMedio { get; set; }
}
