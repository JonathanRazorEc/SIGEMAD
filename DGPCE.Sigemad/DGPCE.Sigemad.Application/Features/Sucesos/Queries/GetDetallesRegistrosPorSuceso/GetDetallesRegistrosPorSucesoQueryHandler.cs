using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.Registros;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.Shared;
using DGPCE.Sigemad.Application.Specifications.RegistrosActualizaciones;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Sucesos.Queries.GetDetallesRegistrosPorSuceso;
public class GetDetallesRegistrosPorSucesoQueryHandler : IRequestHandler<GetDetallesRegistrosPorSucesoQuery, PaginationVm<DetalleRegistroActualizacionDto>>
{
    private readonly ILogger<GetDetallesRegistrosPorSucesoQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetDetallesRegistrosPorSucesoQueryHandler(
        ILogger<GetDetallesRegistrosPorSucesoQueryHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<PaginationVm<DetalleRegistroActualizacionDto>> Handle(GetDetallesRegistrosPorSucesoQuery request, CancellationToken cancellationToken)
    {
        Suceso suceso = await _unitOfWork.Repository<Suceso>().GetByIdAsync(request.IdSuceso);

        if (suceso == null || suceso.Borrado == true)
        {
            _logger.LogWarning($"No se encontro suceso con id: {request.IdSuceso}");
            throw new NotFoundException(nameof(Suceso), request.IdSuceso);
        }

        var specCounting = new DetalleRegistroActualizacionForCountingSpecification(new DetalleRegistroActualizacionParams
        {
            IdSuceso = request.IdSuceso,
        });

        var totalRegistros = await _unitOfWork.Repository<DetalleRegistroActualizacion>().CountAsync(specCounting);
        if (totalRegistros == 0)
        {
            _logger.LogWarning($"No se encontraron registros para el suceso con id: {request.IdSuceso}");
            return new PaginationVm<DetalleRegistroActualizacionDto>
            {
                Count = totalRegistros,
                Data = new List<DetalleRegistroActualizacionDto>(),
                PageCount = 0,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };
        }

        var spec = new DetalleRegistroActualizacionSpecification(new DetalleRegistroActualizacionParams
        {
            IdSuceso = request.IdSuceso,
            PageSize = request.PageSize,
            PageIndex = request.PageIndex
        });

        var registros = await _unitOfWork.Repository<DetalleRegistroActualizacion>().GetAllWithSpec(spec);


        List<DetalleRegistroActualizacionDto> detallesDtos = registros.Select(r => new DetalleRegistroActualizacionDto
        {
            Id = r.Id,
            IdRegistro = r.IdRegistroActualizacion,
            FechaHora = r.FechaCreacion,
            Ambito = r.Ambito,
            Accion = GetDescripcionAccion(r.IdEstadoRegistro),
            Datos = r.Descripcion,
        }).ToList();

        var rounded = Math.Ceiling(Convert.ToDecimal(totalRegistros) / Convert.ToDecimal(request.PageSize));
        var totalPages = Convert.ToInt32(rounded);

        var pagination = new PaginationVm<DetalleRegistroActualizacionDto>
        {
            Count = totalRegistros,
            Data = detallesDtos,
            PageCount = totalPages,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };

        return pagination;
    }

    private string GetDescripcionAccion(EstadoRegistroEnum estadoRegistro)
    {
        switch (estadoRegistro)
        {
            case EstadoRegistroEnum.Creado:
                return "Creado";
            case EstadoRegistroEnum.Modificado:
            case EstadoRegistroEnum.CreadoYModificado:
                return "Modificado";
            case EstadoRegistroEnum.CreadoYEliminado:
            case EstadoRegistroEnum.Eliminado:
                return "Eliminado";
        }

        return "Desconocido";
    }

}