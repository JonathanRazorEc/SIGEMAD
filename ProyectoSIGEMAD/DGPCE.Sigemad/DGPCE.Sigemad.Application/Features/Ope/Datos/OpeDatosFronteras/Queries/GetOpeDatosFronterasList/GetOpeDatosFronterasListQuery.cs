using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronteras.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeDatosFronteras;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronteras.Queries.GetOpeDatosFronterasList
{
    public class GetOpeDatosFronterasListQuery : OpeDatosFronterasSpecificationParams, IRequest<PaginationVm<OpeDatoFronteraVm>>
    {
    }
}
