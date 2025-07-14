using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.Sucesos;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.OtrasInformaciones.Commands.ManageOtraInformaciones;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications.Incendios;
using DGPCE.Sigemad.Application.Specifications.Sucesos;
using DGPCE.Sigemad.Application.Specifications.SucesosRelacionados;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Sucesos.Queries.GetSucesosList;

public class GetSucesosListQueryHandler : IRequestHandler<GetSucesosListQuery, PaginationVm<SucesoGridDto>>
{
    private readonly ILogger<GetSucesosListQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetSucesosListQueryHandler(
        ILogger<GetSucesosListQueryHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginationVm<SucesoGridDto>> Handle(GetSucesosListQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(GetSucesosListQueryHandler)} - BEGIN");

        await ValidateFechas(request);

        List<int> idsRelacionados = null;
        if (request.IdSuceso.HasValue)
        {
            // Precomputar los IDs relacionados
            var specDetalle = new DetalleSucesoRelacionadoByIdSucesoPrincipalSpecification(request.IdSuceso.Value);
            var sucesosRelacionados = await _unitOfWork.Repository<DetalleSucesoRelacionado>().GetAllWithSpec(specDetalle);
            idsRelacionados = sucesosRelacionados.Select(drs => drs.IdSucesoAsociado).ToList();
        }


        var spec = new SucesosSpecification(request, idsRelacionados);
        var sucesos = await _unitOfWork.Repository<Suceso>().GetAllWithSpec(spec);

        var specCount = new SucesoForCountingSpecification(request, idsRelacionados);
        var totalSucesos = await _unitOfWork.Repository<Suceso>().CountAsync(specCount);

        var rounded = Math.Ceiling(Convert.ToDecimal(totalSucesos) / Convert.ToDecimal(request.PageSize));
        var totalPages = Convert.ToInt32(rounded);

        //-------------------------------------
        var sucesosGrid = sucesos.Select(suceso =>
        {
            // Obtener la denominación desde la relación cargada
            string? estado = suceso.IdTipo switch
            {
                (int)TipoSucesoEnum.IncendioForestal => suceso.Incendios?.FirstOrDefault()?.EstadoSuceso?.Descripcion,
                _ => ""
            };

            string? denominacion = suceso.IdTipo switch
            {
                (int)TipoSucesoEnum.IncendioForestal => suceso.Incendios?.FirstOrDefault()?.Denominacion,
                _ => ""
            };

            return new SucesoGridDto
            {
                Id = suceso.Id,
                FechaCreacion = suceso.FechaCreacion,
                FechaModificacion = suceso.FechaModificacion,
                TipoSuceso = suceso.TipoSuceso.Descripcion,
                Estado = estado ?? "",
                Denominacion = denominacion ?? "",
            };

        }).ToList();

        //-------------------------------------

        var pagination = new PaginationVm<SucesoGridDto>
        {
            Count = totalSucesos,
            Data = sucesosGrid,
            PageCount = totalPages,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };


        _logger.LogInformation($"{nameof(GetSucesosListQueryHandler)} - END");
        return pagination;
    }

    private async Task ValidateFechas(GetSucesosListQuery request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if(request.FechaFin < request.FechaInicio)
        {
            throw new BadRequestException($"La fecha fin es menor a la fecha inciio.");
        }

        if (request.FechaInicio > DateOnly.FromDateTime(DateTime.Now))
        {
            throw new BadRequestException($"La fecha inicio es mayor a la fecha del sistema.");
        }

    }
}