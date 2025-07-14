using DGPCE.Sigemad.Application.Dtos.Documentaciones;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.Documentaciones.Queries.GetDetalleDocumentacionesById;
public class GetDocumentacionesByIdQuery : IRequest<DocumentacionDto>
{
    public int Id { get; set; }

    public GetDocumentacionesByIdQuery(int id)
    {
        Id = id;
    }
}