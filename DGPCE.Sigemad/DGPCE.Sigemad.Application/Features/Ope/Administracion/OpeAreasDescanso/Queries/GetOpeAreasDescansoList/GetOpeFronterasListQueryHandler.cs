using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeAreasDescanso.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpeAreasDescanso;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeAreasDescanso.Queries.GetOpeAreasDescansoList
{
    public class GetOpeAreasDescansoListQueryHandler : IRequestHandler<GetOpeAreasDescansoListQuery, PaginationVm<OpeAreaDescansoVm>>
    {
        private readonly ILogger<GetOpeAreasDescansoListQueryHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetOpeAreasDescansoListQueryHandler(
        ILogger<GetOpeAreasDescansoListQueryHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationVm<OpeAreaDescansoVm>> Handle(GetOpeAreasDescansoListQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(GetOpeAreasDescansoListQueryHandler)} - BEGIN");

            var spec = new OpeAreasDescansoSpecification(request);
            var opeAreasDescanso = await _unitOfWork.Repository<OpeAreaDescanso>()
            .GetAllWithSpec(spec);

            var specCount = new OpeAreasDescansoForCountingSpecification(request);
            var totalOpeAreasDescanso = await _unitOfWork.Repository<OpeAreaDescanso>().CountAsync(specCount);
            var opeAreaDescansoVmList = _mapper.Map<List<OpeAreaDescansoVm>>(opeAreasDescanso);



            var rounded = Math.Ceiling(Convert.ToDecimal(totalOpeAreasDescanso) / Convert.ToDecimal(request.PageSize));
            var totalPages = Convert.ToInt32(rounded);

            var pagination = new PaginationVm<OpeAreaDescansoVm>
            {
                Count = totalOpeAreasDescanso,
                Data = opeAreaDescansoVmList,
                PageCount = totalPages,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            _logger.LogInformation($"{nameof(GetOpeAreasDescansoListQueryHandler)} - END");
            return pagination;
        }


    }
}