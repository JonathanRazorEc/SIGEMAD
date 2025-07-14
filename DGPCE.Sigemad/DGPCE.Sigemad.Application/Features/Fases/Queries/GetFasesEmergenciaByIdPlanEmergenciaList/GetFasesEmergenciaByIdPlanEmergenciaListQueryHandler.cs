using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Fases.Vms;
using DGPCE.Sigemad.Application.Specifications.FasesEmergencia;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Fases.Queries.GetFasesEmergenciaListByIdPlanEmergencia;

public class GetFasesEmergenciaByIdPlanEmergenciaListQueryHandler : IRequestHandler<GetFasesEmergenciaByIdPlanEmergenciaListQuery, IReadOnlyList<FaseEmergenciaVm>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetFasesEmergenciaByIdPlanEmergenciaListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<FaseEmergenciaVm>> Handle(GetFasesEmergenciaByIdPlanEmergenciaListQuery request, CancellationToken cancellationToken)
    {

        var faseEmergenciaParams = new FasesEmergenciaParams
        {
            IdPlanEmergencia = request.IdPlanEmergencia,
        };

        var spec = new FasesEmergenciaSpecification(faseEmergenciaParams);
        var fasesEmergencias = await _unitOfWork.Repository<FaseEmergencia>().GetAllWithSpec(spec);

        var fasesEmergenciaVm = _mapper.Map<IReadOnlyList<FaseEmergencia>, IReadOnlyList<FaseEmergenciaVm>>(fasesEmergencias);
        return fasesEmergenciaVm;
    }
}