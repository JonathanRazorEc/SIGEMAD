using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.ProcedenciasMedios.Queries.GetProcedenciasMediosList;
public class GetProcedenciasMediosListQueryHandler : IRequestHandler<GetProcedenciasMediosListQuery, IReadOnlyList<ProcedenciaMedioDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProcedenciasMediosListQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<ProcedenciaMedioDto>> Handle(GetProcedenciasMediosListQuery request, CancellationToken cancellationToken)
    {
        var lista = await _unitOfWork.Repository<ProcedenciaMedio>().GetAllNoTrackingAsync();
        return _mapper.Map<IReadOnlyList<ProcedenciaMedioDto>>(lista);
    }
}
