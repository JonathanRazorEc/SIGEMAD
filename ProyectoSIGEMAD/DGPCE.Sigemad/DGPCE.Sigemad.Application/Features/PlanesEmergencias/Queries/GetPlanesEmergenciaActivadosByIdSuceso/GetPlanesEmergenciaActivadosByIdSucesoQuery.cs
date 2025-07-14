using DGPCE.Sigemad.Application.Features.PlanesEmergencias.Vms;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.PlanesEmergencias.Queries.GetPlanesEmergenciaActivadosByIdSuceso;
public class GetPlanesEmergenciaActivadosByIdSucesoQuery : IRequest<IReadOnlyList<PlanEmergenciaVm>>
{
    public int idSuceso { get; set; }

    public GetPlanesEmergenciaActivadosByIdSucesoQuery(int id)
    {
        idSuceso = id;
    }
}
