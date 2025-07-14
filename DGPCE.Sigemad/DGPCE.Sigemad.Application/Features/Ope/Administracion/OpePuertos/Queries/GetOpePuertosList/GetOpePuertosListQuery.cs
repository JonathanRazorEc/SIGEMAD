using DGPCE.Sigemad.Application.Features.Ope.Datos.OpePuertos.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePuertos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpePuertos.Queries.GetOpePuertosList
{

    public class GetOpePuertosListQuery : OpePuertosSpecificationParams, IRequest<PaginationVm<OpePuertoVm>>
    {
    }
}
