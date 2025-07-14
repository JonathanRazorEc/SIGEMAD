using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosAsistencias.Commands.DeleteOpeDatosAsistencias;

public class DeleteOpeDatoAsistenciaCommandHandler : IRequestHandler<DeleteOpeDatoAsistenciaCommand>
{
    private readonly ILogger<DeleteOpeDatoAsistenciaCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteOpeDatoAsistenciaCommandHandler(
        ILogger<DeleteOpeDatoAsistenciaCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /*
    public async Task<Unit> Handle(DeleteOpeDatoAsistenciaCommand request, CancellationToken cancellationToken)
    {
        var opeDatoAsistenciaToDelete = await _unitOfWork.Repository<OpeDatoAsistencia>().GetByIdAsync(request.Id);
        if (opeDatoAsistenciaToDelete is null || opeDatoAsistenciaToDelete.Borrado)
        {
            _logger.LogWarning($"El ope dato de asistencia con id:{request.Id}, no existe en la base de datos");
            throw new NotFoundException(nameof(OpeDatoAsistencia), request.Id);
        }

        _unitOfWork.Repository<OpeDatoAsistencia>().DeleteEntity(opeDatoAsistenciaToDelete);

        await _unitOfWork.Complete();
        _logger.LogInformation($"El ope dato de asistencia con id: {request.Id}, se ha borrado con éxito");

        return Unit.Value;
    }
    */

    public async Task<Unit> Handle(DeleteOpeDatoAsistenciaCommand request, CancellationToken cancellationToken)
    {
        var opeDatoAsistenciaToDelete = await _unitOfWork.Repository<OpeDatoAsistencia>().GetByIdAsync(request.Id);
        if (opeDatoAsistenciaToDelete is null || opeDatoAsistenciaToDelete.Borrado)
        {
            _logger.LogWarning($"El ope dato de asistencia con id:{request.Id}, no existe en la base de datos");
            throw new NotFoundException(nameof(OpeDatoAsistencia), request.Id);
        }

        // Eliminar las entidades relacionadas
        if (opeDatoAsistenciaToDelete.OpeDatosAsistenciasSociales != null)
        {
            foreach (var social in opeDatoAsistenciaToDelete.OpeDatosAsistenciasSociales)
            {
                // Eliminar tareas asociadas
                if (social.OpeDatosAsistenciasSocialesTareas != null)
                {
                    foreach (var tarea in social.OpeDatosAsistenciasSocialesTareas)
                    {
                        _unitOfWork.Repository<OpeDatoAsistenciaSocialTarea>().DeleteEntity(tarea);
                    }
                }

                // Eliminar organismos asociados
                if (social.OpeDatosAsistenciasSocialesOrganismos != null)
                {
                    foreach (var organismo in social.OpeDatosAsistenciasSocialesOrganismos)
                    {
                        _unitOfWork.Repository<OpeDatoAsistenciaSocialOrganismo>().DeleteEntity(organismo);
                    }
                }

                // Eliminar usuarios asociados
                if (social.OpeDatosAsistenciasSocialesUsuarios != null)
                {
                    foreach (var usuario in social.OpeDatosAsistenciasSocialesUsuarios)
                    {
                        _unitOfWork.Repository<OpeDatoAsistenciaSocialUsuario>().DeleteEntity(usuario);
                    }
                }
            }
        }

        // Eliminar traducciones asociadas
        if (opeDatoAsistenciaToDelete.OpeDatosAsistenciasTraducciones != null)
        {
            foreach (var traduccion in opeDatoAsistenciaToDelete.OpeDatosAsistenciasTraducciones)
            {
                _unitOfWork.Repository<OpeDatoAsistenciaTraduccion>().DeleteEntity(traduccion);
            }
        }

        // Eliminar el OpeDatoAsistencia principal
        _unitOfWork.Repository<OpeDatoAsistencia>().DeleteEntity(opeDatoAsistenciaToDelete);

        await _unitOfWork.Complete();
        _logger.LogInformation($"El ope dato de asistencia con id: {request.Id}, se ha borrado con éxito");

        return Unit.Value;
    }


}
