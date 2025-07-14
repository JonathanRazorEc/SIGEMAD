using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Specifications.TipoPlanes;
using DGPCE.Sigemad.Application.Specifications.TipoRiesgos;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.TipoPlanes.Quereis.GetTipoPlanesList;
public class GetTipoPlanesListQueryHandler : IRequestHandler<GetTipoPlanesListQuery, IReadOnlyList<TipoPlan>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTipoPlanesListQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<TipoPlan>> Handle(GetTipoPlanesListQuery request, CancellationToken cancellationToken)
    {


        TipoRiesgo IdTipoRiesgo = await _unitOfWork.Repository<TipoRiesgo>().GetByIdWithSpec(new TipoRiesgoByIdTipoSucesoSpecification(request.IdTipoSuceso));
        
        var planesEmergencia = await _unitOfWork.Repository<PlanEmergencia>().GetAllWithSpec(new TipoPlanesSpecifications(request.IdAmbito, IdTipoRiesgo?.Id ?? 1));

        var tipoPlanes = planesEmergencia.Select(p => p.TipoPlan).Distinct().ToList();

        return tipoPlanes;



    }
}