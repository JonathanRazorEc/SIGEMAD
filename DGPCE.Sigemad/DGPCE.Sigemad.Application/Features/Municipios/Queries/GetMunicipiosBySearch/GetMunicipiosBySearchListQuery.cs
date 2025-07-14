using DGPCE.Sigemad.Application.Features.Municipios.Vms;
using MediatR;
namespace DGPCE.Sigemad.Application.Features.Municipios.Queries.GetMunicipiosBySearch;

public class GetMunicipiosBySearchListQuery : IRequest<IReadOnlyList<MunicipiosConProvinciaVM>>
{

    public int? IdProvincia { get; set; }
    public string? busqueda { get; set; }


    public GetMunicipiosBySearchListQuery(int? id, string? busqueda)
    {
        IdProvincia = id;
        this.busqueda = busqueda;
    }

}