using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.Superficies.Queries.GetSuperficiesList;
public class GetSuperficieListQueryHandler : IRequestHandler<GetSuperficieListQuery, IReadOnlyList<SuperficieFiltro>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetSuperficieListQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<SuperficieFiltro>> Handle(GetSuperficieListQuery request, CancellationToken cancellationToken)
    {
        var lista = await _unitOfWork.Repository<SuperficieFiltro>().GetAllNoTrackingAsync();
        return lista;
    }
}
