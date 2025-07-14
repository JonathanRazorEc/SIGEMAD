using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.Evoluciones.Vms;
using DGPCE.Sigemad.Application.Helpers;
using DGPCE.Sigemad.Application.Specifications.Incendios;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DGPCE.Sigemad.Application.Features.Incendios.Queries.GetIncendiosNacionalesById
{
    public class GetIncendiosNacionalesByIdQueryHandler : IRequestHandler<GetIncendiosNacionalesByIdQuery, Incendio>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetIncendiosNacionalesByIdQueryHandler> _logger;

        public GetIncendiosNacionalesByIdQueryHandler(
            IUnitOfWork unitOfWork, IMapper mapper,
            ILogger<GetIncendiosNacionalesByIdQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Incendio> Handle(GetIncendiosNacionalesByIdQuery request, CancellationToken cancellationToken)
        {
            var incendioParams = new IncendiosSpecificationParams
            {
               Id = request.Id,
               //IdTerritorio =  1,
            };

            var spec = new IncendiosSpecification(incendioParams);
            var incendio = await _unitOfWork.Repository<Incendio>().GetByIdWithSpec(spec);

            if (incendio == null)
            {
                _logger.LogWarning($"No se encontro incendio con id: {request.Id}");
                throw new NotFoundException(nameof(Incendio), request.Id);
            }

            incendio.Ubicacion = IncendioUtils.ObtenerUbicacion(incendio);


            return incendio;
        }
    }
}
