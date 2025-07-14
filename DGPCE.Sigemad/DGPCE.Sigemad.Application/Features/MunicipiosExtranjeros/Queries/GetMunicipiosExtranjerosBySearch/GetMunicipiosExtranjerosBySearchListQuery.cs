using DGPCE.Sigemad.Application.Features.MunicipiosExtranjeros.Vms;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.MunicipiosExtranjeros.Queries.GetMunicipiosExtranjerosBySearch;
public class GetMunicipiosExtranjerosBySearchListQuery : IRequest<IReadOnlyList<MunicipioExtranjeroVm>>
{
    public int? IdDistrito { get; set; }
    public int? IdPais { get; set; }
    public string? busqueda { get; set; }


    public GetMunicipiosExtranjerosBySearchListQuery(int? id, int? idPais, string? busqueda)
    {
        IdDistrito = id;
        this.busqueda = busqueda;
        IdPais = idPais;
    }



}
