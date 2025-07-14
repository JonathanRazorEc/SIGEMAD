using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.MovilizacionesMedios;
using DGPCE.Sigemad.Application.Specifications.MovilizacionMedios;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.MovilizacionMedios.Queries.GetTipoGestion;
public class GetTipoGestionQueryHandler : IRequestHandler<GetTipoGestionQuery, IReadOnlyList<TipoGestionDto>>
{
    private readonly ILogger<GetTipoGestionQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTipoGestionQueryHandler(
        ILogger<GetTipoGestionQueryHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<TipoGestionDto>> Handle(GetTipoGestionQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(GetTipoGestionQueryHandler)} - BEGIN");

        var spec = new FlujoPasoMovilizacionSpecification(request);
        IReadOnlyList<FlujoPasoMovilizacion> tipoGestiones = await _unitOfWork.Repository<FlujoPasoMovilizacion>().GetAllWithSpec(spec);

        _logger.LogInformation($"{nameof(GetTipoGestionQueryHandler)} - END");

        return tipoGestiones.Select(f => new TipoGestionDto
        {
            Id = f.PasoSiguiente.Id,
            Descripcion = f.PasoSiguiente.Descripcion
        }).ToList();
    }
}
