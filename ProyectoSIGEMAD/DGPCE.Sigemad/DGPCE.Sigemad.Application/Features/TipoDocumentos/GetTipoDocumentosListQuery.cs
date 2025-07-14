using DGPCE.Sigemad.Domain.Modelos;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.TipoDocumentos;
 public class GetTipoDocumentosListQuery : IRequest<IReadOnlyList<TipoDocumento>>
  {
  }

