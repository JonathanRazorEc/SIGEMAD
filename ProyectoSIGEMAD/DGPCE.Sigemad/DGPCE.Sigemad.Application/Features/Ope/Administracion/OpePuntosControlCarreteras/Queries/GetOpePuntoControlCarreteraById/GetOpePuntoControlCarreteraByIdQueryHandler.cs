using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePuntosControlCarreteras;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePuntosControlCarreteras.Queries.GetOpePuntoControlCarreteraById
{
    public class GetOpePuntoControlCarreteraByIdQueryHandler : IRequestHandler<GetOpePuntoControlCarreteraByIdQuery, OpePuntoControlCarretera>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetOpePuntoControlCarreteraByIdQueryHandler> _logger;

        public GetOpePuntoControlCarreteraByIdQueryHandler(
            IUnitOfWork unitOfWork, IMapper mapper,
            ILogger<GetOpePuntoControlCarreteraByIdQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OpePuntoControlCarretera> Handle(GetOpePuntoControlCarreteraByIdQuery request, CancellationToken cancellationToken)
        {
            var opePuntoControlCarreteraParams = new OpePuntosControlCarreterasSpecificationParams
            {
                Id = request.Id,
            };

            var spec = new OpePuntosControlCarreterasSpecification(opePuntoControlCarreteraParams);
            var opePuntoControlCarretera = await _unitOfWork.Repository<OpePuntoControlCarretera>().GetByIdWithSpec(spec);

            if (opePuntoControlCarretera == null)
            {
                _logger.LogWarning($"No se encontro ope punto control de carretera con id: {request.Id}");
                throw new NotFoundException(nameof(OpePuntoControlCarretera), request.Id);
            }

            return opePuntoControlCarretera;
        }
    }
}
