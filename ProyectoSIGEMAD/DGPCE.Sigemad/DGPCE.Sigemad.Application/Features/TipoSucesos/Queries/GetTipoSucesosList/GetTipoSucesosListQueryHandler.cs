using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.TipoSucesos.Queries.GetTipoSucesosList
{
    public class GetTipoSucesosListQueryHandler : IRequestHandler<GetTipoSucesosListQuery, IReadOnlyList<TipoSuceso>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTipoSucesosListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<TipoSuceso>> Handle(GetTipoSucesosListQuery request, CancellationToken cancellationToken)
        {
            var tipoSucesos = await _unitOfWork.Repository<TipoSuceso>().GetAllNoTrackingAsync();
            return tipoSucesos;
        }
    }
}
