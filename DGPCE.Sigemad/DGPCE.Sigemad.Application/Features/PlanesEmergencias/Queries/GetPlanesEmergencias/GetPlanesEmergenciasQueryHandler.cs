using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.PlanesEmergencias.Vms;
using DGPCE.Sigemad.Application.Specifications.PlanesEmergencias;
using DGPCE.Sigemad.Application.Specifications.TipoRiesgos;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.PlanesEmergencias.Queries.GetPlanesEmergencias;
public class GetPlanesEmergenciasQueryHandler : IRequestHandler<GetPlanesEmergenciasQuery, IReadOnlyList<PlanEmergenciaVm>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPlanesEmergenciasQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<IReadOnlyList<PlanEmergenciaVm>> Handle(GetPlanesEmergenciasQuery request, CancellationToken cancellationToken)
    {

        if (request.IdTipoSuceso.HasValue)
        {
            TipoRiesgo IdTipoRiesgo = await _unitOfWork.Repository<TipoRiesgo>().GetByIdWithSpec(new TipoRiesgoByIdTipoSucesoSpecification((int)request.IdTipoSuceso));
            request.IdTipoRiesgo = IdTipoRiesgo?.Id ?? 1;
        }

        var spec = new PlanesEmergenciasSpecification(request);
        var planesEmergencias = await _unitOfWork.Repository<PlanEmergencia>().GetAllWithSpec(spec);

        var lista = planesEmergencias.Select(p => new PlanEmergenciaVm
        {
            Id = p.Id,
            Descripcion = request.IsFullDescription ? $"{p.Codigo} - {p.Descripcion}" : p.Descripcion,
        }).ToList();

        return lista;
    }
}
