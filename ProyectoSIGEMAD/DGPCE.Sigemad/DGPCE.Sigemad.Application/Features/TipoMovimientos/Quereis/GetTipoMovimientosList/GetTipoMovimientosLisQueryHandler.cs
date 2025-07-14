using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;


namespace DGPCE.Sigemad.Application.Features.TipoMovimientos.Quereis.GetTipoMovimientosList
{
   public class GetTipoMovimientosLisQueryHandler : IRequestHandler<GetTipoMovimientosLisQuery, IReadOnlyList<TipoMovimiento>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTipoMovimientosLisQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<TipoMovimiento>> Handle(GetTipoMovimientosLisQuery request, CancellationToken cancellationToken)
        {
            var tipoMovimientos = await _unitOfWork.Repository<TipoMovimiento>().GetAllNoTrackingAsync();
            return tipoMovimientos;
        }
    }
}
