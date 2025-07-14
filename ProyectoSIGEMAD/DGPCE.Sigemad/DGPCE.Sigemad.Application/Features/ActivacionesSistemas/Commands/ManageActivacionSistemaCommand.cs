using DGPCE.Sigemad.Application.Dtos.ActivacionSistema;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.ActivacionesSistemas.Commands;
public class ManageActivacionSistemaCommand : IRequest<ManageActivacionSistemaResponse>
{
    public int? IdRegistroActualizacion { get; set; }
    public int IdSuceso { get; set; }

    public List<ManageActivacionSistemaDto>? Detalles { get; set; } = new();
}
