using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeAreasDescanso.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeAreasDescanso;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeAreasDescanso.Queries.GetOpeAreasDescansoList
{

    public class GetOpeAreasDescansoListQuery : OpeAreasDescansoSpecificationParams, IRequest<PaginationVm<OpeAreaDescansoVm>>
    {
    }
}
