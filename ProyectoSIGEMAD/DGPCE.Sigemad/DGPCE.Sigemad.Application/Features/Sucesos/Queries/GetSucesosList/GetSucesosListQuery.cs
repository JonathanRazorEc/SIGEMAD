using DGPCE.Sigemad.Application.Dtos.Sucesos;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications.Sucesos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Sucesos.Queries.GetSucesosList;
public class GetSucesosListQuery : SucesosSpecificationParams, IRequest<PaginationVm<SucesoGridDto>>
{
}
