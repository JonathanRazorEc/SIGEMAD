using DGPCE.Sigemad.Application.Features.Ope.Datos.OpePeriodos.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePeriodos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePeriodos.Queries.GetOpePeriodosList
{

    public class GetOpePeriodosListQuery : OpePeriodosSpecificationParams, IRequest<PaginationVm<OpePeriodoVm>>
    {
    }
}
