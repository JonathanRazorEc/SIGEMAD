
using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.PlanesSituaciones.Vms;
using DGPCE.Sigemad.Application.Specifications.PlanesSituaciones;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.PlanesSituaciones.Queries.GetPlanesSituacionesListByIdPlanIdFase;
public class GetPlanesSituacionesByIdPlanIdFaseListQueryHandler : IRequestHandler<GetPlanesSituacionesByIdPlanIdFaseListQuery, IReadOnlyList<PlanSituacionVm>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPlanesSituacionesByIdPlanIdFaseListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<PlanSituacionVm>> Handle(GetPlanesSituacionesByIdPlanIdFaseListQuery request, CancellationToken cancellationToken)
    {

        var planesSituacionesParams = new PlanesSituacionesParams
        {
            IdPlanEmergencia = request.IdPlanEmergencia,
            IdFaseEmergencia = request.IdFaseEmergencia,
        };

        var spec = new PlanesSituacionesSpecification(planesSituacionesParams);
        var planesSituaciones = await _unitOfWork.Repository<PlanSituacion>().GetAllWithSpec(spec);

        var planesSituacionesVm = _mapper.Map<IReadOnlyList<PlanSituacion>, IReadOnlyList<PlanSituacionVm>>(planesSituaciones);
        return planesSituacionesVm;
    }
}
