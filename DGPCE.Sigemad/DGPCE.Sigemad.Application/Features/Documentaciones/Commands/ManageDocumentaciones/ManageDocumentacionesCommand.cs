using DGPCE.Sigemad.Application.Dtos.DetallesDocumentaciones;
using DGPCE.Sigemad.Application.Dtos.Documentaciones;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Documentaciones.Commands.ManageDocumentaciones;
public class ManageDocumentacionesCommand : IRequest<CreateOrUpdateDocumentacionResponse>
{
    public int? IdRegistroActualizacion { get; set; }
    public int IdSuceso { get; set; }

    public List<DetalleDocumentacionDto>? DetallesDocumentaciones { get; set; } = new();
}
