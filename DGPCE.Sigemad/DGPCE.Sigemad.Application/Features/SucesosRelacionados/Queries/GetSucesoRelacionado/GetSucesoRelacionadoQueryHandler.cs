using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.Sucesos;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.SucesosRelacionados.Queries.GetSucesoRelacionadoById;
using DGPCE.Sigemad.Application.Features.SucesosRelacionados.Vms;
using DGPCE.Sigemad.Application.Specifications.RegistrosActualizaciones;
using DGPCE.Sigemad.Application.Specifications.SucesosRelacionados;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DGPCE.Sigemad.Application.Features.SucesosRelacionados.Queries.GetSucesoRelacionado;
public class GetSucesoRelacionadoQueryHandler : IRequestHandler<GetSucesoRelacionadoQuery, SucesoRelacionadoVm>
{
    private readonly ILogger<GetSucesoRelacionadoQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;


    public GetSucesoRelacionadoQueryHandler(
    ILogger<GetSucesoRelacionadoQueryHandler> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<SucesoRelacionadoVm> Handle(GetSucesoRelacionadoQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(GetSucesoRelacionadoQueryHandler)} - BEGIN");

        SucesoRelacionado sucesoRelacionado;
        List<int> idsReferencias = new List<int>();
        List<int> idsDetallesSucesoRelacionado = new List<int>();
        List<int> idsDetallesSucesoRelacionadoEliminables = new List<int>();

        if (request.IdRegistroActualizacion.HasValue)
        {
            _logger.LogInformation($"Buscando Documentacion para IdRegistroActualizacion: {request.IdRegistroActualizacion}");

            // Obtener RegistroActualizacion con IdReferencia (documentacion.Id)
            var registroSpec = new RegistroActualizacionSpecificationParams
            {
                Id = request.IdRegistroActualizacion.Value
            };
            var registroActualizacion = await _unitOfWork.Repository<RegistroActualizacion>()
                .GetByIdWithSpec(new RegistroActualizacionSpecification(registroSpec));

            if (registroActualizacion == null)
            {
                _logger.LogWarning($"No se encontró RegistroActualizacion con Id: {request.IdRegistroActualizacion}");
                throw new NotFoundException(nameof(RegistroActualizacion), request.IdRegistroActualizacion);
            }

            // Separar IdReferencia según su tipo
            foreach (var detalle in registroActualizacion.DetallesRegistro)
            {
                switch (detalle.IdApartadoRegistro)
                {
                    case (int)ApartadoRegistroEnum.SucesosRelacionados:
                        idsDetallesSucesoRelacionado.Add(detalle.IdReferencia);
                        if (IsEliminable(detalle.IdEstadoRegistro)) idsDetallesSucesoRelacionado.Add(detalle.IdReferencia);
                        break;
                }
            }

            // Buscar la Documentacion por IdReferencia
            sucesoRelacionado = await _unitOfWork.Repository<SucesoRelacionado>()
                .GetByIdWithSpec(new SucesoRelacionadoWithFilteredData(registroActualizacion.IdReferencia, idsDetallesSucesoRelacionado));
        }
        else
        {
            _logger.LogInformation($"Buscando Dirección y Coordinación de Emergencia para IdSuceso: {request.IdSuceso}");

            // Buscar Documentacion por IdSuceso
            sucesoRelacionado = await _unitOfWork.Repository<SucesoRelacionado>()
                .GetByIdWithSpec(new SucesosRelacionadosActiveByIdSucesoPrincipalSpecification(request.IdSuceso));
        }


        if (sucesoRelacionado == null)
        {
            _logger.LogWarning($"No se encontró sucesoRelacionado para Suceso: {request.IdSuceso}");
            return new SucesoRelacionadoVm
            {
                IdSuceso = request.IdSuceso,
            };
        }

        var sucesoRelacionadoDto = new SucesoRelacionadoVm
        {
            Id = sucesoRelacionado.Id,
            IdSuceso = sucesoRelacionado.IdSucesoPrincipal,
            FechaCreacion = sucesoRelacionado.FechaCreacion,
            FechaModificacion = sucesoRelacionado.FechaModificacion,
            SucesosAsociados = sucesoRelacionado.DetalleSucesoRelacionados.Where(d => d.Borrado == false).Select(sr =>
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
    private bool IsEliminable(EstadoRegistroEnum estado)
    {
        return estado == EstadoRegistroEnum.Creado ||
            estado == EstadoRegistroEnum.CreadoYModificado;
    }
}



