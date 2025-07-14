
using DGPCE.Sigemad.Application.Dtos.Documentaciones;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Documentaciones.Queries.GetDocumentacion;
public class GetDoumentacionQuery : IRequest<DocumentacionDto>
{
    public int? IdRegistroActualizacion { get; set; }
    public int IdSuceso { get; set; }
}
