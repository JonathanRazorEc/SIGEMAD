using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeLineasMaritimas;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeLineasMaritimas.Queries.GetOpeLineaMaritimaById
{
    public class GetOpeLineaMaritimaByIdQueryHandler : IRequestHandler<GetOpeLineaMaritimaByIdQuery, OpeLineaMaritima>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetOpeLineaMaritimaByIdQueryHandler> _logger;

        public GetOpeLineaMaritimaByIdQueryHandler(
            IUnitOfWork unitOfWork, IMapper mapper,
            ILogger<GetOpeLineaMaritimaByIdQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OpeLineaMaritima> Handle(GetOpeLineaMaritimaByIdQuery request, CancellationToken cancellationToken)
        {
            var opeLineaMaritimaParams = new OpeLineasMaritimasSpecificationParams
            {
                Id = request.Id,
            };

            var spec = new OpeLineasMaritimasSpecification(opeLineaMaritimaParams);
            var opeLineaMaritima = await _unitOfWork.Repository<OpeLineaMaritima>().GetByIdWithSpec(spec);

            if (opeLineaMaritima == null)
            {
                _logger.LogWarning($"No se encontro línea marítima con id: {request.Id}");
                throw new NotFoundException(nameof(OpeLineaMaritima), request.Id);
            }

            return opeLineaMaritima;
        }
    }
}
