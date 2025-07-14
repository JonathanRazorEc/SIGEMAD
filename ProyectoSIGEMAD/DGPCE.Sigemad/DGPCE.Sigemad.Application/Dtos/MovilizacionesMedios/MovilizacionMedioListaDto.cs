using DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios.Pasos;

namespace DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios;
public class MovilizacionMedioListaDto
{
    public int? Id { get; set; }
    public string Solicitante { get; set; }
    public List<EjecucionPasoDto> Pasos { get; set; } = new();

    public bool EsEliminable { get; set; }

}
