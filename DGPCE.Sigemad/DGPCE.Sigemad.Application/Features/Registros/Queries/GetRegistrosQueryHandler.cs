using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.Evoluciones;
using DGPCE.Sigemad.Application.Dtos.Impactos;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.Evoluciones.Queries.GetEvolucion;
using DGPCE.Sigemad.Application.Specifications.Registros;
using DGPCE.Sigemad.Application.Specifications.RegistrosActualizaciones;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Registros.Queries;
public class GetRegistrosQueryHandler : IRequestHandler<GetRegistrosQuery, RegistroEvolucionDto>
{
    private readonly ILogger<GetRegistrosQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetRegistrosQueryHandler(
        ILogger<GetRegistrosQueryHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    public async Task<RegistroEvolucionDto> Handle(GetRegistrosQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(GetEvolucionQueryHandler)} - BEGIN");

        RegistroActualizacion registroActualizacion = null;
        Registro registro = null;
        List<int> idsAreaAfectdas = new List<int>();
        List<int> idsParametro = new List<int>();
        List<int> idsConsecuenciaActuacion = new List<int>();
        List<int> idsIntervencionMedios = new List<int>();

        List<int> idsDirecciones = new List<int>();
        List<int> idsCecopi = new List<int>();
        List<int> idsPma = new List<int>();

        List<int> idsActivacionPlanEmergencias = new List<int>();
        List<int> idsActivacionSistemas = new List<int>();

        List<int> idsAreaAfectdasEliminables = new List<int>();
        List<int> idsParametroEliminables = new List<int>();
        List<int> idsConsecuenciaActuacionEliminables = new List<int>();
        List<int> idsIntervencionMediosEliminables = new List<int>();
        List<int> idsDireccionesEliminables = new List<int>();
        List<int> idsCecopiEliminables = new List<int>();
        List<int> idsPmaEliminables = new List<int>();
        List<int> idsActivacionPlanEmergenciasEliminables = new List<int>();
        List<int> idsActivacionSistemasEliminables = new List<int>();

        bool includeRegistro = false;
        bool includeDatoPrincipal = false;
        IReadOnlyList<Registro> registrosPosteriores = new List<Registro>();

        if (request.IdRegistroActualizacion > 0)
        {
            _logger.LogInformation($"Buscando Registro para IdRegistroActualizacion: {request.IdRegistroActualizacion}");
            // Obtener RegistroActualizacion con IdReferencia (Evolucion.Id)
            var registroSpec = new RegistroActualizacionSpecificationParams
            {
                Id = request.IdRegistroActualizacion
            };
            registroActualizacion = await _unitOfWork.Repository<RegistroActualizacion>()
                .GetByIdWithSpec(new RegistroActualizacionSpecification(registroSpec));

            if (registroActualizacion == null)
            {
                _logger.LogWarning($"No se encontró RegistroActualizacion con Id: {request.IdRegistroActualizacion}");
                throw new NotFoundException(nameof(RegistroActualizacion), request.IdRegistroActualizacion);
            }

            foreach (var detalle in registroActualizacion.DetallesRegistro)
            {
                switch (detalle.IdApartadoRegistro)
                {
                    case (int)ApartadoRegistroEnum.AreaAfectada:
                        idsAreaAfectdas.Add(detalle.IdReferencia);
                        if (IsEliminable(detalle.IdEstadoRegistro)) idsAreaAfectdasEliminables.Add(detalle.IdReferencia);
                        break;

                    case (int)ApartadoRegistroEnum.Parametro:
                        idsParametro.Add(detalle.IdReferencia);
                        if (IsEliminable(detalle.IdEstadoRegistro)) idsParametroEliminables.Add(detalle.IdReferencia);
                        break;


                    case (int)ApartadoRegistroEnum.ConsecuenciaActuacion:
                        idsConsecuenciaActuacion.Add(detalle.IdReferencia);
                        if (IsEliminable(detalle.IdEstadoRegistro)) idsConsecuenciaActuacionEliminables.Add(detalle.IdReferencia);
                        break;

                    case (int)ApartadoRegistroEnum.IntervencionMedios:
                        idsIntervencionMedios.Add(detalle.IdReferencia);
                        if (IsEliminable(detalle.IdEstadoRegistro)) idsIntervencionMediosEliminables.Add(detalle.IdReferencia);
                        break;

                    case (int)ApartadoRegistroEnum.Direccion:
                        idsDirecciones.Add(detalle.IdReferencia);
                        if (IsEliminable(detalle.IdEstadoRegistro)) idsDireccionesEliminables.Add(detalle.IdReferencia);
                        break;

                    case (int)ApartadoRegistroEnum.CoordinacionPMA:
                        idsPma.Add(detalle.IdReferencia);
                        if (IsEliminable(detalle.IdEstadoRegistro)) idsPmaEliminables.Add(detalle.IdReferencia);
                        break;

                    case (int)ApartadoRegistroEnum.CoordinacionCECOPI:
                        idsCecopi.Add(detalle.IdReferencia);
                        if (IsEliminable(detalle.IdEstadoRegistro)) idsCecopiEliminables.Add(detalle.IdReferencia);
                        break;

                    case (int)ApartadoRegistroEnum.ActivacionDePlanes:
                        idsActivacionPlanEmergencias.Add(detalle.IdReferencia);
                        if (IsEliminable(detalle.IdEstadoRegistro)) idsActivacionPlanEmergenciasEliminables.Add(detalle.IdReferencia);
                        break;

                    case (int)ApartadoRegistroEnum.ActivacionDeSistemas:
                        idsActivacionSistemas.Add(detalle.IdReferencia);
                        if (IsEliminable(detalle.IdEstadoRegistro)) idsActivacionSistemasEliminables.Add(detalle.IdReferencia);
                        break;

                }
            }

            // Buscar la registro por IdReferencia
            registro = await _unitOfWork.Repository<Registro>()
                    .GetByIdWithSpec(new RegistroWithFilteredDataSpecification(
                        registroActualizacion.IdReferencia, idsAreaAfectdas, idsParametro, idsConsecuenciaActuacion, idsIntervencionMedios, idsDirecciones, idsCecopi, idsPma, idsActivacionPlanEmergencias: idsActivacionPlanEmergencias,idsActivacionSistemas: idsActivacionSistemas));


            if (registro == null)
            {
                _logger.LogWarning($"No se encontró IdRegistroActualizacion con Id: {registroActualizacion.IdReferencia}");
                throw new NotFoundException(nameof(Registro), registroActualizacion.IdReferencia);
            }

            registrosPosteriores = await _unitOfWork.Repository<Registro>()
                    .GetAllWithSpec(new ComprobacionRegistrosPosterioresSpecifications(
                        new ComprobacionRegistrosPosterioresParams { IdSuceso = registro.IdSuceso,
                                                                    FechaCreacion = registro.FechaCreacion,
                                                                    Id = registro.Id}));



            registro.TipoImpactosEvoluciones = registro.TipoImpactosEvoluciones
             .Select(tipo =>
             {
                 tipo.ImpactosEvoluciones = tipo.ImpactosEvoluciones
                 .Where(ie => !ie.Borrado)
                 .ToList();
                 return tipo;
                         })
                         .ToList();
            }

        // Mapear registro a RegistroEvolucionDto
        var registroDto = _mapper.Map<RegistroEvolucionDto>(registro);


        //foreach (var activacion in registroDto.ActivacionPlanEmergencias)
        //{
        //    var activacionOriginal = registro.ActivacionPlanEmergencias
        //        .FirstOrDefault(a => a.Id == activacion.Id);

        //    if (activacion.PlanEmergencia != null && activacionOriginal?.PlanEmergencia != null)
        //    {
        //        activacion.PlanEmergencia.Descripcion = $"{activacionOriginal.PlanEmergencia.Codigo} - {activacionOriginal.PlanEmergencia.Descripcion}";
        //    }
        //}


        if (registrosPosteriores.Any())
        {
            registroDto.RegistrosPosterioresConAreasAfectadas = true;
         }


        registroDto.AreaAfectadas.ForEach(area => area.EsEliminable = idsAreaAfectdasEliminables.Contains(area.Id));
        registroDto.Parametros.ForEach(parametro => parametro.EsEliminable = idsParametroEliminables.Contains(parametro.Id));
        registroDto.TipoImpactosEvoluciones.ForEach(impacto =>
        {
            impacto.EsEliminable = idsConsecuenciaActuacionEliminables.Contains(impacto.Id);
            // Sumar los valores de 'Total' de los impactos no borrados
            impacto.Total = impacto.ImpactosEvoluciones
                .Sum(ie => ie.Numero);
        });

        registroDto.IntervencionMedios.ForEach(intervencion => intervencion.EsEliminable = idsIntervencionMedios.Contains(intervencion.Id));
        registroDto.Direcciones.ForEach(direccion => direccion.EsEliminable = idsDirecciones.Contains(direccion.Id));
        registroDto.CoordinacionesPMA.ForEach(coordinacionPMA => coordinacionPMA.EsEliminable = idsPma.Contains(coordinacionPMA.Id));
        registroDto.CoordinacionesCecopi.ForEach(CoordinacionCecopi => CoordinacionCecopi.EsEliminable = idsCecopi.Contains(CoordinacionCecopi.Id));
        registroDto.ActivacionPlanEmergencias.ForEach(activacionPlan => activacionPlan.EsEliminable = idsActivacionPlanEmergencias.Contains(activacionPlan.Id));
        registroDto.ActivacionSistemas.ForEach(ActivacionSistema => ActivacionSistema.EsEliminable = idsActivacionSistemas.Contains(ActivacionSistema.Id));

        //registroDto.ActivacionPlanEmergenciasRegionales = _mapper
        // .Map<List<ActivacionPlanEmergenciaDto>>(registro.ActivacionPlanEmergencias
        // .Where(p => p.TipoPlan?.Ambito == 'R'))
        // .Select(dto => { dto.EsEliminable = true; return dto; })
        //    .ToList();

        //registroDto.ActivacionPlanEmergenciasEstatales = _mapper
        // .Map<List<ActivacionPlanEmergenciaDto>>(registro.ActivacionPlanEmergencias
        // .Where(p => p.TipoPlan?.Ambito == 'E'))
        // .Select(dto => { dto.EsEliminable = true; return dto; })
        //    .ToList();

        

        //ES NUEVO O MODIFICADO

        foreach (var parametro in registroDto.Parametros)
        {
            var parametroOriginal = registro.Parametros.FirstOrDefault(p => p.Id == parametro.Id);

            if (parametroOriginal != null)
            {
                parametro.FechaCreacion = parametroOriginal.FechaCreacion;
                parametro.FechaModificacion = parametroOriginal.FechaModificacion;

                parametro.EsNuevo = (DateTime.UtcNow - parametroOriginal.FechaCreacion).TotalDays <= 3
                                    || parametroOriginal.FechaModificacion == parametroOriginal.FechaCreacion;

                parametro.EsModificado = parametroOriginal.FechaModificacion.HasValue
                                         && parametroOriginal.FechaModificacion > parametroOriginal.FechaCreacion;
            }
        }

        foreach (var area in registroDto.AreaAfectadas)
        {
            var areaOriginal = registro.AreaAfectadas.FirstOrDefault(a => a.Id == area.Id);

            if (areaOriginal != null)
            {
                area.EsNuevo = (DateTime.UtcNow - areaOriginal.FechaCreacion).TotalDays <= 3
                               || (areaOriginal.FechaModificacion.HasValue && areaOriginal.FechaModificacion == areaOriginal.FechaCreacion);

                area.EsModificado = areaOriginal.FechaModificacion.HasValue
                                    && areaOriginal.FechaModificacion > areaOriginal.FechaCreacion;
            }
        }


        foreach (var tipo in registroDto.TipoImpactosEvoluciones)
        {
            foreach (var impacto in tipo.ImpactosEvoluciones)
            {
                var impactoOriginal = registro.TipoImpactosEvoluciones
                    .SelectMany(t => t.ImpactosEvoluciones)
                    .FirstOrDefault(i => i.Id == impacto.Id);

                if (impactoOriginal != null)
                {
                    impacto.EsNuevo = (DateTime.UtcNow - impactoOriginal.FechaCreacion).TotalDays <= 3
                                      || (impactoOriginal.FechaModificacion.HasValue && impactoOriginal.FechaModificacion == impactoOriginal.FechaCreacion);

                    impacto.EsModificado = impactoOriginal.FechaModificacion.HasValue
                                           && impactoOriginal.FechaModificacion > impactoOriginal.FechaCreacion;
                }
            }
        }


        foreach (var intervencion in registroDto.IntervencionMedios)
        {
            var intervencionOriginal = registro.IntervencionMedios.FirstOrDefault(i => i.Id == intervencion.Id);

            if (intervencionOriginal != null)
            {
                intervencion.EsNuevo = (DateTime.UtcNow - intervencionOriginal.FechaCreacion).TotalDays <= 3
                                       || (intervencionOriginal.FechaModificacion.HasValue && intervencionOriginal.FechaModificacion == intervencionOriginal.FechaCreacion);

                intervencion.EsModificado = intervencionOriginal.FechaModificacion.HasValue
                                            && intervencionOriginal.FechaModificacion > intervencionOriginal.FechaCreacion;
            }
        }

        //ES NUEVO O MODIFICADO FIN

        _logger.LogInformation($"{nameof(GetEvolucionQueryHandler)} - END");


        return registroDto;
    }

    private bool IsEliminable(EstadoRegistroEnum estado)
    {
        return estado == EstadoRegistroEnum.Creado ||
            estado == EstadoRegistroEnum.CreadoYModificado;
    }
}
