namespace DGPCE.Sigemad.Application.Features.OtrasInformaciones.Vms;
public class OtraInformacionVm
{
    public int IdOtraInformacion { get; set; }
    public int IdSuceso { get; set; }
    public DateTime FechaHora { get; set; }
    public int IdMedio { get; set; }
    public string Asunto { get; set; }
    public string Observaciones { get; set; }
    public List<int> IdsProcedenciaDestino { get; set; }
}
