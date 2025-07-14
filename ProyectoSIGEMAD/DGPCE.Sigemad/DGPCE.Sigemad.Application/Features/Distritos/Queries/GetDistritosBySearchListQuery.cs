using DGPCE.Sigemad.Application.Features.Distritos.Vms;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Distritos.Queries;
public class GetDistritosBySearchListQuery : IRequest<IReadOnlyList<DistritoVm>>
{
    public int? IdPais { get; set; }
    public string? busqueda { get; set; }


    public GetDistritosBySearchListQuery(int? id, string? busqueda)
    {
        IdPais = id;
        this.busqueda = busqueda;
    }
}
