using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeLogs.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeLogs;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeLogs.Queries.GetOpeLogsList
{
    public class GetOpeLogsListQueryHandler : IRequestHandler<GetOpeLogsListQuery, PaginationVm<OpeLogVm>>
    {
        private readonly ILogger<GetOpeLogsListQueryHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetOpeLogsListQueryHandler(
        ILogger<GetOpeLogsListQueryHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationVm<OpeLogVm>> Handle(GetOpeLogsListQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(GetOpeLogsListQueryHandler)} - BEGIN");

            var spec = new OpeLogsSpecification(request);
            var opeLogs = await _unitOfWork.Repository<OpeLog>()
            .GetAllWithSpec(spec);

            var specCount = new OpeLogsForCountingSpecification(request);
            var totalOpeLogs = await _unitOfWork.Repository<OpeLog>().CountAsync(specCount);
            var opeLogVmList = _mapper.Map<List<OpeLogVm>>(opeLogs);



            var rounded = Math.Ceiling(Convert.ToDecimal(totalOpeLogs) / Convert.ToDecimal(request.PageSize));
            var totalPages = Convert.ToInt32(rounded);

            var pagination = new PaginationVm<OpeLogVm>
            {
                Count = totalOpeLogs,
                Data = opeLogVmList,
                PageCount = totalPages,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            _logger.LogInformation($"{nameof(GetOpeLogsListQueryHandler)} - END");
            return pagination;
        }


    }
}