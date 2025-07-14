using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Ope.Datos.OpeDatosAsistencias;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.Ope.Datos;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.OtrasInformaciones;
using DGPCE.Sigemad.Domain.Modelos.Ope.Datos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Ope.Datos.OpeDatosAsistencias.Commands.UpdateOpeDatosAsistencias;

public class UpdateOpeDatoAsistenciaCommandHandler : IRequestHandler<UpdateOpeDatoAsistenciaCommand>
{
    private readonly ILogger<UpdateOpeDatoAsistenciaCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IOpeDatoAsistenciaService _opeDatoAsistenciaService;

    public UpdateOpeDatoAsistenciaCommandHandler(
        ILogger<UpdateOpeDatoAsistenciaCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IOpeDatoAsistenciaService opeDatoAsistenciaService
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _opeDatoAsistenciaService = opeDatoAsistenciaService;
    }

    public async Task<Unit> Handle(UpdateOpeDatoAsistenciaCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(nameof(UpdateOpeDatoAsistenciaCommandHandler) + " - BEGIN");

        //var opeDatoAsistenciaSpec = new OpeDatoAsistenciaActiveByIdSpecification(request.Id);
        var opeDatoAsistenciaSpec = new OpeDatosAsistenciasWithDatosAsistenciasSpecification(request.Id);
        var opeDatoAsistenciaToUpdate = await _unitOfWork.Repository<OpeDatoAsistencia>().GetByIdWithSpec(opeDatoAsistenciaSpec);

        if (opeDatoAsistenciaToUpdate == null)
        {
            _logger.LogWarning($"No se encontro ope dato de asistencia con id: {request.Id}");
            throw new NotFoundException(nameof(OpeDatoAsistencia), request.Id);
        }

        // VALIDACIONES
        await _opeDatoAsistenciaService.ValidarRegistrosDuplicados(request.Id, request.IdOpePuerto, request.Fecha);
        // FIN VALIDACIONES

        //
        opeDatoAsistenciaToUpdate.Fecha = request.Fecha;
        opeDatoAsistenciaToUpdate.IdOpePuerto = request.IdOpePuerto;

        // DATOS ASISTENCIAS SANITARIAS
        if (request.OpeDatosAsistenciasSanitariasModificado)
        {
            var idsEnRequest = request.OpeDatosAsistenciasSanitarias?
                .Where(d => d.Id.HasValue && d.Id > 0)
            .Select(d => d.Id)
            .ToList() ?? new List<int?>();

            var opeDatosAsistenciasSanitariasParaEliminar = opeDatoAsistenciaToUpdate.OpeDatosAsistenciasSanitarias?
                .Where(d => d.Id > 0 && !idsEnRequest.Contains(d.Id)) 
                .ToList();

            if (opeDatosAsistenciasSanitariasParaEliminar?.Count > 0)
            {
                foreach (var opeDatoAsistenciaSanitariaParaEliminar in opeDatosAsistenciasSanitariasParaEliminar)
                {
                    _unitOfWork.Repository<OpeDatoAsistenciaSanitaria>().DeleteEntity(opeDatoAsistenciaSanitariaParaEliminar);
                }
            }

            OpeDatoAsistenciaSanitaria opeDatoAsistenciaSanitariaToSave;

            if (request.OpeDatosAsistenciasSanitarias?.Count > 0)
            {
                foreach (var opeDatoAsistenciaSanitaria in request.OpeDatosAsistenciasSanitarias)
                {
                    if (opeDatoAsistenciaSanitaria.Id == null || opeDatoAsistenciaSanitaria.Id == 0)
                    {
                        opeDatoAsistenciaSanitariaToSave = _mapper.Map<OpeDatoAsistenciaSanitaria>(opeDatoAsistenciaSanitaria);
                        opeDatoAsistenciaToUpdate.OpeDatosAsistenciasSanitarias.Add(opeDatoAsistenciaSanitariaToSave);
                    }
                    else
                    {
                        var opeDatoAsistenciaSanitariaActual = opeDatoAsistenciaToUpdate.OpeDatosAsistenciasSanitarias.FirstOrDefault(p => p.Id == opeDatoAsistenciaSanitaria.Id);
                        if (opeDatoAsistenciaSanitariaActual != null)
                        {
                            _mapper.Map(opeDatoAsistenciaSanitaria, opeDatoAsistenciaSanitariaActual);
                        }

                    }
                }
            }
        }

        // DATOS ASISTENCIAS SOCIALES
        if (request.OpeDatosAsistenciasSocialesModificado)
        {
            var idsEnRequest = request.OpeDatosAsistenciasSociales?
                .Where(d => d.Id.HasValue && d.Id > 0)
            .Select(d => d.Id)
            .ToList() ?? new List<int?>();

            var opeDatosAsistenciasSocialesParaEliminar = opeDatoAsistenciaToUpdate.OpeDatosAsistenciasSociales?
                .Where(d => d.Id > 0 && !idsEnRequest.Contains(d.Id))
                .ToList();

            if (opeDatosAsistenciasSocialesParaEliminar?.Count > 0)
            {
                foreach (var opeDatoAsistenciaSocialParaEliminar in opeDatosAsistenciasSocialesParaEliminar)
                {
                    _unitOfWork.Repository<OpeDatoAsistenciaSocial>().DeleteEntity(opeDatoAsistenciaSocialParaEliminar);
                }
            }

            OpeDatoAsistenciaSocial opeDatoAsistenciaSocialToSave;

            if (request.OpeDatosAsistenciasSociales?.Count > 0)
            {
                foreach (var opeDatoAsistenciaSocial in request.OpeDatosAsistenciasSociales)
                {
                    if (opeDatoAsistenciaSocial.Id == null || opeDatoAsistenciaSocial.Id == 0)
                    {
                        opeDatoAsistenciaSocialToSave = _mapper.Map<OpeDatoAsistenciaSocial>(opeDatoAsistenciaSocial);
                        opeDatoAsistenciaToUpdate.OpeDatosAsistenciasSociales.Add(opeDatoAsistenciaSocialToSave);
                    }
                    else
                    {
                        var opeDatoAsistenciaSocialActual = opeDatoAsistenciaToUpdate.OpeDatosAsistenciasSociales.FirstOrDefault(p => p.Id == opeDatoAsistenciaSocial.Id);
                        if (opeDatoAsistenciaSocialActual != null)
                        {
                            _mapper.Map(opeDatoAsistenciaSocial, opeDatoAsistenciaSocialActual);

                            // MAPEAMOS TAREAS
                            //var tareasRequest = opeDatoAsistenciaSocial.OpeDatosAsistenciasSocialesTareas;
                            var tareasRequest = opeDatoAsistenciaSocial.OpeDatosAsistenciasSocialesTareas ?? new List<CreateOpeDatoAsistenciaSocialTareaDto>();
                            var idsTareasEnRequest = opeDatoAsistenciaSocial.OpeDatosAsistenciasSocialesTareas?
                           .Where(d => d.Id.HasValue && d.Id > 0)
                           .Select(d => d.Id)
                           .ToList() ?? new List<int?>();

                            var tareasBD = opeDatoAsistenciaSocialActual?.OpeDatosAsistenciasSocialesTareas?.ToList() ?? new List<OpeDatoAsistenciaSocialTarea>();

                            // Eliminar tareas que ya no están
                            var tareasParaEliminar = tareasBD
                                .Where(t => t.Id > 0 && !idsTareasEnRequest.Contains(t.Id))
                                .ToList();

                            foreach (var tarea in tareasParaEliminar)
                            {
                                _unitOfWork.Repository<OpeDatoAsistenciaSocialTarea>().DeleteEntity(tarea);
                            }

                            // Agregar o actualizar tareas del request
                            foreach (var tareaDTO in tareasRequest)
                            {
                                if (tareaDTO.Id == null || tareaDTO.Id == 0)
                                {
                                    var nuevaTarea = _mapper.Map<OpeDatoAsistenciaSocialTarea>(tareaDTO);
                                    opeDatoAsistenciaSocialActual.OpeDatosAsistenciasSocialesTareas.Add(nuevaTarea);
                                }
                                else
                                {
                                    var tareaExistente = opeDatoAsistenciaSocialActual.OpeDatosAsistenciasSocialesTareas
                                        .FirstOrDefault(t => t.Id == tareaDTO.Id);

                                    if (tareaExistente != null)
                                    {
                                        _mapper.Map(tareaDTO, tareaExistente);
                                    }
                                }
                            }

                            // MAPEAMOS ORGANISMOS
                            //var organismosRequest = opeDatoAsistenciaSocial.OpeDatosAsistenciasSocialesOrganismos;
                            var organismosRequest = opeDatoAsistenciaSocial.OpeDatosAsistenciasSocialesOrganismos ?? new List<CreateOpeDatoAsistenciaSocialOrganismoDto>();
                            var idsOrganismosEnRequest = opeDatoAsistenciaSocial.OpeDatosAsistenciasSocialesOrganismos?
                           .Where(d => d.Id.HasValue && d.Id > 0)
                           .Select(d => d.Id)
                           .ToList() ?? new List<int?>();

                            var organismosBD = opeDatoAsistenciaSocialActual?.OpeDatosAsistenciasSocialesOrganismos?.ToList() ?? new List<OpeDatoAsistenciaSocialOrganismo>();

                            // Eliminar organismos que ya no están
                            var organismosParaEliminar = organismosBD
                                .Where(t => t.Id > 0 && !idsOrganismosEnRequest.Contains(t.Id))
                                .ToList();

                            foreach (var organismo in organismosParaEliminar)
                            {
                                _unitOfWork.Repository<OpeDatoAsistenciaSocialOrganismo>().DeleteEntity(organismo);
                            }

                            // Agregar o actualizar organismos del request
                            foreach (var organismoDTO in organismosRequest)
                            {
                                if (organismoDTO.Id == null || organismoDTO.Id == 0)
                                {
                                    var nuevoOrganismo = _mapper.Map<OpeDatoAsistenciaSocialOrganismo>(organismoDTO);
                                    opeDatoAsistenciaSocialActual.OpeDatosAsistenciasSocialesOrganismos.Add(nuevoOrganismo);
                                }
                                else
                                {
                                    var organismoExistente = opeDatoAsistenciaSocialActual.OpeDatosAsistenciasSocialesOrganismos
                                        .FirstOrDefault(t => t.Id == organismoDTO.Id);

                                    if (organismoExistente != null)
                                    {
                                        _mapper.Map(organismoDTO, organismoExistente);
                                    }
                                }
                            }
                            // MAPEAMOS USUARIOS
                            //var usuariosRequest = opeDatoAsistenciaSocial.OpeDatosAsistenciasSocialesUsuarios;
                            var usuariosRequest = opeDatoAsistenciaSocial.OpeDatosAsistenciasSocialesUsuarios ?? new List<CreateOpeDatoAsistenciaSocialUsuarioDto>();
                            var idsUsuariosEnRequest = opeDatoAsistenciaSocial.OpeDatosAsistenciasSocialesUsuarios?
                           .Where(d => d.Id.HasValue && d.Id > 0)
                           .Select(d => d.Id)
                           .ToList() ?? new List<int?>();

                            var usuariosBD = opeDatoAsistenciaSocialActual?.OpeDatosAsistenciasSocialesUsuarios?.ToList() ?? new List<OpeDatoAsistenciaSocialUsuario>();

                            // Eliminar usuarios que ya no están
                            var usuariosParaEliminar = usuariosBD
                                .Where(t => t.Id > 0 && !idsUsuariosEnRequest.Contains(t.Id))
                                .ToList();

                            foreach (var usuario in usuariosParaEliminar)
                            {
                                _unitOfWork.Repository<OpeDatoAsistenciaSocialUsuario>().DeleteEntity(usuario);
                            }

                            // Agregar o actualizar usuarios del request
                            foreach (var usuarioDTO in usuariosRequest)
                            {
                                if (usuarioDTO.Id == null || usuarioDTO.Id == 0)
                                {
                                    var nuevoUsuario = _mapper.Map<OpeDatoAsistenciaSocialUsuario>(usuarioDTO);
                                    opeDatoAsistenciaSocialActual.OpeDatosAsistenciasSocialesUsuarios.Add(nuevoUsuario);
                                }
                                else
                                {
                                    var usuarioExistente = opeDatoAsistenciaSocialActual.OpeDatosAsistenciasSocialesUsuarios
                                        .FirstOrDefault(t => t.Id == usuarioDTO.Id);

                                    if (usuarioExistente != null)
                                    {
                                        _mapper.Map(usuarioDTO, usuarioExistente);
                                    }
                                }
                            }
                        }

                    }

                   

                }
            }
        }


        // DATOS ASISTENCIAS TRADUCCIONES
        if (request.OpeDatosAsistenciasTraduccionesModificado)
        {
            var idsEnRequest = request.OpeDatosAsistenciasTraducciones?
                .Where(d => d.Id.HasValue && d.Id > 0)
            .Select(d => d.Id)
            .ToList() ?? new List<int?>();

            var opeDatosAsistenciasTraduccionesParaEliminar = opeDatoAsistenciaToUpdate.OpeDatosAsistenciasTraducciones?
                .Where(d => d.Id > 0 && !idsEnRequest.Contains(d.Id)) // Solo las direcciones que ya existen en la base de datos y no están en el request
                .ToList();

            if (opeDatosAsistenciasTraduccionesParaEliminar?.Count > 0)
            {
                foreach (var opeDatoAsistenciaTraduccionParaEliminar in opeDatosAsistenciasTraduccionesParaEliminar)
                {
                    _unitOfWork.Repository<OpeDatoAsistenciaTraduccion>().DeleteEntity(opeDatoAsistenciaTraduccionParaEliminar);
                }
            }

            OpeDatoAsistenciaTraduccion opeDatoAsistenciaTraduccionToSave;

            if (request.OpeDatosAsistenciasTraducciones?.Count > 0) 
            { 
                foreach (var opeDatoAsistenciaTraduccion in request.OpeDatosAsistenciasTraducciones)
                {
                    if (opeDatoAsistenciaTraduccion.Id == null || opeDatoAsistenciaTraduccion.Id == 0)
                    {
                        opeDatoAsistenciaTraduccionToSave = _mapper.Map<OpeDatoAsistenciaTraduccion>(opeDatoAsistenciaTraduccion);
                        opeDatoAsistenciaToUpdate.OpeDatosAsistenciasTraducciones.Add(opeDatoAsistenciaTraduccionToSave);
                    }
                    else
                    {
                        var opeDatoAsistenciaTraduccionActual = opeDatoAsistenciaToUpdate.OpeDatosAsistenciasTraducciones.FirstOrDefault(p => p.Id == opeDatoAsistenciaTraduccion.Id);
                        if (opeDatoAsistenciaTraduccionActual != null)
                        {
                            _mapper.Map(opeDatoAsistenciaTraduccion, opeDatoAsistenciaTraduccionActual);
                        }

                    }
                }
             }
        }
        //

        //_mapper.Map(request, opeDatoAsistenciaToUpdate, typeof(UpdateOpeDatoAsistenciaCommand), typeof(OpeDatoAsistencia));

        _unitOfWork.Repository<OpeDatoAsistencia>().UpdateEntity(opeDatoAsistenciaToUpdate);
        await _unitOfWork.Complete();

        _logger.LogInformation($"Se actualizo correctamente el ope dato de asistencia con id: {request.Id}");
        _logger.LogInformation(nameof(UpdateOpeDatoAsistenciaCommandHandler) + " - END");

        return Unit.Value;
    }
}
