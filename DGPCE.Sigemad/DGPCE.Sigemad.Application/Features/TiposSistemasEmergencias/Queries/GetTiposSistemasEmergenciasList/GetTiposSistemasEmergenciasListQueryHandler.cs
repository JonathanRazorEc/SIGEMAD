using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.TiposSistemasEmergencias.Queries.GetTiposSistemasEmergenciasList;


public class GetTiposSistemasEmergenciasListQueryHandler : IRequestHandler<GetTiposSistemasEmergenciasListQuery, IReadOnlyList<TipoSistemaEmergencia>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTiposSistemasEmergenciasListQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<TipoSistemaEmergencia>> Handle(GetTiposSistemasEmergenciasListQuery request, CancellationToken cancellationToken)
    {
        var tiposFasesEmergencias = await _unitOfWork.Repository<TipoSistemaEmergencia>().GetAllNoTrackingAsync();
        return tiposFasesEmergencias;
    }
}
