using MediatR;

namespace DGPCE.Sigemad.Application.Features.Documentaciones.Commands.DeleteDocumentacionByRegistro;
public class DeleteDocumentacionByIdRegistroCommand : IRequest
{
    public int IdRegistroActualizacion { get; set; }
}
