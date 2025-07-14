using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePeriodos;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePeriodos.Queries.GetOpePeriodoById
{
    public class GetOpePeriodoByIdQueryHandler : IRequestHandler<GetOpePeriodoByIdQuery, OpePeriodo>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetOpePeriodoByIdQueryHandler> _logger;

        public GetOpePeriodoByIdQueryHandler(
            IUnitOfWork unitOfWork, IMapper mapper,
            ILogger<GetOpePeriodoByIdQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OpePeriodo> Handle(GetOpePeriodoByIdQuery request, CancellationToken cancellationToken)
        {
            var periodoParams = new OpePeriodosSpecificationParams
            {
                Id = request.Id,
            };

            var spec = new OpePeriodosSpecification(periodoParams);
            var periodo = await _unitOfWork.Repository<OpePeriodo>().GetByIdWithSpec(spec);

            if (periodo == null)
            {
                _logger.LogWarning($"No se encontro periodo con id: {request.Id}");
                throw new NotFoundException(nameof(OpePeriodo), request.Id);
            }

            return periodo;
        }
    }
}
