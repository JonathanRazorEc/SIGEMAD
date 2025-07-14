using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.TiposRegistros.Queries.GetTiposRegistrosList;
public class GetTiposRegistrosListQueryHandler : IRequestHandler<GetTiposRegistrosListQuery, IReadOnlyList<TipoRegistro>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTiposRegistrosListQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<TipoRegistro>> Handle(GetTiposRegistrosListQuery request, CancellationToken cancellationToken)
    {
        var tiposRegistros = await _unitOfWork.Repository<TipoRegistro>().GetAllNoTrackingAsync();

        return tiposRegistros;
    }
}
