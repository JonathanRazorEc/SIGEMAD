using AutoMapper;
using Azure.Core;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Helpers;
using DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeDatosAsistencias;
using DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeDatosEmbarquesDiarios;
using DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeDatosFronteras;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpePeriodos.Commands.DeleteOpePeriodos;

public class DeleteOpePeriodoCommandHandler : IRequestHandler<DeleteOpePeriodoCommand>
{
    private readonly ILogger<DeleteOpePeriodoCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteOpePeriodoCommandHandler(
        ILogger<DeleteOpePeriodoCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(DeleteOpePeriodoCommand request, CancellationToken cancellationToken)
    {
        var opePeriodoToDelete = await _unitOfWork.Repository<OpePeriodo>().GetByIdAsync(request.Id);
        if (opePeriodoToDelete is null || opePeriodoToDelete.Borrado)
        {
            _logger.LogWarning($"El ope periodo con id:{request.Id}, no existe en la base de datos");
            throw new NotFoundException(nameof(OpePeriodo), request.Id);
        }

        // VALIDACIONES
        await ValidarOpePeriodoAntesDeBorrar(opePeriodoToDelete);
        // FIN VALIDACIONES

        _unitOfWork.Repository<OpePeriodo>().DeleteEntity(opePeriodoToDelete);

        await _unitOfWork.Complete();
        _logger.LogInformation($"El ope periodo con id: {request.Id}, se ha borrado con éxito");

        return Unit.Value;
    }

    private async Task ValidarOpePeriodoAntesDeBorrar(OpePeriodo opePeriodoToDelete)
    {
        await ValidarOpeDatosEmbarquesDiariosAsociadosAntesDeBorrar(opePeriodoToDelete);
        await ValidarOpeDatosAsistenciasAsociadosAntesDeBorrar(opePeriodoToDelete);
        await ValidarOpeDatosFronterasAsociadosAntesDeBorrar(opePeriodoToDelete);
    }

    public async Task ValidarOpeDatosEmbarquesDiariosAsociadosAntesDeBorrar(OpePeriodo opePeriodoToDelete)
    {
        var specParamsBusquedaAsociados = new OpeDatosEmbarquesDiariosSpecificationParams
        {
            FechaInicio = DateOnly.FromDateTime(opePeriodoToDelete.FechaInicioFaseSalida),
            FechaFin = DateOnly.FromDateTime(opePeriodoToDelete.FechaFinFaseRetorno),
        };

        var specBusquedaAsociados = new OpeDatosEmbarquesDiariosSpecification(specParamsBusquedaAsociados);
        var opeDatosEmbarquesDiariosAsociados = await _unitOfWork.Repository<OpeDatoEmbarqueDiario>().GetAllWithSpec(specBusquedaAsociados);

        if (opeDatosEmbarquesDiariosAsociados.Any())
        {
            //_logger.LogWarning($"Ya existe otro ope dato de frontera para la frontera con la misma fecha y con el mismo intervalo horario: {request.IdOpeDatoEmbarqueDiarioIntervaloHorario}");
            throw new BadRequestException("No se puede borrar el periodo porque existen datos de embarque asociados a esas fechas.");
        }
    }

    public async Task ValidarOpeDatosAsistenciasAsociadosAntesDeBorrar(OpePeriodo opePeriodoToDelete)
    {
        var specParamsBusquedaAsociados = new OpeDatosAsistenciasSpecificationParams
        {
            FechaInicio = DateOnly.FromDateTime(opePeriodoToDelete.FechaInicioFaseSalida),
            FechaFin = DateOnly.FromDateTime(opePeriodoToDelete.FechaFinFaseRetorno),
        };

        var specBusquedaAsociados = new OpeDatosAsistenciasSpecification(specParamsBusquedaAsociados);
        var opeDatosAsistenciasAsociados = await _unitOfWork.Repository<OpeDatoAsistencia>().GetAllWithSpec(specBusquedaAsociados);

        if (opeDatosAsistenciasAsociados.Any())
        {
            //_logger.LogWarning($"Ya existe otro ope dato de frontera para la frontera con la misma fecha y con el mismo intervalo horario: {request.IdOpeDatoEmbarqueDiarioIntervaloHorario}");
            throw new BadRequestException("No se puede borrar el periodo porque existen datos de asistencias asociados a esas fechas.");
        }
    }

    public async Task ValidarOpeDatosFronterasAsociadosAntesDeBorrar(OpePeriodo opePeriodoToDelete)
    {
        var specParamsBusquedaAsociados = new OpeDatosFronterasSpecificationParams
        {
            FechaInicio = DateOnly.FromDateTime(opePeriodoToDelete.FechaInicioFaseSalida),
            FechaFin = DateOnly.FromDateTime(opePeriodoToDelete.FechaFinFaseRetorno),
        };

        var specBusquedaAsociados = new OpeDatosFronterasSpecification(specParamsBusquedaAsociados);
        var opeDatosFronterasAsociados = await _unitOfWork.Repository<OpeDatoFrontera>().GetAllWithSpec(specBusquedaAsociados);

        if (opeDatosFronterasAsociados.Any())
        {
            //_logger.LogWarning($"Ya existe otro ope dato de frontera para la frontera con la misma fecha y con el mismo intervalo horario: {request.IdOpeDatoEmbarqueDiarioIntervaloHorario}");
            throw new BadRequestException("No se puede borrar el periodo porque existen datos de fronteras asociados a esas fechas.");
        }
    }

}
