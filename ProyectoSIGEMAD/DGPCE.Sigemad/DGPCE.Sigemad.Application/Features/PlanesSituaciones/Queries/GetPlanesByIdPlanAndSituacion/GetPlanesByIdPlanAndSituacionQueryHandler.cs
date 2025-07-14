using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.PlanesEmergencias.Vms;
using DGPCE.Sigemad.Application.Features.PlanesSituaciones.Vms;
using DGPCE.Sigemad.Application.Specifications.PlanesEmergencias;
using DGPCE.Sigemad.Application.Specifications.PlanesSituaciones;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGPCE.Sigemad.Application.Features.PlanesSituaciones.Queries.GetPlanesByIdPlanAndSituacion;
public class GetPlanesByIdPlanAndSituacionQueryHandler : IRequestHandler<GetPlanesByIdPlanAndSituacionQuery, PlanSituacionConFaseVm>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPlanesByIdPlanAndSituacionQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PlanSituacionConFaseVm> Handle(GetPlanesByIdPlanAndSituacionQuery request, CancellationToken cancellationToken)
    {
        var planesSituacionesParams = new PlanesSituacionesParams
        {
            IdPlanEmergencia = request.idPlan,
            SituacionEquivalente = request.situacionEquivalente.ToString(),
        };

        var spec = new PlanesSituacionesSpecification(planesSituacionesParams,true);
        var planesSituaciones = await _unitOfWork.Repository<PlanSituacion>().GetFirstOrDefaultAsync(spec);

        var planesSituacionesVm = _mapper.Map<PlanSituacion, PlanSituacionConFaseVm>(planesSituaciones);
        return planesSituacionesVm;
    }
}
