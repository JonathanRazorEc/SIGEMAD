using DGPCE.Sigemad.Application.Contracts.Persistence;

using DGPCE.Sigemad.Domain.Modelos;
using MediatR;

namespace DGPCE.Sigemad.Application.Features.TipoDocumentos;
    public class GetTipoDocumentosListQueryHandler : IRequestHandler<GetTipoDocumentosListQuery, IReadOnlyList<TipoDocumento>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTipoDocumentosListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<TipoDocumento>> Handle(GetTipoDocumentosListQuery request, CancellationToken cancellationToken)
        {
            var tipoDocumentos = await _unitOfWork.Repository<TipoDocumento>().GetAllNoTrackingAsync();
            return tipoDocumentos;
        }
    }

