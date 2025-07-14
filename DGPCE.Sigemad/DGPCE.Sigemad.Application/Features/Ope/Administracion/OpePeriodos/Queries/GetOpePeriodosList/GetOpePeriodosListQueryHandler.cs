using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpePeriodos.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePeriodos;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePeriodos.Queries.GetOpePeriodosList
{
    public class GetOpePeriodosListQueryHandler : IRequestHandler<GetOpePeriodosListQuery, PaginationVm<OpePeriodoVm>>
    {
        private readonly ILogger<GetOpePeriodosListQueryHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetOpePeriodosListQueryHandler(
        ILogger<GetOpePeriodosListQueryHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationVm<OpePeriodoVm>> Handle(GetOpePeriodosListQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(GetOpePeriodosListQueryHandler)} - BEGIN");

            var spec = new OpePeriodosSpecification(request);
            var opePeriodos = await _unitOfWork.Repository<OpePeriodo>()
            .GetAllWithSpec(spec);

            var specCount = new OpePeriodosForCountingSpecification(request);
            var totalOpePeriodos = await _unitOfWork.Repository<OpePeriodo>().CountAsync(specCount);
            var opePeriodoVmList = _mapper.Map<List<OpePeriodoVm>>(opePeriodos);



            var rounded = Math.Ceiling(Convert.ToDecimal(totalOpePeriodos) / Convert.ToDecimal(request.PageSize));
            var totalPages = Convert.ToInt32(rounded);

            var pagination = new PaginationVm<OpePeriodoVm>
            {
                Count = totalOpePeriodos,
                Data = opePeriodoVmList,
                PageCount = totalPages,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            _logger.LogInformation($"{nameof(GetOpePeriodosListQueryHandler)} - END");
            return pagination;
        }


    }
}