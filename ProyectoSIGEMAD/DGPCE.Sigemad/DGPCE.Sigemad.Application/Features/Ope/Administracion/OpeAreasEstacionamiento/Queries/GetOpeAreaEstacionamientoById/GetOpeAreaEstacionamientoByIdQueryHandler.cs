using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeAreasEstacionamiento;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePuertos;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeAreasEstacionamiento.Queries.GetOpeAreaEstacionamientoById
{
    public class GetOpeAreaEstacionamientoByIdQueryHandler : IRequestHandler<GetOpeAreaEstacionamientoByIdQuery, OpeAreaEstacionamiento>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetOpeAreaEstacionamientoByIdQueryHandler> _logger;

        public GetOpeAreaEstacionamientoByIdQueryHandler(
            IUnitOfWork unitOfWork, IMapper mapper,
            ILogger<GetOpeAreaEstacionamientoByIdQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OpeAreaEstacionamiento> Handle(GetOpeAreaEstacionamientoByIdQuery request, CancellationToken cancellationToken)
        {
            var opeAreaEstacionamientoParams = new OpeAreasEstacionamientoSpecificationParams
            {
                Id = request.Id,
            };

            var spec = new OpeAreasEstacionamientoSpecification(opeAreaEstacionamientoParams);
            var opeAreaEstacionamiento = await _unitOfWork.Repository<OpeAreaEstacionamiento>().GetByIdWithSpec(spec);

            if (opeAreaEstacionamiento == null)
            {
                _logger.LogWarning($"No se encontro ope área de estacionamiento con id: {request.Id}");
                throw new NotFoundException(nameof(OpeAreaEstacionamiento), request.Id);
            }

            return opeAreaEstacionamiento;
        }
    }
}
