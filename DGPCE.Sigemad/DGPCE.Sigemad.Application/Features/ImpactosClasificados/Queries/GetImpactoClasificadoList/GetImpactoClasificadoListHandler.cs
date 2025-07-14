using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Caching;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.ImpactosClasificados.Vms;
using DGPCE.Sigemad.Application.Specifications.Impactos;
using DGPCE.Sigemad.Domain;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DGPCE.Sigemad.Application.Features.ImpactosClasificados.Queries.GetImpactoClasificadoList;
public class GetImpactoClasificadoListHandler : IRequestHandler<GetImpactoClasificadoListQuery, IReadOnlyList<ImpactoClasificadoConTipoImpactoVM>>
{
    private readonly ILogger<GetImpactoClasificadoListHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISIGEMemoryCache _cache;
    private readonly IMapper _mapper;

    public GetImpactoClasificadoListHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetImpactoClasificadoListHandler> logger,
        ISIGEMemoryCache cache,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<IReadOnlyList<ImpactoClasificadoConTipoImpactoVM>> Handle(GetImpactoClasificadoListQuery request, CancellationToken cancellationToken)
    {
        if (request.IdTipoImpacto.HasValue && request.IdTipoImpacto.Value > 0)
        {      
            var tipoImpacto = await _unitOfWork.Repository<TipoImpacto>().GetByIdAsync(request.IdTipoImpacto.Value);
            if (tipoImpacto is null)
            {
                _logger.LogWarning($"request.IdTipoImpacto: {request.IdTipoImpacto}, no encontrado");
                throw new NotFoundException(nameof(TipoImpacto), request.IdTipoImpacto);
            }
        }

        var impactosClasificados = await _unitOfWork.Repository<ImpactoClasificado>()
        .GetAllWithSpec(new ImpactosClasificadosSpecification(new ImpactoClasificadoParams {IdTipoImpacto= (request.IdTipoImpacto.HasValue ? request.IdTipoImpacto.Value: null) ,
            nuclear = request.nuclear.HasValue ? request.nuclear.Value: false,
            busqueda = request.busqueda }));

        var impactosClasificadosVM = _mapper.Map<IReadOnlyList<ImpactoClasificado>, IReadOnlyList<ImpactoClasificadoConTipoImpactoVM>>(impactosClasificados);

        return impactosClasificadosVM;
    }
}
