using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeAreasDescanso;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePuertos;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeAreasDescanso.Queries.GetOpeAreaDescansoById
{
    public class GetOpeAreaDescansoByIdQueryHandler : IRequestHandler<GetOpeAreaDescansoByIdQuery, OpeAreaDescanso>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetOpeAreaDescansoByIdQueryHandler> _logger;

        public GetOpeAreaDescansoByIdQueryHandler(
            IUnitOfWork unitOfWork, IMapper mapper,
            ILogger<GetOpeAreaDescansoByIdQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OpeAreaDescanso> Handle(GetOpeAreaDescansoByIdQuery request, CancellationToken cancellationToken)
        {
            var opeAreaDescansoParams = new OpeAreasDescansoSpecificationParams
            {
                Id = request.Id,
            };

            var spec = new OpeAreasDescansoSpecification(opeAreaDescansoParams);
            var opeAreaDescanso = await _unitOfWork.Repository<OpeAreaDescanso>().GetByIdWithSpec(spec);

            if (opeAreaDescanso == null)
            {
                _logger.LogWarning($"No se encontro ope area descanso con id: {request.Id}");
                throw new NotFoundException(nameof(OpeAreaDescanso), request.Id);
            }

            return opeAreaDescanso;
        }
    }
}
