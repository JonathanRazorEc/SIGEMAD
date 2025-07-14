using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.TipoDireccionEmergencias.Quereis.GetTipoDireccionEmergenciasList;

public class GetTipoDireccionEmergenciasListQueryHandler : IRequestHandler<GetTipoDireccionEmergenciasListQuery, IReadOnlyList<TipoDireccionEmergencia>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTipoDireccionEmergenciasListQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<TipoDireccionEmergencia>> Handle(GetTipoDireccionEmergenciasListQuery request, CancellationToken cancellationToken)
    {
        var tipoDireccionEmergencias = await _unitOfWork.Repository<TipoDireccionEmergencia>().GetAllNoTrackingAsync();
        return tipoDireccionEmergencias;
    }
}
