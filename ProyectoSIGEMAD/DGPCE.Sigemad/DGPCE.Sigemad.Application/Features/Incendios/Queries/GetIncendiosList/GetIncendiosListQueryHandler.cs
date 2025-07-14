using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Features.Incendios.Queries.GetIncendiosList;
using DGPCE.Sigemad.Application.Features.Incendios.Vms;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Helpers;
using DGPCE.Sigemad.Application.Specifications.Incendios;
using DGPCE.Sigemad.Application.Specifications.SuperficieFiltros;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Incendios.Queries;

public class GetIncendiosListQueryHandler : IRequestHandler<GetIncendiosListQuery, PaginationVm<IncendioVm>>
{
    private readonly ILogger<GetIncendiosListQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetIncendiosListQueryHandler(
        ILogger<GetIncendiosListQueryHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginationVm<IncendioVm>> Handle(GetIncendiosListQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(GetIncendiosListQueryHandler)} - BEGIN");


        if (request.IdSuperficieAfectada.HasValue)
        {
            var specSuperficie = new SuperficieFiltrosSpecifications(new SuperficieFiltroParams{ Id = request.IdSuperficieAfectada });
            var filtroArea = await _unitOfWork.Repository<SuperficieFiltro>().GetByIdWithSpec(specSuperficie);
            request.IdTipoFiltro = filtroArea.IdTipoFiltro;
            request.valorFiltro = filtroArea.Valor;
        }


        var spec = new IncendiosSpecification(request);
        var incendios = await _unitOfWork.Repository<Incendio>()
        .GetAllWithSpec(spec);

        var specCount = new IncendiosForCountingSpecification(request);
        var totalIncendios = await _unitOfWork.Repository<Incendio>().CountAsync(specCount);

        List<IncendioVm> incendioVmList = (await Task.WhenAll(incendios.Select(i => MapIncendioVmAsync(i, cancellationToken)))).ToList();

        var rounded = Math.Ceiling(Convert.ToDecimal(totalIncendios) / Convert.ToDecimal(request.PageSize));
        var totalPages = Convert.ToInt32(rounded);

        var pagination = new PaginationVm<IncendioVm>
        {
            Count = totalIncendios,
            Data = incendioVmList,
            PageCount = totalPages,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };

        _logger.LogInformation($"{nameof(GetIncendiosListQueryHandler)} - END");
        return pagination;
    }

    private async Task<IncendioVm> MapIncendioVmAsync(Incendio item, CancellationToken cancellationToken)
    {
        var incendioVm = _mapper.Map<IncendioVm>(item);
        incendioVm.Ubicacion =IncendioUtils.ObtenerUbicacion(item);

        RegistroActualizacion? ultimoRegistro = TryGetUltimoRegistro(item.Suceso.RegistroActualizaciones);

        (string? ultimaSituacionOperativa, string? maximaSituacionOperativa, string? estadoIncendio) = await ObtenerParametrosAsync(item);

        incendioVm.FechaUltimoRegistro = ultimoRegistro?.FechaCreacion;
        incendioVm.Sop = ultimaSituacionOperativa;
        incendioVm.MaxSop = maximaSituacionOperativa;
        incendioVm.EstadoIncendio = estadoIncendio;

        return incendioVm;
    }


    private RegistroActualizacion? TryGetUltimoRegistro(List<RegistroActualizacion> registros)
    {
        return registros?
            .OrderByDescending(r => r.FechaCreacion)
            .FirstOrDefault();
    }

    private Task<(string? UltimaSituacionOperativa, string? MaximaSituacionOperativa, string? estadoIncendio)> ObtenerParametrosAsync(Incendio item)
    {
        List<Registro>? registros = item.Suceso?.Registros;
        if (registros == null) return Task.FromResult<(string?, string?, string?)>((null, null, null));

        var parametros = registros.SelectMany(r => r.Parametros)
            .Where(p => !p.Borrado)
            .ToList();

        var ultimaSituacionOperativa = parametros
            .OrderByDescending(p => p.FechaCreacion)
            .FirstOrDefault()?.SituacionEquivalente?.Descripcion;

        var ultimoEstadoIncendio = parametros
            .OrderByDescending(p => p.FechaCreacion)
            .FirstOrDefault()?.EstadoIncendio?.Descripcion;

        var maximaSituacionOperativa = parametros
            .OrderBy(p => p.SituacionEquivalente?.Prioridad)
            .Select(p => p.SituacionEquivalente?.Descripcion)
            .FirstOrDefault();

        return Task.FromResult((ultimaSituacionOperativa, maximaSituacionOperativa,ultimoEstadoIncendio ));
    }
}
