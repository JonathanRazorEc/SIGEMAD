using MediatR;

namespace DGPCE.Sigemad.Application.Features.OtrasInformaciones.Commands.UpdateOtrasInformaciones;
public class UpdateDetalleOtraInformacionCommand : IRequest
{    
    public int Id { get; set; }
    public int? IdOtraInformacion { get; set; }
    public int IdSuceso { get; set; }
    public DateTime FechaHora { get; set; }
    public int IdMedio { get; set; }
    public string Asunto { get; set; }
    public string Observaciones { get; set; }
    public List<int> IdsProcedenciaDestino { get; set; }
}
