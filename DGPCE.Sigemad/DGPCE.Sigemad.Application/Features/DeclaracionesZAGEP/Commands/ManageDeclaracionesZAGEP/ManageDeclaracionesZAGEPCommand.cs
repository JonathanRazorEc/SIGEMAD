using DGPCE.Sigemad.Application.Dtos.DeclaracionesZAGEP;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.DeclaracionesZAGEP.Commands.ManageDeclaracionesZAGEP;
public class ManageDeclaracionesZAGEPCommand : IRequest<ManageDeclaracionZAGEPResponse>
{
    public int? IdRegistroActualizacion { get; set; }
    public int IdSuceso { get; set; }
    public List<ManageDeclaracionZAGEPDto>? Detalles { get; set; } = new();
}
