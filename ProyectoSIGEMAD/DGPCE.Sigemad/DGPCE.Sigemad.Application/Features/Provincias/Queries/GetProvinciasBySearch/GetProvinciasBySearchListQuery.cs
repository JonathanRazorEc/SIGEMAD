using DGPCE.Sigemad.Application.Features.Provincias.Vms;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.Provincias.Queries.GetProvinciasBySearch;
public class GetProvinciasBySearchListQuery : IRequest<IReadOnlyList<ProvinciasConCCAAVm>>
{

    public int? idComunidadAutonoma { get; set; }
    public string? busqueda { get; set; }


    public GetProvinciasBySearchListQuery(int? id, string? busqueda)
    {
        idComunidadAutonoma = id;
        this.busqueda = busqueda;
    }

}
