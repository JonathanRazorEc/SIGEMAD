using DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.ProcedenciasMedios.Queries.GetProcedenciasMediosList;
public class GetProcedenciasMediosListQuery : IRequest<IReadOnlyList<ProcedenciaMedioDto>>
{
}
