using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.TipoDanios.Queries.GetTipoDaniosList;


public class GetTipoDaniosListQueryHandler : IRequestHandler<GetTipoDaniosListQuery, IReadOnlyList<TipoDanio>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTipoDaniosListQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<TipoDanio>> Handle(GetTipoDaniosListQuery request, CancellationToken cancellationToken)
    {
        var tipoDanios = await _unitOfWork.Repository<TipoDanio>().GetAllNoTrackingAsync();
        return tipoDanios;
    }
}