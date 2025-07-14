
using DGPCE.Sigemad.Application.Features.Evoluciones.Vms;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.Evoluciones.Quereis.GetEvolucionesByIdIncendioList
{
    public class GetEvolucionesByIdSucesoListQuery : IRequest<IReadOnlyList<EvolucionVm>>
    {
        public int IdSuceso { get; set; }


        public GetEvolucionesByIdSucesoListQuery(int id)
        {
            IdSuceso = id;
        }
    }
}
