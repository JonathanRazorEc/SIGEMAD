using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeFronteras.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeFronteras;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeFronteras.Queries.GetOpeFronterasList
{

    public class GetOpeFronterasListQuery : OpeFronterasSpecificationParams, IRequest<PaginationVm<OpeFronteraVm>>
    {
    }
}
