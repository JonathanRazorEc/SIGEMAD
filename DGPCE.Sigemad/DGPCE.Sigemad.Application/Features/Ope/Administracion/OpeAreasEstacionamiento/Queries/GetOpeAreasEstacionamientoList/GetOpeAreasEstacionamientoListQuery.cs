using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeAreasEstacionamiento.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeAreasEstacionamiento;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeAreasEstacionamiento.Queries.GetOpeAreasEstacionamientoList
{

    public class GetOpeAreasEstacionamientoListQuery : OpeAreasEstacionamientoSpecificationParams, IRequest<PaginationVm<OpeAreaEstacionamientoVm>>
    {
    }
}
