using DGPCE.Sigemad.Application.Dtos.EmergenciasNacionales;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.EmergenciasNacionales.Commands.ManageEmergenciasNacionales;
public class ManageEmergenciasNacionalesCommand : IRequest<ManageEmergenciaNacionalResponse>
{
    public int? IdRegistroActualizacion { get; set; }
    public int IdSuceso { get; set; }
    public ManageEmergenciaNacionalDto? EmergenciaNacional { get; set; } = null!;
}
