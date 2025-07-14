using DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.DestinosMedios.Queries.GetDestinoMediosList;
public class GetDestinoMediosListQuery : IRequest<IReadOnlyList<DestinoMedioDto>>
{
}
