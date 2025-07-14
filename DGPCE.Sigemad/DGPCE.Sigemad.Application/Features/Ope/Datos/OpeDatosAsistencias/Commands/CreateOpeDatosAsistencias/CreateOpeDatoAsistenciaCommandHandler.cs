using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Ope.Datos.OpeDatosAsistencias;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosAsistencias.Commands.CreateOpeDatosAsistencias;

public class CreateOpeDatoAsistenciaCommandHandler : IRequestHandler<CreateOpeDatoAsistenciaCommand, CreateOpeDatoAsistenciaResponse>
{
    private readonly ILogger<CreateOpeDatoAsistenciaCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IOpeDatoAsistenciaService _opeDatoAsistenciaService;

    public CreateOpeDatoAsistenciaCommandHandler(
        ILogger<CreateOpeDatoAsistenciaCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper,
         IOpeDatoAsistenciaService opeDatoAsistenciaService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _opeDatoAsistenciaService = opeDatoAsistenciaService;
    }

 
    public async Task<CreateOpeDatoAsistenciaResponse> Handle(CreateOpeDatoAsistenciaCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(CreateOpeDatoAsistenciaCommandHandler) + " - BEGIN");

        // VALIDACIONES
        await _opeDatoAsistenciaService.ValidarRegistrosDuplicados(null, request.IdOpePuerto, request.Fecha);
        // FIN VALIDACIONES

        //var opeDatoAsistenciaEntity = _mapper.Map<OpeDatoAsistencia>(request);
        var opeDatoAsistenciaToCreate = new OpeDatoAsistencia
        {
            IdOpePuerto = request.IdOpePuerto,
            Fecha = request.Fecha,
            OpeDatosAsistenciasSanitarias = new List<OpeDatoAsistenciaSanitaria>(),
            OpeDatosAsistenciasSociales = new List<OpeDatoAsistenciaSocial>(),
            OpeDatosAsistenciasTraducciones = new List<OpeDatoAsistenciaTraduccion>()
        };

        // DATOS ASISTENCIAS SANITARIAS
        if (request.opeDatosAsistenciasSanitariasModificado)
        {
            OpeDatoAsistenciaSanitaria opeDatoAsistenciaSanitariaToSave;

            foreach (var opeDatoAsistenciaSanitaria in request.OpeDatosAsistenciasSanitarias)
            {
                opeDatoAsistenciaSanitariaToSave = _mapper.Map<OpeDatoAsistenciaSanitaria>(opeDatoAsistenciaSanitaria);
                opeDatoAsistenciaToCreate.OpeDatosAsistenciasSanitarias.Add(opeDatoAsistenciaSanitariaToSave);
            }
        }

        // DATOS ASISTENCIAS SOCIALES
        if (request.opeDatosAsistenciasSocialesModificado)
        {
            OpeDatoAsistenciaSocial opeDatoAsistenciaSocialToSave;

            foreach (var opeDatoAsistenciaSocial in request.OpeDatosAsistenciasSociales)
            {
                opeDatoAsistenciaSocialToSave = _mapper.Map<OpeDatoAsistenciaSocial>(opeDatoAsistenciaSocial);
                opeDatoAsistenciaToCreate.OpeDatosAsistenciasSociales.Add(opeDatoAsistenciaSocialToSave);
            }
        }

        // DATOS ASISTENCIAS TRADUCCIONES
        if (request.opeDatosAsistenciasTraduccionesModificado)
        {
            OpeDatoAsistenciaTraduccion opeDatoAsistenciaTraduccionToSave;

            foreach (var opeDatoAsistenciaTraduccion in request.OpeDatosAsistenciasTraducciones)
            {
                opeDatoAsistenciaTraduccionToSave = _mapper.Map<OpeDatoAsistenciaTraduccion>(opeDatoAsistenciaTraduccion);
                opeDatoAsistenciaToCreate.OpeDatosAsistenciasTraducciones.Add(opeDatoAsistenciaTraduccionToSave);
            }
        }


        //_mapper.Map(request, opeDatoAsistenciaToUpdate, typeof(UpdateOpeDatoAsistenciaCommand), typeof(OpeDatoAsistencia));

        _unitOfWork.Repository<OpeDatoAsistencia>().AddEntity(opeDatoAsistenciaToCreate);
        await _unitOfWork.Complete();

        _logger.LogInformation($"El ope dato de asistencia {opeDatoAsistenciaToCreate.Id} fue creado correctamente");
        _logger.LogInformation(nameof(CreateOpeDatoAsistenciaCommandHandler) + " - END");

        return new CreateOpeDatoAsistenciaResponse { Id = opeDatoAsistenciaToCreate.Id };
    }

    //
}
