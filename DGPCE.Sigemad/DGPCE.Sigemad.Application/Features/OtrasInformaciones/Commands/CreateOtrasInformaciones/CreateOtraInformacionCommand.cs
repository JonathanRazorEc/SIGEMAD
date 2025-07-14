using MediatR;

namespace DGPCE.Sigemad.Application.Features.OtrasInformaciones.Commands.CreateOtrasInformaciones;
public class CreateOtraInformacionCommand : IRequest<CreateOtraInformacionResponse>
{
    public int IdSuceso { get; set; }
    public int? IdOtraInformacion { get; set; }
    public DateTime FechaHora { get; set; }
    public int IdMedio { get; set; }
    public string Asunto { get; set; }
    public string Observaciones { get; set; }
    public List<int> IdsProcedenciaDestino { get; set; }
}


