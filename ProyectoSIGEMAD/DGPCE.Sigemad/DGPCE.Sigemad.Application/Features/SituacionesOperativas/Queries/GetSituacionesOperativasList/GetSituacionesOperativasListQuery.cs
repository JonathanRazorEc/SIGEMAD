using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.SituacionesOperativas.Queries.GetSituacionesOperativasList;
public class GetSituacionesOperativasListQuery : IRequest<IReadOnlyList<SituacionOperativa>>
{
}
