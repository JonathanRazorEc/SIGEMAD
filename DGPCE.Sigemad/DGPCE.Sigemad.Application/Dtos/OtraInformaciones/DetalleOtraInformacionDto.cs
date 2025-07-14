using DGPCE.Sigemad.Domain.Modelos;

namespace DGPCE.Sigemad.Application.Dtos.OtraInformaciones;
public class DetalleOtraInformacionDto
{
    public int Id { get; set; }
    public DateTime FechaHora { get; set; }
    public Medio Medio { get; set; }
    public string Asunto { get; set; }
    public string Observaciones { get; set; }

    public List<ProcedenciaDestino> ProcedenciasDestinos { get; set; } = new();

    public bool EsEliminable { get; set; }



}
