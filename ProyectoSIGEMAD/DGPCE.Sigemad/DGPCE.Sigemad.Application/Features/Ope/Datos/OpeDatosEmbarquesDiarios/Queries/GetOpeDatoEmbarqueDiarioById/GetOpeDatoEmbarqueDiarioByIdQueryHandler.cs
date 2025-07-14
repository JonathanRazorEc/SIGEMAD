using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeDatosEmbarquesDiarios;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosEmbarquesDiarios.Queries.GetOpeDatoEmbarqueDiarioById
{
    public class GetOpeDatoEmbarqueDiarioByIdQueryHandler : IRequestHandler<GetOpeDatoEmbarqueDiarioByIdQuery, OpeDatoEmbarqueDiario>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetOpeDatoEmbarqueDiarioByIdQueryHandler> _logger;

        public GetOpeDatoEmbarqueDiarioByIdQueryHandler(
            IUnitOfWork unitOfWork, IMapper mapper,
            ILogger<GetOpeDatoEmbarqueDiarioByIdQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OpeDatoEmbarqueDiario> Handle(GetOpeDatoEmbarqueDiarioByIdQuery request, CancellationToken cancellationToken)
        {
            var opeDatoEmbarqueDiarioParams = new OpeDatosEmbarquesDiariosSpecificationParams
            {
                Id = request.Id,
            };

            var spec = new OpeDatosEmbarquesDiariosSpecification(opeDatoEmbarqueDiarioParams);
            var opeDatoEmbarqueDiario = await _unitOfWork.Repository<OpeDatoEmbarqueDiario>().GetByIdWithSpec(spec);

            if (opeDatoEmbarqueDiario == null)
            {
                _logger.LogWarning($"No se encontro dato de embarque diario de OPE con id: {request.Id}");
                throw new NotFoundException(nameof(OpeDatoEmbarqueDiario), request.Id);
            }

            return opeDatoEmbarqueDiario;
        }
    }
}
