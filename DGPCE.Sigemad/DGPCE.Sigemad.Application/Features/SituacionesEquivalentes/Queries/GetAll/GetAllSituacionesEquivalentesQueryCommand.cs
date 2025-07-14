using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.SituacionesEquivalentes;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.SituacionesEquivalentes.Queries.GetAll;
public class GetAllSituacionesEquivalentesQueryCommand : IRequestHandler<GetAllSituacionesEquivalentesQuery, IReadOnlyList<SituacionEquivalenteDto>>
{
    private readonly ILogger<GetAllSituacionesEquivalentesQueryCommand> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllSituacionesEquivalentesQueryCommand(
        ILogger<GetAllSituacionesEquivalentesQueryCommand> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<SituacionEquivalenteDto>> Handle(GetAllSituacionesEquivalentesQuery request, CancellationToken cancellationToken)
    {
        var lista = await _unitOfWork.Repository<SituacionEquivalente>().GetAsync
            (
                predicate: s => s.Obsoleto == false,
                selector: s => new SituacionEquivalenteDto
                {
                    Id = s.Id,
                    Descripcion = s.Descripcion,
                },
                orderBy: null,
                disableTracking: true
            );

        return lista;
    }
}
