using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePorcentajesOcupacionAreasEstacionamiento;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePorcentsOcAE.Queries.GetOpePorcentajeOcupacionAreaEstacionamientoById
{
    public class GetOpePorcentOcAEByIdQueryHandler : IRequestHandler<GetOpePorcentOcAEByIdQuery, OpePorcentajeOcupacionAreaEstacionamiento>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetOpePorcentOcAEByIdQueryHandler> _logger;

        public GetOpePorcentOcAEByIdQueryHandler(
            IUnitOfWork unitOfWork, IMapper mapper,
            ILogger<GetOpePorcentOcAEByIdQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OpePorcentajeOcupacionAreaEstacionamiento> Handle(GetOpePorcentOcAEByIdQuery request, CancellationToken cancellationToken)
        {
            var opePorcentajeOcupacionAreaEstacionamientoParams = new OpePorcentajesOcupacionAreasEstacionamientoSpecificationParams
            {
                Id = request.Id,
            };

            var spec = new OpePorcentajesOcupacionAreasEstacionamientoSpecification(opePorcentajeOcupacionAreaEstacionamientoParams);
            var opePorcentajeOcupacionAreaEstacionamiento = await _unitOfWork.Repository<OpePorcentajeOcupacionAreaEstacionamiento>().GetByIdWithSpec(spec);

            if (opePorcentajeOcupacionAreaEstacionamiento == null)
            {
                _logger.LogWarning($"No se encontro ope porcentaje área estacionamiento con id: {request.Id}");
                throw new NotFoundException(nameof(OpePorcentajeOcupacionAreaEstacionamiento), request.Id);
            }

            return opePorcentajeOcupacionAreaEstacionamiento;
        }
    }
}
