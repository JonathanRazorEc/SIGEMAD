using DGPCE.Sigemad.Application.Features.Provincias.Vms;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Provincias.Queries.GetProvinciasByIdCCAAList
{
    public class GetProvinciasByIdCCAAListQuery : IRequest<IReadOnlyList<ProvinciaSinMunicipiosVm>>
    {
        public int IdCcaa { get; set; }


        public GetProvinciasByIdCCAAListQuery(int id)
        {
            IdCcaa = id;
        }
    }
}

