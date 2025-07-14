using DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Capacidades.Queries.GetCapacidadesList;
public class GetCapacidadesListQuery : IRequest<IReadOnlyList<CapacidadDto>>
{
}
