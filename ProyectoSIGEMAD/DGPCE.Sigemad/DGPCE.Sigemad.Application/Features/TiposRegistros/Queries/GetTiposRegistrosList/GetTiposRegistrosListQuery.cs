using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.TiposRegistros.Queries.GetTiposRegistrosList;
public class GetTiposRegistrosListQuery : IRequest<IReadOnlyList<TipoRegistro>>
{
}
