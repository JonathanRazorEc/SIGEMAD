using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.NivelesGravedad.Queries.GetNivelesGravedadList
{
    public class GetNivelesGravedadListQueryHandler : IRequestHandler<GetNivelesGravedadListQuery, IReadOnlyList<NivelGravedad>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetNivelesGravedadListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<NivelGravedad>> Handle(GetNivelesGravedadListQuery request, CancellationToken cancellationToken)
        {
            var nivelesGravedad = await _unitOfWork.Repository<NivelGravedad>().GetAllNoTrackingAsync();
            return nivelesGravedad;
        }
    }
}
