using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeLogs.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeLogs;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeLogs.Queries.GetOpeLogsList
{

    public class GetOpeLogsListQuery : OpeLogsSpecificationParams, IRequest<PaginationVm<OpeLogVm>>
    {
    }
}
