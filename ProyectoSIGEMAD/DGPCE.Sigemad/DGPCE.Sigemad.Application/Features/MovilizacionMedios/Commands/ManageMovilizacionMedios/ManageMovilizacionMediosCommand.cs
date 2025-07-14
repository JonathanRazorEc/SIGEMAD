using DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.MovilizacionMedios.Commands.ManageMovilizacionMedios;
public class ManageMovilizacionMediosCommand : IRequest<ManageMovilizacionMediosResponse>
{
    public int? IdRegistroActualizacion { get; set; }
    public int IdSuceso { get; set; }
    public List<MovilizacionMedioDto> Movilizaciones { get; set; } = new();
}
