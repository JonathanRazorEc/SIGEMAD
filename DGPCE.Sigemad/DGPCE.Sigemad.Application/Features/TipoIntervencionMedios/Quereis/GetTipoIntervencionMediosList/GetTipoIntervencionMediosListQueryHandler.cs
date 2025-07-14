using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.TipoIntervencionMedios.Vms;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using System.Linq.Expressions;


namespace DGPCE.Sigemad.Application.Features.TipoIntervencionMedios.Quereis.GetTipoIntervencionMediosList;
public class GetTipoIntervencionMediosListQueryHandler : IRequestHandler<GetTipoIntervencionMediosListQuery, IReadOnlyList<TipoIntervencionMedioVm>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTipoIntervencionMediosListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<TipoIntervencionMedioVm>> Handle(GetTipoIntervencionMediosListQuery request, CancellationToken cancellationToken)
    {
        var includes = new List<Expression<Func<TipoIntervencionMedio, object>>>();
        includes.Add(t =>t.ClasificacionMedio);
        includes.Add(t => t.TitularidadMedio);
        includes.Add(t => t.TipoEntidadTitularidadMedio);
        includes.Add(t => t.TitularidadAutonomica);
        includes.Add(t => t.TitularidadAutonomicaMunicipal);
        includes.Add(t => t.TitularidadProvinciaMunicipal);
        includes.Add(t => t.TitularidadMunicipal);
        includes.Add(t => t.TitularidadPais);

        var tipoIntervencionMedios = (await _unitOfWork.Repository<TipoIntervencionMedio>().GetAsync(null, null, includes))
            .ToList()
            .AsReadOnly();

        var tipoIntervencionMediosVm = _mapper.Map<IReadOnlyList<TipoIntervencionMedio>, IReadOnlyList<TipoIntervencionMedioVm>>(tipoIntervencionMedios);
        return tipoIntervencionMediosVm;
    }
}
