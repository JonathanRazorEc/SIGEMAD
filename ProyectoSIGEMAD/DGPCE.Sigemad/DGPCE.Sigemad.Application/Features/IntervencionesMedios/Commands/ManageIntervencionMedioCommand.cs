using DGPCE.Sigemad.Application.Dtos.IntervencionMedios;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.IntervencionesMedios.Commands;
public class ManageIntervencionMedioCommand : IRequest<ManageIntervencionMedioResponse>
{
    public int? IdRegistroActualizacion { get; set; }
    public int IdSuceso { get; set; }

    public List<CreateOrUpdateIntervencionMedioDto> Intervenciones { get; set; } = new();
}
