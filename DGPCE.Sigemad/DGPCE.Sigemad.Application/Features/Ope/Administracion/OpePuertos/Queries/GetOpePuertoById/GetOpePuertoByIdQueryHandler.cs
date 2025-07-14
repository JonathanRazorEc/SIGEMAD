using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePuertos;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpePuertos.Queries.GetOpePuertoById
{
    public class GetOpePuertoByIdQueryHandler : IRequestHandler<GetOpePuertoByIdQuery, OpePuerto>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetOpePuertoByIdQueryHandler> _logger;

        public GetOpePuertoByIdQueryHandler(
            IUnitOfWork unitOfWork, IMapper mapper,
            ILogger<GetOpePuertoByIdQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OpePuerto> Handle(GetOpePuertoByIdQuery request, CancellationToken cancellationToken)
        {
            var opePuertoParams = new OpePuertosSpecificationParams
            {
                Id = request.Id,
            };

            var spec = new OpePuertosSpecification(opePuertoParams);
            var opePuerto = await _unitOfWork.Repository<OpePuerto>().GetByIdWithSpec(spec);

            if (opePuerto == null)
            {
                _logger.LogWarning($"No se encontro ope puerto con id: {request.Id}");
                throw new NotFoundException(nameof(OpePuerto), request.Id);
            }

            return opePuerto;
        }
    }
}
