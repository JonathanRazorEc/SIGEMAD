using DGPCE.Sigemad.Application.Features.ImpactosClasificados.Vms;
using DGPCE.Sigemad.Application.Specifications.Impactos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.ImpactosClasificados.Queries.GetDescripcionImpactosList;
public class GetDescripcionImpactosListQuery : ImpactosClasificadosSpecificationParams, IRequest<IReadOnlyList<ImpactoVm>>
{
}

