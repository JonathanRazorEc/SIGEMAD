using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeFronteras.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeFronteras;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeFronteras.Queries.GetOpeFronterasList
{
    public class GetOpeFronterasListQueryHandler : IRequestHandler<GetOpeFronterasListQuery, PaginationVm<OpeFronteraVm>>
    {
        private readonly ILogger<GetOpeFronterasListQueryHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetOpeFronterasListQueryHandler(
        ILogger<GetOpeFronterasListQueryHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationVm<OpeFronteraVm>> Handle(GetOpeFronterasListQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(GetOpeFronterasListQueryHandler)} - BEGIN");

            var spec = new OpeFronterasSpecification(request);
            var opeFronteras = await _unitOfWork.Repository<OpeFrontera>()
            .GetAllWithSpec(spec);

            var specCount = new OpeFronterasForCountingSpecification(request);
            var totalOpeFronteras = await _unitOfWork.Repository<OpeFrontera>().CountAsync(specCount);
            var opeFronteraVmList = _mapper.Map<List<OpeFronteraVm>>(opeFronteras);



            var rounded = Math.Ceiling(Convert.ToDecimal(totalOpeFronteras) / Convert.ToDecimal(request.PageSize));
            var totalPages = Convert.ToInt32(rounded);

            var pagination = new PaginationVm<OpeFronteraVm>
            {
                Count = totalOpeFronteras,
                Data = opeFronteraVmList,
                PageCount = totalPages,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            _logger.LogInformation($"{nameof(GetOpeFronterasListQueryHandler)} - END");
            return pagination;
        }


    }
}