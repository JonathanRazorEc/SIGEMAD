using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.IntervencionMedios;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.MediosCapacidad;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.MediosCapacidades.Queries.GetMediosCapacidadesByIdTipoCapacidad;
public class GetMediosCapacidadesByIdTipoCapacidadListQueryHandler : IRequestHandler<GetMediosCapacidadesByIdTipoCapacidadListQuery, IReadOnlyList<MediosCapacidadDto>>
{
    private readonly ILogger<GetMediosCapacidadesByIdTipoCapacidadListQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetMediosCapacidadesByIdTipoCapacidadListQueryHandler(
        ILogger<GetMediosCapacidadesByIdTipoCapacidadListQueryHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<MediosCapacidadDto>> Handle(GetMediosCapacidadesByIdTipoCapacidadListQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(GetMediosCapacidadesByIdTipoCapacidadListQueryHandler)} - BEGIN");

        var medioCapacidadesSpec = new MedioCapacidadByIdTipoCapacidadSpecification(request.idTipoCapacidad);
        var mediosCapacidades = await _unitOfWork.Repository<MediosCapacidad>().GetAllWithSpec(medioCapacidadesSpec);

        var mediosCapacidadesDto = _mapper.Map<IReadOnlyList<MediosCapacidadDto>>(mediosCapacidades);
        _logger.LogInformation($"{nameof(GetMediosCapacidadesByIdTipoCapacidadListQueryHandler)} - END");
        return mediosCapacidadesDto;
    }


}
