using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePuntosControlCarreteras.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePuntosControlCarreteras;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePuntosControlCarreteras.Queries.GetOpePuntosControlCarreterasList
{
    public class GetOpePuntosControlCarreterasListQueryHandler : IRequestHandler<GetOpePuntosControlCarreterasListQuery, PaginationVm<OpePuntoControlCarreteraVm>>
    {
        private readonly ILogger<GetOpePuntosControlCarreterasListQueryHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetOpePuntosControlCarreterasListQueryHandler(
        ILogger<GetOpePuntosControlCarreterasListQueryHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationVm<OpePuntoControlCarreteraVm>> Handle(GetOpePuntosControlCarreterasListQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(GetOpePuntosControlCarreterasListQueryHandler)} - BEGIN");

            var spec = new OpePuntosControlCarreterasSpecification(request);
            var opePuntosControlCarreteras = await _unitOfWork.Repository<OpePuntoControlCarretera>()
            .GetAllWithSpec(spec);

            var specCount = new OpePuntosControlCarreterasForCountingSpecification(request);
            var totalOpePuntosControlCarreteras = await _unitOfWork.Repository<OpePuntoControlCarretera>().CountAsync(specCount);
            var opePuntoControlCarreteraVmList = _mapper.Map<List<OpePuntoControlCarreteraVm>>(opePuntosControlCarreteras);



            var rounded = Math.Ceiling(Convert.ToDecimal(totalOpePuntosControlCarreteras) / Convert.ToDecimal(request.PageSize));
            var totalPages = Convert.ToInt32(rounded);

            var pagination = new PaginationVm<OpePuntoControlCarreteraVm>
            {
                Count = totalOpePuntosControlCarreteras,
                Data = opePuntoControlCarreteraVmList,
                PageCount = totalPages,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            _logger.LogInformation($"{nameof(GetOpePuntosControlCarreterasListQueryHandler)} - END");
            return pagination;
        }


    }
}