using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.ClasificacionMedios.Quereis.GetClasificacionMediosList;
public class GetClasificacionMediosListQuery : IRequest<IReadOnlyList<ClasificacionMedio>>
{
}
