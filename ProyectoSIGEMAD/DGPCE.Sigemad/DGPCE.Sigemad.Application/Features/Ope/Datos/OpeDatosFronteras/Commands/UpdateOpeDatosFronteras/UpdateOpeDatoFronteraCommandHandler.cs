using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Ope.Datos.OpeDatosFronteras;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeDatosFronteras;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronteras.Commands.UpdateOpeDatosFronteras;

public class UpdateOpeDatoFronteraCommandHandler : IRequestHandler<UpdateOpeDatoFronteraCommand>
{
    private readonly ILogger<UpdateOpeDatoFronteraCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IOpeDatoFronteraService _opeDatoFronteraService;

    public UpdateOpeDatoFronteraCommandHandler(
        ILogger<UpdateOpeDatoFronteraCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IOpeDatoFronteraService opeDatoFronteraService
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _opeDatoFronteraService = opeDatoFronteraService;
    }

    public async Task<Unit> Handle(UpdateOpeDatoFronteraCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(UpdateOpeDatoFronteraCommandHandler) + " - BEGIN");

        var opeDatoFronteraSpec = new OpeDatoFronteraActiveByIdSpecification(request.Id);
        var opeDatoFronteraToUpdate = await _unitOfWork.Repository<OpeDatoFrontera>().GetByIdWithSpec(opeDatoFronteraSpec);


        if (opeDatoFronteraToUpdate == null)
        {
            _logger.LogWarning($"No se encontro ope dato de frontera con id: {request.Id}");
            throw new NotFoundException(nameof(OpeDatoFrontera), request.Id);
        }

        // VALIDACIONES
        // Validaciones para los intervalos personalizados
        if (request.IntervaloHorarioPersonalizado)
        {
            if (!request.InicioIntervaloHorarioPersonalizado.HasValue || !request.FinIntervaloHorarioPersonalizado.HasValue)
            {
                throw new BadRequestException("Debe indicar tanto la hora de inicio como la de fin del intervalo.");
            }

            await _opeDatoFronteraService.ValidarHoraInicioMenorHoraFinIntervaloHorarioPersonalizado(request.InicioIntervaloHorarioPersonalizado, request.FinIntervaloHorarioPersonalizado);
            await _opeDatoFronteraService.ValidarHoraDentroIntervaloHorario(request.InicioIntervaloHorarioPersonalizado, request.IdOpeDatoFronteraIntervaloHorario);
            await _opeDatoFronteraService.ValidarHoraDentroIntervaloHorario(request.FinIntervaloHorarioPersonalizado, request.IdOpeDatoFronteraIntervaloHorario);
        }
        // Validación de que no repite para dato de frontera: frontera, fecha, intervalo, fecha inicio y fecha fin
        TimeSpan horaInicio = request.IntervaloHorarioPersonalizado
             ? request.InicioIntervaloHorarioPersonalizado ?? opeDatoFronteraToUpdate.OpeDatoFronteraIntervaloHorario.Inicio
             : opeDatoFronteraToUpdate.OpeDatoFronteraIntervaloHorario.Inicio;

        TimeSpan horaFin = request.IntervaloHorarioPersonalizado
           ? request.FinIntervaloHorarioPersonalizado ?? opeDatoFronteraToUpdate.OpeDatoFronteraIntervaloHorario.Fin
           : opeDatoFronteraToUpdate.OpeDatoFronteraIntervaloHorario.Fin;


        await _opeDatoFronteraService.ValidarRegistrosDuplicados(request.Id, request.IdOpeFrontera, request.Fecha, request.IdOpeDatoFronteraIntervaloHorario, horaInicio, horaFin);
        // FIN VALIDACIONES

        _mapper.Map(request, opeDatoFronteraToUpdate, typeof(UpdateOpeDatoFronteraCommand), typeof(OpeDatoFrontera));

        _unitOfWork.Repository<OpeDatoFrontera>().UpdateEntity(opeDatoFronteraToUpdate);
        await _unitOfWork.Complete();

        _logger.LogInformation($"Se actualizo correctamente el ope dato de frontera con id: {request.Id}");
        _logger.LogInformation(nameof(UpdateOpeDatoFronteraCommandHandler) + " - END");

        return Unit.Value;
    }





 

}
