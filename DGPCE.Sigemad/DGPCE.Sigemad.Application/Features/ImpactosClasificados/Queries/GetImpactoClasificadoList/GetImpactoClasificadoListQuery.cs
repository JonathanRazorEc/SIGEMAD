using DGPCE.Sigemad.Application.Features.ImpactosClasificados.Vms;

using MediatR;

namespace DGPCE.Sigemad.Application.Features.ImpactosClasificados.Queries.GetImpactoClasificadoList;
public class GetImpactoClasificadoListQuery : IRequest<IReadOnlyList<ImpactoClasificadoConTipoImpactoVM>>
{
    public int? IdTipoImpacto { get; set; }
    public bool? nuclear { get; set; }
    public string? busqueda { get; set; } = null;

    public GetImpactoClasificadoListQuery(int? id, bool? nuclear,string? busqueda)
    {
        IdTipoImpacto = id;
        this.nuclear = nuclear;
        this.busqueda = busqueda;
    }
}
