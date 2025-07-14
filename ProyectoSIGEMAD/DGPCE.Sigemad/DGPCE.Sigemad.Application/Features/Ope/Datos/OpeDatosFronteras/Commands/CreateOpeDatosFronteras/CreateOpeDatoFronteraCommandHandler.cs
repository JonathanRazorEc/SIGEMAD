using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Ope.Datos.OpeDatosFronteras;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Ope.Datos.OpeDatosFronterasIntervalosHorarios;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosFronteras.Commands.CreateOpeDatosFronteras;

public class CreateOpeDatoFronteraCommandHandler : IRequestHandler<CreateOpeDatoFronteraCommand, CreateOpeDatoFronteraResponse>
{
    private readonly ILogger<CreateOpeDatoFronteraCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IOpeDatoFronteraService _opeDatoFronteraService;

    public CreateOpeDatoFronteraCommandHandler(
        ILogger<CreateOpeDatoFronteraCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IOpeDatoFronteraService opeDatoFronteraService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _opeDatoFronteraService = opeDatoFronteraService;
    }

    public async Task<CreateOpeDatoFronteraResponse> Handle(CreateOpeDatoFronteraCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(CreateOpeDatoFronteraCommandHandler) + " - BEGIN");

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
        // Leemos el intervalo horario de la frontera
        var opeDatoFronteraIntervaloHorarioParams = new OpeDatosFronterasIntervalosHorariosSpecificationParams
        {
            Id = request.IdOpeDatoFronteraIntervaloHorario,
        };

        var specOpeDatoFronteraIntervaloHorario = new OpeDatosFronterasIntervalosHorariosSpecification(opeDatoFronteraIntervaloHorarioParams);
        var opeDatoFronteraIntervaloHorario = await _unitOfWork.Repository<OpeDatoFronteraIntervaloHorario>().GetByIdWithSpec(specOpeDatoFronteraIntervaloHorario);

        if (opeDatoFronteraIntervaloHorario == null)
        {
            _logger.LogWarning($"No se encontro dato de frontera intervalo horario de OPE con id: {request.IdOpeDatoFronteraIntervaloHorario}");
            throw new NotFoundException(nameof(OpeDatoFronteraIntervaloHorario), request.IdOpeDatoFronteraIntervaloHorario);
        }

        TimeSpan horaInicio = request.IntervaloHorarioPersonalizado
             ? request.InicioIntervaloHorarioPersonalizado ?? opeDatoFronteraIntervaloHorario.Inicio
             : opeDatoFronteraIntervaloHorario.Inicio;

        TimeSpan horaFin = request.IntervaloHorarioPersonalizado
           ? request.FinIntervaloHorarioPersonalizado ?? opeDatoFronteraIntervaloHorario.Fin
           : opeDatoFronteraIntervaloHorario.Fin;


        await _opeDatoFronteraService.ValidarRegistrosDuplicados(null, request.IdOpeFrontera, request.Fecha, request.IdOpeDatoFronteraIntervaloHorario, horaInicio, horaFin);
        // FIN VALIDACIONES

        var opeDatoFronteraEntity = _mapper.Map<OpeDatoFrontera>(request);
        _unitOfWork.Repository<OpeDatoFrontera>().AddEntity(opeDatoFronteraEntity);

        var result = await _unitOfWork.Complete();
        if (result <= 0)
        {
            throw new Exception("No se pudo insertar nuevo ope dato de frontera");
        }

        _logger.LogInformation($"El ope dato de frontera {opeDatoFronteraEntity.Id} fue creado correctamente");

        _logger.LogInformation(nameof(CreateOpeDatoFronteraCommandHandler) + " - END");
        return new CreateOpeDatoFronteraResponse { Id = opeDatoFronteraEntity.Id };
    }
}
