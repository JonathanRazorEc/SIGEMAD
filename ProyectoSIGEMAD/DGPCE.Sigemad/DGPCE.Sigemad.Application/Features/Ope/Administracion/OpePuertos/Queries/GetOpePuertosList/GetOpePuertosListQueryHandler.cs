using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpePuertos.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePuertos;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpePuertos.Queries.GetOpePuertosList
{
    public class GetOpePuertosListQueryHandler : IRequestHandler<GetOpePuertosListQuery, PaginationVm<OpePuertoVm>>
    {
        private readonly ILogger<GetOpePuertosListQueryHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetOpePuertosListQueryHandler(
        ILogger<GetOpePuertosListQueryHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationVm<OpePuertoVm>> Handle(GetOpePuertosListQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(GetOpePuertosListQueryHandler)} - BEGIN");

            var spec = new OpePuertosSpecification(request);
            var opePuertos = await _unitOfWork.Repository<OpePuerto>()
            .GetAllWithSpec(spec);

            var specCount = new OpePuertosForCountingSpecification(request);
            var totalOpePuertos = await _unitOfWork.Repository<OpePuerto>().CountAsync(specCount);
            var opePuertoVmList = _mapper.Map<List<OpePuertoVm>>(opePuertos);

            var rounded = Math.Ceiling(Convert.ToDecimal(totalOpePuertos) / Convert.ToDecimal(request.PageSize));
            var totalPages = Convert.ToInt32(rounded);

            var pagination = new PaginationVm<OpePuertoVm>
            {
                Count = totalOpePuertos,
                Data = opePuertoVmList,
                PageCount = totalPages,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            _logger.LogInformation($"{nameof(GetOpePuertosListQueryHandler)} - END");
            return pagination;
        }


    }
}