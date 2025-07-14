using DGPCE.Sigemad.Application.Dtos.CaracterMedios;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.CaracterMedios.Quereis.GetCaracterMediosList;
public class GetCaracterMediosListQuery : IRequest<IReadOnlyList<CaracterMedioDto>>
{
}
