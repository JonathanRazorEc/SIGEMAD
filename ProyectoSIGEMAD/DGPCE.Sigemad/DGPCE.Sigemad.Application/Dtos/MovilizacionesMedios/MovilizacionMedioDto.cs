using DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios.Pasos;

namespace DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios;
public class MovilizacionMedioDto
{
    public int? Id { get; set; }
    public string Solicitante { get; set; }
    public List<DatosPasoBase> Pasos { get; set; } = new();
}
