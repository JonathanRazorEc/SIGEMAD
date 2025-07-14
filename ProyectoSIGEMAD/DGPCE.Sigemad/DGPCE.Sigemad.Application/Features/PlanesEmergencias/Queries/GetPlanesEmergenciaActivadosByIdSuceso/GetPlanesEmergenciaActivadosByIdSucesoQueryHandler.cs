using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.PlanesEmergencias.Queries.GetPlanesEmergenciasByIdTipoPlan;
using DGPCE.Sigemad.Application.Features.PlanesEmergencias.Vms;
using DGPCE.Sigemad.Application.Specifications.PlanesEmergencias;
using DGPCE.Sigemad.Application.Specifications.Registros;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.PlanesEmergencias.Queries.GetPlanesEmergenciaActivadosByIdSuceso;
public class GetPlanesEmergenciaActivadosByIdSucesoQueryHandler : IRequestHandler<GetPlanesEmergenciaActivadosByIdSucesoQuery, IReadOnlyList<PlanEmergenciaVm>>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPlanesEmergenciaActivadosByIdSucesoQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<IReadOnlyList<PlanEmergenciaVm>> Handle(GetPlanesEmergenciaActivadosByIdSucesoQuery request, CancellationToken cancellationToken)
    {

        var spec = new RegistrosBySucesoPlanesActivosSpecifications(new RegistroSpecificationParams { IdSuceso = request.idSuceso });
        var registros = await _unitOfWork.Repository<Registro>().GetAllWithSpec(spec);

        var activacionesMasRecientes = registros
        .SelectMany(r => r.ActivacionPlanEmergencias)
        .GroupBy(a => a.IdPlanEmergencia)
        .Select(g => g.OrderBy(a => a.FechaHoraInicio).First())
        .ToList();

        var planesEmergencia = activacionesMasRecientes
        .Select(a => a.PlanEmergencia)
        .Distinct()
        .ToList();


        var planesEmergenciaVm = _mapper.Map<IReadOnlyList<PlanEmergencia>, IReadOnlyList<PlanEmergenciaVm>>(planesEmergencia);
        return planesEmergenciaVm;


    }
}
