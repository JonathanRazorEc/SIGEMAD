using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios;
using DGPCE.Sigemad.Application.Specifications.Capacidades;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Capacidades.Queries.GetCapacidadesList;
public class GetCapacidadesListQueryHandler : IRequestHandler<GetCapacidadesListQuery, IReadOnlyList<CapacidadDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCapacidadesListQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<CapacidadDto>> Handle(GetCapacidadesListQuery request, CancellationToken cancellationToken)
    {
        var lista = await _unitOfWork.Repository<Capacidad>().GetAllWithSpec(new GetAllCapacidadesSpecification());
        return _mapper.Map<IReadOnlyList<CapacidadDto>>(lista);
    }
}
