using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Municipios;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.MunicipiosExtranjeros.Queries.GetMunicipiosExtranjerosByIdPais;
public class GetMunicipiosExtranjerosByIdPaisQueryHandler : IRequestHandler<GetMunicipiosExtranjerosByIdPaisQuery, IReadOnlyList<MunicipioExtranjero>>
{
    private readonly ILogger<GetMunicipiosExtranjerosByIdPaisQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetMunicipiosExtranjerosByIdPaisQueryHandler(
        IUnitOfWork unitOfWork, IMapper mapper,
        ILogger<GetMunicipiosExtranjerosByIdPaisQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IReadOnlyList<MunicipioExtranjero>> Handle(GetMunicipiosExtranjerosByIdPaisQuery request, CancellationToken cancellationToken)
    {
        var pais = await _unitOfWork.Repository<Pais>().GetByIdAsync(request.IdPais);
        if (pais is null)
        {
            _logger.LogWarning($"request.IdPais: {request.IdPais}, no encontrado");
            throw new NotFoundException(nameof(Pais), request.IdPais);
        }

        var specification = new MunicipiosExtranjerosByPaisSpecification(request.IdPais);
        var lista = await _unitOfWork.Repository<MunicipioExtranjero>().GetAllWithSpec(specification);

        return lista;
    }
}
