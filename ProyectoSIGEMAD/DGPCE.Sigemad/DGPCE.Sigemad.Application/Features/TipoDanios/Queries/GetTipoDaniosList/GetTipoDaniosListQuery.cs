using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.TipoDanios.Queries.GetTipoDaniosList;
public class GetTipoDaniosListQuery : IRequest<IReadOnlyList<TipoDanio>>
{
}
