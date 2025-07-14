using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.SituacionesOperativas.Queries.GetSituacionesOperativasList;
public class GetSituacionesOperativasListQueryHandler : IRequestHandler<GetSituacionesOperativasListQuery, IReadOnlyList<SituacionOperativa>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetSituacionesOperativasListQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<SituacionOperativa>> Handle(GetSituacionesOperativasListQuery request, CancellationToken cancellationToken)
    {
        var lista = await _unitOfWork.Repository<SituacionOperativa>().GetAllNoTrackingAsync();
        return lista;
    }
}
