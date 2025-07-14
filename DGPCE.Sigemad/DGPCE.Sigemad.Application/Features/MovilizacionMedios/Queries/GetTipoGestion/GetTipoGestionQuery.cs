using DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios;
using DGPCE.Sigemad.Application.Specifications.MovilizacionMedios;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.MovilizacionMedios.Queries.GetTipoGestion;
public class GetTipoGestionQuery: FlujoPasoMovilizacionParams, IRequest<IReadOnlyList<TipoGestionDto>>
{
}
