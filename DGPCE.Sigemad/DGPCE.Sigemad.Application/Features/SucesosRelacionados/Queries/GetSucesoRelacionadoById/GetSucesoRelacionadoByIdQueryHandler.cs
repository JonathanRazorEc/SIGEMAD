using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.Sucesos;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.SucesosRelacionados.Vms;
using DGPCE.Sigemad.Application.Specifications.SucesosRelacionados;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.SucesosRelacionados.Queries.GetSucesoRelacionadoById;
public class GetSucesoRelacionadoByIdQueryHandler : IRequestHandler<GetSucesoRelacionadoByIdQuery, SucesoRelacionadoVm>
{
    private readonly ILogger<GetSucesoRelacionadoByIdQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetSucesoRelacionadoByIdQueryHandler(
        ILogger<GetSucesoRelacionadoByIdQueryHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    public async Task<SucesoRelacionadoVm> Handle(GetSucesoRelacionadoByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(GetSucesoRelacionadoByIdQueryHandler)} - BEGIN");

        var spec = new SucesoRelacionadoActiveByIdSpecification(request.Id);
        var sucesosRelacionado = await _unitOfWork.Repository<SucesoRelacionado>().GetByIdWithSpec(spec);

        if (sucesosRelacionado is null)
        {
            _logger.LogWarning($"request.Id: {request.Id}, no encontrado");
            throw new NotFoundException(nameof(SucesoRelacionado), request.Id);
        }

        var sucesoRelacionadoDto = new SucesoRelacionadoVm
        {
            Id = sucesosRelacionado.Id,
            IdSuceso = sucesosRelacionado.IdSucesoPrincipal,
            FechaCreacion = sucesosRelacionado.FechaCreacion,
            FechaModificacion = sucesosRelacionado.FechaModificacion,
            SucesosAsociados = sucesosRelacionado.DetalleSucesoRelacionados.Where(d => d.Borrado == false).Select(sr =>
            {
                var sucesoAsociado = sr.SucesoAsociado;

                // Obtener la denominación desde la relación cargada
                string? estado = sucesoAsociado.IdTipo switch
                {
                    (int)TipoSucesoEnum.IncendioForestal => sucesoAsociado.Incendios?.FirstOrDefault()?.EstadoSuceso?.Descripcion,
                    _ => ""
                };

                string? denominacion = sucesoAsociado.IdTipo switch
                {
                    (int)TipoSucesoEnum.IncendioForestal => sucesoAsociado.Incendios?.FirstOrDefault()?.Denominacion,
                    _ => ""
                };

                return new SucesoGridDto
                {
                    Id = sr.IdSucesoAsociado,
                    FechaCreacion = sucesoAsociado.FechaCreacion,
                    FechaModificacion = sucesoAsociado.FechaModificacion,
                    TipoSuceso = sucesoAsociado.TipoSuceso.Descripcion,
                    Estado = estado ?? "",
                    Denominacion = denominacion ?? "",
                };

            }).ToList()
        };

        _logger.LogInformation($"{nameof(GetSucesoRelacionadoByIdQueryHandler)} - END");
        return sucesoRelacionadoDto;
    }
}
