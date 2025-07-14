using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeDatosFronteras;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronteras.Queries.GetOpeDatoFronteraById
{
    public class GetOpeDatoFronteraByIdQueryHandler : IRequestHandler<GetOpeDatoFronteraByIdQuery, OpeDatoFrontera>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetOpeDatoFronteraByIdQueryHandler> _logger;

        public GetOpeDatoFronteraByIdQueryHandler(
            IUnitOfWork unitOfWork, IMapper mapper,
            ILogger<GetOpeDatoFronteraByIdQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OpeDatoFrontera> Handle(GetOpeDatoFronteraByIdQuery request, CancellationToken cancellationToken)
        {
            var opeDatoFronteraParams = new OpeDatosFronterasSpecificationParams
            {
                Id = request.Id,
            };

            var spec = new OpeDatosFronterasSpecification(opeDatoFronteraParams);
            var opeDatoFrontera = await _unitOfWork.Repository<OpeDatoFrontera>().GetByIdWithSpec(spec);

            if (opeDatoFrontera == null)
            {
                _logger.LogWarning($"No se encontro dato de frontera de OPE con id: {request.Id}");
                throw new NotFoundException(nameof(OpeDatoFrontera), request.Id);
            }

            return opeDatoFrontera;
        }
    }
}
