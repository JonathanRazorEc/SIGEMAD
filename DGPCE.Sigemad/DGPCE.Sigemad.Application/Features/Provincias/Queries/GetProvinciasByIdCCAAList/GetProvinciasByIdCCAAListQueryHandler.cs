using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.Provincias.Vms;
using DGPCE.Sigemad.Application.Specifications.Provincias;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Provincias.Queries.GetProvinciasByIdCCAAList
{
    public class GetProvinciasByIdCCAAListQueryHandler : IRequestHandler<GetProvinciasByIdCCAAListQuery, IReadOnlyList<ProvinciaSinMunicipiosVm>>
    {
        
    private readonly ILogger<GetProvinciasByIdCCAAListQueryHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProvinciasByIdCCAAListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetProvinciasByIdCCAAListQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;

        }
        public async Task<IReadOnlyList<ProvinciaSinMunicipiosVm>> Handle(GetProvinciasByIdCCAAListQuery request, CancellationToken cancellationToken)
        {

            var ccaa = await _unitOfWork.Repository<Ccaa>().GetByIdAsync(request.IdCcaa);
            if (ccaa is null)
            {
                _logger.LogWarning($"request.IdCcaa: {request.IdCcaa}, no encontrado");
                throw new NotFoundException(nameof(ccaa), request.IdCcaa);
            }

            var provinciaParams = new ProvinciaSpecificationParams
            {
                IdCcaa = request.IdCcaa
            };

            var spec = new ProvinciasSpecification(provinciaParams);
            var provinciasListado = await _unitOfWork.Repository<Provincia>()
            .GetAllWithSpec(spec);

            var provincias = _mapper.Map<IReadOnlyList<Provincia>, IReadOnlyList<ProvinciaSinMunicipiosVm>>(provinciasListado);

            return provincias;

        }
    }


}


