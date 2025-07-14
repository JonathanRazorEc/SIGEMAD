using DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.TiposAdministraciones.Queries.GetTiposAdministracionList;
public class GetTiposAdministracionListQuery : IRequest<IReadOnlyList<TipoAdministracionDto>>
{
}
