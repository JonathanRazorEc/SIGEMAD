using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeFronteras;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePuertos;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeFronteras.Queries.GetOpeFronteraById
{
    public class GetOpeFronteraByIdQueryHandler : IRequestHandler<GetOpeFronteraByIdQuery, OpeFrontera>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetOpeFronteraByIdQueryHandler> _logger;

        public GetOpeFronteraByIdQueryHandler(
            IUnitOfWork unitOfWork, IMapper mapper,
            ILogger<GetOpeFronteraByIdQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OpeFrontera> Handle(GetOpeFronteraByIdQuery request, CancellationToken cancellationToken)
        {
            var opeFronteraParams = new OpeFronterasSpecificationParams
            {
                Id = request.Id,
            };

            var spec = new OpeFronterasSpecification(opeFronteraParams);
            var opeFrontera = await _unitOfWork.Repository<OpeFrontera>().GetByIdWithSpec(spec);

            if (opeFrontera == null)
            {
                _logger.LogWarning($"No se encontro ope frontera con id: {request.Id}");
                throw new NotFoundException(nameof(OpeFrontera), request.Id);
            }

            return opeFrontera;
        }
    }
}
