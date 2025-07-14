using DGPCE.Sigemad.Application.Features.CCAA.Vms;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.CCAA.Queries.GetCCAABySearch;

public class GetCCAABySearchListQuery : IRequest<IReadOnlyList<ComunidadesAutonomasConPaisVm>>
{

    public int? IdPais { get; set; }
    public string? busqueda { get; set; }


    public GetCCAABySearchListQuery(int? id, string? busqueda)
    {
        IdPais = id;
        this.busqueda = busqueda;
    }

}