using MediatR;

namespace DGPCE.Sigemad.Application.Features.Documentaciones.Commands.DeleteDocumentaciones;
public class DeleteDocumentacionCommand : IRequest
{
    public int Id { get; set; }
}
