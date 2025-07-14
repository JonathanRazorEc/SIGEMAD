using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpePorcentajesOcupacionAreasEstacionamiento.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications.Ope.Administracion.OpePorcentajesOcupacionAreasEstacionamiento;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePorcentsOcAE.Queries.GetOpePorcentajesOcupacionAreasEstacionamientoList
{
    public class GetOpePorcentsOcAEListQueryHandler : IRequestHandler<GetOpePorcentsOcAEListQuery, PaginationVm<OpePorcentajeOcupacionAreaEstacionamientoVm>>
    {
        private readonly ILogger<GetOpePorcentsOcAEListQueryHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetOpePorcentsOcAEListQueryHandler(
        ILogger<GetOpePorcentsOcAEListQueryHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationVm<OpePorcentajeOcupacionAreaEstacionamientoVm>> Handle(GetOpePorcentsOcAEListQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(GetOpePorcentsOcAEListQueryHandler)} - BEGIN");

            var spec = new OpePorcentajesOcupacionAreasEstacionamientoSpecification(request);
            var opePorcentajesOcupacionAreasEstacionamiento = await _unitOfWork.Repository<OpePorcentajeOcupacionAreaEstacionamiento>()
            .GetAllWithSpec(spec);

            var specCount = new OpePorcentajesOcupacionAreasEstacionamientoForCountingSpecification(request);
            var totalOpePorcentajesOcupacionAreasEstacionamiento = await _unitOfWork.Repository<OpePorcentajeOcupacionAreaEstacionamiento>().CountAsync(specCount);
            var opePorcentajeOcupacionAreaEstacionamientoVmList = _mapper.Map<List<OpePorcentajeOcupacionAreaEstacionamientoVm>>(opePorcentajesOcupacionAreasEstacionamiento);



            var rounded = Math.Ceiling(Convert.ToDecimal(totalOpePorcentajesOcupacionAreasEstacionamiento) / Convert.ToDecimal(request.PageSize));
            var totalPages = Convert.ToInt32(rounded);

            var pagination = new PaginationVm<OpePorcentajeOcupacionAreaEstacionamientoVm>
            {
                Count = totalOpePorcentajesOcupacionAreasEstacionamiento,
                Data = opePorcentajeOcupacionAreaEstacionamientoVmList,
                PageCount = totalPages,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            _logger.LogInformation($"{nameof(GetOpePorcentsOcAEListQueryHandler)} - END");
            return pagination;
        }


    }
}