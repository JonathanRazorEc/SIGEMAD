using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.TitularidadMedios.Quereis.GetTitularidadMediosList;
internal class GetTitularidadMediosListQueryHandler : IRequestHandler<GetTitularidadMediosListQuery, IReadOnlyList<TitularidadMedio>>
{

    private readonly IUnitOfWork _unitOfWork;

    public GetTitularidadMediosListQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<TitularidadMedio>> Handle(GetTitularidadMediosListQuery request, CancellationToken cancellationToken)
    {
        var titularidadMedios = await _unitOfWork.Repository<TitularidadMedio>().GetAllNoTrackingAsync();
        return titularidadMedios;
    }
}
