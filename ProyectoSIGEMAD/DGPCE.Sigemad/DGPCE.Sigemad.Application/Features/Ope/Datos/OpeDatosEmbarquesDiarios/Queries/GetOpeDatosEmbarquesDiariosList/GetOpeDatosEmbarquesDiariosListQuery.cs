using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosEmbarquesDiarios.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeDatosEmbarquesDiarios;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosEmbarquesDiarios.Queries.GetOpeDatosEmbarquesDiariosList
{
    public class GetOpeDatosEmbarquesDiariosListQuery : OpeDatosEmbarquesDiariosSpecificationParams, IRequest<PaginationVm<OpeDatoEmbarqueDiarioVm>>
    {
    }
}
