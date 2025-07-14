using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeLineasMaritimas.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeLineasMaritimas;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeLineasMaritimas.Queries.GetOpeLineasMaritimasList
{
    public class GetOpeLineasMaritimasListQueryHandler : IRequestHandler<GetOpeLineasMaritimasListQuery, PaginationVm<OpeLineaMaritimaVm>>
    {
        private readonly ILogger<GetOpeLineasMaritimasListQueryHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetOpeLineasMaritimasListQueryHandler(
        ILogger<GetOpeLineasMaritimasListQueryHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationVm<OpeLineaMaritimaVm>> Handle(GetOpeLineasMaritimasListQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(GetOpeLineasMaritimasListQueryHandler)} - BEGIN");

            var spec = new OpeLineasMaritimasSpecification(request);
            var opeLineasMaritimas = await _unitOfWork.Repository<OpeLineaMaritima>()
            .GetAllWithSpec(spec);

            var specCount = new OpeLineasMaritimasForCountingSpecification(request);
            var totalOpeLineasMaritimas = await _unitOfWork.Repository<OpeLineaMaritima>().CountAsync(specCount);
            var opeLineaMaritimaVmList = _mapper.Map<List<OpeLineaMaritimaVm>>(opeLineasMaritimas);



            var rounded = Math.Ceiling(Convert.ToDecimal(totalOpeLineasMaritimas) / Convert.ToDecimal(request.PageSize));
            var totalPages = Convert.ToInt32(rounded);

            var pagination = new PaginationVm<OpeLineaMaritimaVm>
            {
                Count = totalOpeLineasMaritimas,
                Data = opeLineaMaritimaVmList,
                PageCount = totalPages,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            _logger.LogInformation($"{nameof(GetOpeLineasMaritimasListQueryHandler)} - END");
            return pagination;
        }


    }
}