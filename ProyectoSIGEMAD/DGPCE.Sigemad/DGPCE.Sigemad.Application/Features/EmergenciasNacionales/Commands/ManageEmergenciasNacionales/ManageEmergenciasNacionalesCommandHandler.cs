using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Contracts.RegistrosActualizacion;
using DGPCE.Sigemad.Application.Dtos.EmergenciasNacionales;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.ConvocatoriasCECOD.Commands;
using DGPCE.Sigemad.Application.Features.DireccionCoordinacionEmergencias.Commands.Manage;
using DGPCE.Sigemad.Application.Specifications.ActuacionesRelevantesDGPCE;
using DGPCE.Sigemad.Application.Specifications.Incendios;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DGPCE.Sigemad.Application.Features.EmergenciasNacionales.Commands.ManageEmergenciasNacionales;
public class ManageEmergenciasNacionalesCommandHandler : IRequestHandler<ManageEmergenciasNacionalesCommand, ManageEmergenciaNacionalResponse>
{

    private readonly ILogger<ManageEmergenciasNacionalesCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRegistroActualizacionService _registroActualizacionService;

    public ManageEmergenciasNacionalesCommandHandler(
        ILogger<ManageEmergenciasNacionalesCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IRegistroActualizacionService registroActualizacionService
    )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _registroActualizacionService = registroActualizacionService;
    }

    public async Task<ManageEmergenciaNacionalResponse> Handle(ManageEmergenciasNacionalesCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(ManageEmergenciasNacionalesCommandHandler)} - BEGIN");

        await _registroActualizacionService.ValidarSuceso(request.IdSuceso);
        await _unitOfWork.BeginTransactionAsync();
        await ValidateFechas(request);

        try
        {
            RegistroActualizacion registroActualizacion = await _registroActualizacionService.GetOrCreateRegistroActualizacion<ActuacionRelevanteDGPCE>(
                request.IdRegistroActualizacion, request.IdSuceso, TipoRegistroActualizacionEnum.ActuacionRelevante);

            ActuacionRelevanteDGPCE actuacion = await GetOrCreateDireccionCoordinacion(request, registroActualizacion);

            var emergenciaNacionalOriginal = _mapper.Map<ManageEmergenciaNacionalDto>(actuacion.EmergenciaNacional);

            MapAndSaveActuacionRelevante(request, actuacion, registroActualizacion.Id);

            //No hay listas para eliminar objeto
            await SaveActuacionRelevante(actuacion);
            MapAndSaveRegistroActualizacion(registroActualizacion, actuacion, emergenciaNacionalOriginal);


            await _registroActualizacionService.SaveRegistroActualizacion<
            ActuacionRelevanteDGPCE, EmergenciaNacional, ManageEmergenciaNacionalDto>(
            registroActualizacion,
            actuacion,
            ApartadoRegistroEnum.EmergenciaNacional,
            new List<int>(), null);


            await _unitOfWork.CommitAsync();

            _logger.LogInformation($"{nameof(CreateOrUpdateDireccionCoordinacionCommandHandler)} - END");

            return new ManageEmergenciaNacionalResponse
            {
                IdActuacionRelevante = actuacion.Id,
                IdRegistroActualizacion = registroActualizacion.Id
            };

        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Error en la transacción de CreateOrUpdateDireccionCommandHandler");
            throw;
        }
    }

    private async Task ValidateFechas(ManageEmergenciasNacionalesCommand request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (request.EmergenciaNacional == null)
            throw new BadRequestException("La Emergencia nacional no pueden estar vacia.");

        var incendioAsociado = await _unitOfWork.Repository<Incendio>()
           .GetByIdWithSpec(new IncendioActiveByIdSpecification(request.IdSuceso))
           ?? throw new BadRequestException($"No se encontró el incendio con ID {request.IdSuceso}.");

        if(request.EmergenciaNacional.FechaHoraDeclaracion < request.EmergenciaNacional.FechaHoraSolicitud)
        {
            throw new BadRequestException("La fecha declaracion no puede ser menor que la fecha Solicitud.");
        }

        if(request.EmergenciaNacional.FechaHoraSolicitud < incendioAsociado.FechaInicio)
        {
            throw new BadRequestException("La fecha Solicitud no puede ser menor que la fecha Inicio del Incendio.");
        }

        if (request.EmergenciaNacional.FechaHoraSolicitud > DateTime.Now)
        {
            throw new BadRequestException("La fecha Solicitud no puede ser mayor que la fecha del sistema.");
        }
    }

    private void MapAndSaveRegistroActualizacion(
        RegistroActualizacion registroActualizacion,
        ActuacionRelevanteDGPCE actuacion,
        ManageEmergenciaNacionalDto originalEmergencia)
    {
        registroActualizacion.IdReferencia = actuacion.Id;


        // Agregar registro de registro
        DetalleRegistroActualizacion detalleRegistro = new()
        {
            IdApartadoRegistro = (int)ApartadoRegistroEnum.EmergenciaNacional,
            IdReferencia = actuacion.EmergenciaNacional.Id,
        };

        var copiaNuevoRegistro = _mapper.Map<ManageEmergenciaNacionalDto>(actuacion.EmergenciaNacional);
        if (originalEmergencia == null && copiaNuevoRegistro != null)
        {
            detalleRegistro.IdEstadoRegistro = EstadoRegistroEnum.Creado;
        }
        else if (originalEmergencia.Equals(copiaNuevoRegistro))
        {
            detalleRegistro.IdEstadoRegistro = EstadoRegistroEnum.Modificado;
        }
        else
        {
            detalleRegistro.IdEstadoRegistro = EstadoRegistroEnum.Permanente;
        }

        var detallePrevioRegistro = registroActualizacion.DetallesRegistro
            .FirstOrDefault(d => d.IdReferencia == actuacion.EmergenciaNacional.Id && d.IdApartadoRegistro == (int)ApartadoRegistroEnum.EmergenciaNacional);

        if (detallePrevioRegistro != null)
        {
            if (!originalEmergencia.Equals(copiaNuevoRegistro))
            {
                if (detallePrevioRegistro.IdEstadoRegistro == EstadoRegistroEnum.Creado ||
                    detallePrevioRegistro.IdEstadoRegistro == EstadoRegistroEnum.CreadoYModificado)
                {
                    detallePrevioRegistro.IdEstadoRegistro = EstadoRegistroEnum.CreadoYModificado;
                }

                detallePrevioRegistro.IdEstadoRegistro = EstadoRegistroEnum.Modificado;
            }
            detallePrevioRegistro.IdEstadoRegistro = EstadoRegistroEnum.Permanente;
        }
        else
        {
            registroActualizacion.DetallesRegistro.Add(detalleRegistro);
        }
    }

    private void MapAndSaveActuacionRelevante(ManageEmergenciasNacionalesCommand request, ActuacionRelevanteDGPCE actuacion, int? idRegistroActualizacion)
    {
        if (request.EmergenciaNacional != null)
        {
            if (actuacion.EmergenciaNacional != null)
            {
                var copiaOriginal = _mapper.Map<ManageEmergenciaNacionalDto>(actuacion.EmergenciaNacional);
                var copiaNueva = _mapper.Map<ManageEmergenciaNacionalDto>(request.EmergenciaNacional);
                if (!copiaOriginal.Equals(copiaNueva))
                {
                    _mapper.Map(request.EmergenciaNacional, actuacion.EmergenciaNacional);
                    actuacion.Borrado = false;
                }
            }
            else
            {
                actuacion.EmergenciaNacional = _mapper.Map<EmergenciaNacional>(request.EmergenciaNacional);
            }
        }
      }
    

    private async Task SaveActuacionRelevante(ActuacionRelevanteDGPCE actuacionRelevante)
    {
        if (actuacionRelevante.Id > 0)
        {
            _unitOfWork.Repository<ActuacionRelevanteDGPCE>().UpdateEntity(actuacionRelevante);
        }
        else
        {
            _unitOfWork.Repository<ActuacionRelevanteDGPCE>().AddEntity(actuacionRelevante);
        }

        if (await _unitOfWork.Complete() <= 0)
            throw new Exception("No se pudo insertar/actualizar la actuacionRelevante");
    }

    private async Task<List<int>> MapAndSaveOrDeleteEmergenciaNacional(ManageEmergenciasNacionalesCommand request, ActuacionRelevanteDGPCE actuacionRelevante)
    {
        List<int> idemergenciaEliminar  = new List<int>();

        if (actuacionRelevante.EmergenciaNacional !=null && request.EmergenciaNacional != null)
        {
                var copiaOriginal = _mapper.Map<ManageEmergenciaNacionalDto>(actuacionRelevante.EmergenciaNacional);
                var copiaNueva = _mapper.Map<ManageEmergenciaNacionalDto>(request.EmergenciaNacional);

                if (!copiaOriginal.Equals(copiaNueva))
                {
                    _mapper.Map(request.EmergenciaNacional, actuacionRelevante.EmergenciaNacional);
                    actuacionRelevante.EmergenciaNacional.Borrado = false;
                }
        } else if (actuacionRelevante.EmergenciaNacional != null && !actuacionRelevante.EmergenciaNacional.Borrado)
        {
            // Eliminar lógicamente la emergencia nacional si no se envía en la solicitud
            idemergenciaEliminar.Add(actuacionRelevante.EmergenciaNacional.Id);
            _unitOfWork.Repository<EmergenciaNacional>().DeleteEntity(actuacionRelevante.EmergenciaNacional);
        }else
        {
          actuacionRelevante.EmergenciaNacional = _mapper.Map<EmergenciaNacional>(request.EmergenciaNacional);
        }

        return idemergenciaEliminar;
    }
       

    private async Task<ActuacionRelevanteDGPCE> GetOrCreateDireccionCoordinacion(ManageEmergenciasNacionalesCommand request, RegistroActualizacion registroActualizacion)
    {
        if (registroActualizacion.IdReferencia > 0)
        {
            List<int> idsReferencias = new List<int>();
            bool includeEmergenciaNacional = true;
            List<int> idsActivacionPlanEmergencias = new List<int>();
            List<int> idsDeclaracionesZAGEP = new List<int>();
            List<int> idsActivacionSistemas = new List<int>();
            List<int> idsConvocatoriasCECOD = new List<int>();
            List<int> idsNotificacionesEmergencias = new List<int>();
            List<int> idsMovilizacionMedios = new List<int>();

            // Separar IdReferencia según su tipo
            foreach (var detalle in registroActualizacion.DetallesRegistro)
            {
                switch (detalle.IdApartadoRegistro)
                {
                    case (int)ApartadoRegistroEnum.EmergenciaNacional:
                        includeEmergenciaNacional = true;
                        break;
                    case (int)ApartadoRegistroEnum.ActivacionDePlanes:
                        idsActivacionPlanEmergencias.Add(detalle.IdReferencia);
                        break;
                    case (int)ApartadoRegistroEnum.DeclaracionZAGEP:
                        idsDeclaracionesZAGEP.Add(detalle.IdReferencia);
                        break;
                    case (int)ApartadoRegistroEnum.ActivacionDeSistemas:
                        idsActivacionSistemas.Add(detalle.IdReferencia);
                        break;
                    case (int)ApartadoRegistroEnum.ConvocatoriaCECOD:
                        idsConvocatoriasCECOD.Add(detalle.IdReferencia);
                        break;
                    case (int)ApartadoRegistroEnum.NotificacionesOficiales:
                        idsNotificacionesEmergencias.Add(detalle.IdReferencia);
                        break;
                    case (int)ApartadoRegistroEnum.MovilizacionMedios:
                        idsMovilizacionMedios.Add(detalle.IdReferencia);
                        break;
                    default:
                        idsReferencias.Add(detalle.IdReferencia);
                        break;
                }
            }

            // Buscar la Dirección y Coordinación de Emergencia por IdReferencia
            var actuacionRelevante = await _unitOfWork.Repository<ActuacionRelevanteDGPCE>()
                .GetByIdWithSpec(new ActuacionRelevanteDGPCEWithFilteredData(registroActualizacion.IdReferencia, idsActivacionPlanEmergencias, idsDeclaracionesZAGEP, idsActivacionSistemas, idsConvocatoriasCECOD, idsNotificacionesEmergencias, idsMovilizacionMedios, includeEmergenciaNacional));

            if (actuacionRelevante is null || actuacionRelevante.Borrado)
                throw new BadRequestException($"El registro de actualización con Id [{registroActualizacion.Id}] no tiene registro de actuacionRelevanteDGPCE");

            return actuacionRelevante;
        }

        // Validar si ya existe un registro de Dirección y Coordinación de Emergencia para este suceso
        var specSuceso = new ActuacionRelevanteDGPCEWithEmergenciaNacional(new ActuacionRelevanteDGPCESpecificationParams { IdSuceso = request.IdSuceso });
        var emergenciaNacionalExistente = await _unitOfWork.Repository<ActuacionRelevanteDGPCE>().GetByIdWithSpec(specSuceso);

        return emergenciaNacionalExistente ?? new ActuacionRelevanteDGPCE { IdSuceso = request.IdSuceso };
    }


}


