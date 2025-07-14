using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Dtos.IntervencionMedios;
public class DetalleIntervencionMedioDto
{
    public MediosCapacidadDto MediosCapacidad { get; set; }

    public int NumeroIntervinientes { get; set; }
}
