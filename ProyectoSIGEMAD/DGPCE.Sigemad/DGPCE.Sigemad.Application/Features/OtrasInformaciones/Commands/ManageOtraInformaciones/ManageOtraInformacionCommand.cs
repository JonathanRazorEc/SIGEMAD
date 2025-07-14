using DGPCE.Sigemad.Application.Dtos.OtraInformaciones;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.OtrasInformaciones.Commands.ManageOtraInformaciones;
public class ManageOtraInformacionCommand: IRequest<ManageOtraInformacionResponse>
{
    public int? IdRegistroActualizacion { get; set; }
    public int IdSuceso { get; set; }
    public List<CreateDetalleOtraInformacionDto> Lista { get; set; }
}
