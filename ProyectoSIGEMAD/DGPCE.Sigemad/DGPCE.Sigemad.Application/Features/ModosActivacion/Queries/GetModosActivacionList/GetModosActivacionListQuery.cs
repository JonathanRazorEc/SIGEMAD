using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.ModosActivacion.Queries.GetModosActivacionList;
public class GetModosActivacionListQuery : IRequest<IReadOnlyList<ModoActivacion>>
{
}
