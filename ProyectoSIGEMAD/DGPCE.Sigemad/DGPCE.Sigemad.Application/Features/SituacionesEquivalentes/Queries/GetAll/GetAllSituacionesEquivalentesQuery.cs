using DGPCE.Sigemad.Application.Dtos.SituacionesEquivalentes;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.SituacionesEquivalentes.Queries.GetAll;
public class GetAllSituacionesEquivalentesQuery : IRequest<IReadOnlyList<SituacionEquivalenteDto>>
{
}
