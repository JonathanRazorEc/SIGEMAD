using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Superficies.Queries.GetSuperficiesList;
public class GetSuperficieListQuery : IRequest<IReadOnlyList<SuperficieFiltro>>
{
}
