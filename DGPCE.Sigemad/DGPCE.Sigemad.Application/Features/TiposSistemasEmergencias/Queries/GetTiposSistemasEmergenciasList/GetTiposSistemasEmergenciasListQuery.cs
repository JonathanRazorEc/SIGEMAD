using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.TiposSistemasEmergencias.Queries.GetTiposSistemasEmergenciasList;
public class GetTiposSistemasEmergenciasListQuery : IRequest<IReadOnlyList<TipoSistemaEmergencia>>
{
}
