using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeAreasEstacionamiento.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeAreasEstacionamiento;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeAreasEstacionamiento.Queries.GetOpeAreasEstacionamientoList
{
    public class GetOpeAreasEstacionamientoListQueryHandler : IRequestHandler<GetOpeAreasEstacionamientoListQuery, PaginationVm<OpeAreaEstacionamientoVm>>
    {
        private readonly ILogger<GetOpeAreasEstacionamientoListQueryHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetOpeAreasEstacionamientoListQueryHandler(
        ILogger<GetOpeAreasEstacionamientoListQueryHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationVm<OpeAreaEstacionamientoVm>> Handle(GetOpeAreasEstacionamientoListQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(GetOpeAreasEstacionamientoListQueryHandler)} - BEGIN");

            var spec = new OpeAreasEstacionamientoSpecification(request);
            var opeAreasEstacionamiento = await _unitOfWork.Repository<OpeAreaEstacionamiento>()
            .GetAllWithSpec(spec);

            var specCount = new OpeAreasEstacionamientoForCountingSpecification(request);
            var totalOpeAreasEstacionamiento = await _unitOfWork.Repository<OpeAreaEstacionamiento>().CountAsync(specCount);
            var opeAreaEstacionamientoVmList = _mapper.Map<List<OpeAreaEstacionamientoVm>>(opeAreasEstacionamiento);



            var rounded = Math.Ceiling(Convert.ToDecimal(totalOpeAreasEstacionamiento) / Convert.ToDecimal(request.PageSize));
            var totalPages = Convert.ToInt32(rounded);

            var pagination = new PaginationVm<OpeAreaEstacionamientoVm>
            {
                Count = totalOpeAreasEstacionamiento,
                Data = opeAreaEstacionamientoVmList,
                PageCount = totalPages,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            _logger.LogInformation($"{nameof(GetOpeAreasEstacionamientoListQueryHandler)} - END");
            return pagination;
        }


    }
}