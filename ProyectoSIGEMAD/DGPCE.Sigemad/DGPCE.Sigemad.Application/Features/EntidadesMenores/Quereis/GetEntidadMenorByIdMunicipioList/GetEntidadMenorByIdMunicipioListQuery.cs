using DGPCE.Sigemad.Application.Features.EntidadesMenores.Vms;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.EntidadesMenores.Quereis.GetEntidadMenorByIdMunicipioList;
public class GetEntidadMenorByIdMunicipioListQuery : IRequest<IReadOnlyList<EntidadMenorVm>>
{
    public int IdMunicipio { get; set; }

    public GetEntidadMenorByIdMunicipioListQuery(int idMunicipio)
    {
        IdMunicipio = idMunicipio;
    }
}