using DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios;

namespace DGPCE.Sigemad.Application.Dtos.IntervencionMedios;
public class MediosCapacidadDto
{
    public int Id { get; set; }
    public TipoCapacidadDto TipoCapacidad { get; set; }
    public TipoMedioDto TipoMedio { get; set; }
    public string Descripcion { get; set; }
    public int NumeroMedio { get; set; }
}
