using DGPCE.Sigemad.Application.Features.EntidadesMenores.Vms;

using MediatR;

namespace DGPCE.Sigemad.Application.Features.EntidadesMenores.Quereis.GetEntidadMenorList;
public class GetEntidadMenorListQuery : IRequest<IReadOnlyList<EntidadMenorVm>>
{
}
