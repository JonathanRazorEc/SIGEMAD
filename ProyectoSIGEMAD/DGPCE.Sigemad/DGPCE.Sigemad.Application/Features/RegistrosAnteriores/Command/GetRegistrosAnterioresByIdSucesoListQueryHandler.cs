using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.ActivacionesPlanes;
using DGPCE.Sigemad.Application.Dtos.ActivacionSistema;
using DGPCE.Sigemad.Application.Dtos.AreasAfectadas;
using DGPCE.Sigemad.Application.Dtos.CoordinacionCecopis;
using DGPCE.Sigemad.Application.Dtos.CoordinacionesPMA;
using DGPCE.Sigemad.Application.Dtos.Direcciones;
using DGPCE.Sigemad.Application.Dtos.IntervencionMedios;
using DGPCE.Sigemad.Application.Dtos.Parametros.Dtos;
using DGPCE.Sigemad.Application.Dtos.RegistrosAnteriores;
using DGPCE.Sigemad.Application.Dtos.TipoImpactoEvolucion;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.Evoluciones.Queries.GetEvolucion;
using DGPCE.Sigemad.Application.Specifications.Registros;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DGPCE.Sigemad.Application.Features.RegistrosAnteriores.Command;

public class GetRegistrosAnterioresByIdSucesoListQueryHandler : IRequestHandler<GetRegistrosAnterioresByIdSucesoListQuery, IReadOnlyList<RegistrosAnterioresDto>>
{
    private readonly ILogger<GetRegistrosAnterioresByIdSucesoListQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;


    public GetRegistrosAnterioresByIdSucesoListQueryHandler(
        ILogger<GetRegistrosAnterioresByIdSucesoListQueryHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<RegistrosAnterioresDto>> Handle(GetRegistrosAnterioresByIdSucesoListQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(GetEvolucionQueryHandler)} - BEGIN");

        IReadOnlyList<Registro>? registros = null;
        List<int> idsAreaAfectdas = new List<int>();
        List<int> idsParametros = new List<int>();
        List<int> idsConsecuenciaActuacion = new List<int>();
        List<int> idsIntervencionMedios = new List<int>();

        List<int> idsAreaAfectdasEliminables = new List<int>();
        List<int> idsParametrosEliminables = new List<int>();
        List<int> idsConsecuenciaActuacionEliminables = new List<int>();
        List<int> idsIntervencionMediosEliminables = new List<int>();
        List<int> idsActivacionPlanEmergenciasEliminables = new List<int>();
        List<int> idsActivacionSistemasEliminables = new List<int>();

        List<int> idsDirecciones = new List<int>();
        List<int> idsCecopi = new List<int>();
        List<int> idsPma = new List<int>();

        List<int> idsActivacionPlanEmergencias = new List<int>();
        List<int> idsActivacionSistemas = new List<int>();

        _logger.LogInformation($"Buscando registros  para IdSuceso: {request.IdSuceso}");

        var registroParams = new RegistroSpecificationParams { IdSuceso = request.IdSuceso };

        if (request.IdRegistro.HasValue && request.IdRegistro.Value > 0)
        {
            var registro = await _unitOfWork.Repository<Registro>().GetByIdAsync(request.IdRegistro.Value);

            if (registro is null)
                throw new BadRequestException($"El registro con Id [{request.IdSuceso}] no existe");

            registroParams.FechaRegistro = registro.FechaCreacion;
        }


        // Buscar Evolución por IdSuceso
        registros = await _unitOfWork.Repository<Registro>()
            .GetAllWithSpec(new RegistroSpecification(registroParams));


        if (registros is null)
            throw new BadRequestException($"El suceso con Id [{request.IdSuceso}] no tiene registros");

        // Mapear registros a RegistrosAnterioresDto
        IReadOnlyList<RegistrosAnterioresDto> registroAnterioresDto = _mapper.Map<IReadOnlyList<RegistrosAnterioresDto>>(registros);

        // Unificar todas las listas en una sola lista


        var ultimoRegistroConAreasAfectadas = _mapper.Map<IReadOnlyList<AreaAfectadaDto>>(
             registros
             .Where(r => r.AreaAfectadas.Any())
             .OrderByDescending(r => r.FechaCreacion)
             .Take(1)
             .SelectMany(r => r.AreaAfectadas)
            ).ToList();


        // Obtener el último registro que tenga al menos una de las tres listas con elementos
        var ultimoRegistroConDireccionCoordinacion = registros
         .Where(r => r.Direcciones.Any() || r.CoordinacionesCecopi.Any() || r.CoordinacionesPMA.Any())
         .OrderByDescending(r => r.FechaCreacion)
         .FirstOrDefault();

        // Mapear las listas solo si el registro existe
        var ultimoRegistroConDirecciones = ultimoRegistroConDireccionCoordinacion != null
         ? _mapper.Map<List<DireccionDto>>(ultimoRegistroConDireccionCoordinacion.Direcciones)
         : new List<DireccionDto>();

        var ultimoRegistroConCoordinacionesCecopi = ultimoRegistroConDireccionCoordinacion != null
         ? _mapper.Map<List<CoordinacionCecopiDto>>(ultimoRegistroConDireccionCoordinacion.CoordinacionesCecopi)
         : new List<CoordinacionCecopiDto>();

        var ultimoRegistroConCoordinacionesPMA = ultimoRegistroConDireccionCoordinacion != null
         ? _mapper.Map<List<CoordinacionPMADto>>(ultimoRegistroConDireccionCoordinacion.CoordinacionesPMA)
         : new List<CoordinacionPMADto>();


        var ultimoRegistroConConsecuenciasActuaciones = _mapper.Map<IReadOnlyList<TipoImpactoEvolucionDTO>>(
         registros
         .Where(r => r.TipoImpactosEvoluciones.Any())
         .OrderByDescending(r => r.FechaCreacion)
         .Take(1)
         .SelectMany(r => r.TipoImpactosEvoluciones)
         .Select(tipo =>{tipo.ImpactosEvoluciones = tipo.ImpactosEvoluciones
                 .Where(ie => !ie.Borrado)
                 .ToList();
                    return tipo;
                     })
                ).ToList();


        var ultimoRegistroConActivacionPlanes = _mapper.Map<IReadOnlyList<ActivacionPlanEmergenciaDto>>(registros
          .Where(r => r.ActivacionPlanEmergencias.Any())
          .OrderByDescending(r => r.FechaCreacion)
          .Take(1)
          .SelectMany(r => r.ActivacionPlanEmergencias)).ToList();
        //.Select(p =>
        //{
        //    var dto = _mapper.Map<ActivacionPlanEmergenciaDto>(p);

        //    if (dto.PlanEmergencia != null && p.PlanEmergencia != null)
        //    {
        //        dto.PlanEmergencia.Descripcion = $"{p.PlanEmergencia.Codigo} - {p.PlanEmergencia.Descripcion}";
        //    }

        //    return dto;
        //}) .ToList();


        var ultimoRegistroConParametros = _mapper.Map<IReadOnlyList<ParametroDto>>(
          registros
          .Where(r => r.Parametros.Any())
          .OrderByDescending(r => r.FechaCreacion)
          .Take(1)
          .SelectMany(r => r.Parametros)
         ).ToList();


        var ultimoRegistroConIntervencionMedios = _mapper.Map<IReadOnlyList<IntervencionMedioDto>>(
         registros
         .Where(r => r.IntervencionMedios.Any())
         .OrderByDescending(r => r.FechaCreacion)
         .Take(1)
         .SelectMany(r => r.IntervencionMedios)
        ).ToList();


        var ultimoRegistroConActivacionDeSistemas = _mapper.Map<IReadOnlyList<ActivacionSistemaDto>>(
             registros
             .Where(r => r.ActivacionSistemas.Any())
             .OrderByDescending(r => r.FechaCreacion)
             .Take(1)
             .SelectMany(r => r.ActivacionSistemas)
            ).ToList();


        //var todaasIntervencionMedios = registroAnterioresDto.SelectMany(r => r.IntervencionMedios).ToList();

        //var todosPlanesEmergencia = registroAnterioresDto.SelectMany(r => r.ActivacionPlanEmergencias).ToList();

        //var todasActivacionPlanesRegionales = _mapper.Map<List<ActivacionPlanEmergenciaDto>>(registros.SelectMany(r => r.ActivacionPlanEmergencias).ToList()
        //                                             .Where(p => p.TipoPlan?.Ambito == 'R'))
        //                                             .Select(dto => { dto.EsEliminable = false; return dto; })
        //                                              .ToList();


        //var ActivacionPlanEmergenciasEstatales = _mapper.Map<List<ActivacionPlanEmergenciaDto>>(registros.SelectMany(r => r.ActivacionPlanEmergencias).ToList()
        //                                             .Where(p => p.TipoPlan?.Ambito == 'E'))
        //                                             .Select(dto => { dto.EsEliminable = false; return dto; })
        //                                              .ToList();


        // Actualizar la propiedad EsEliminable de cada AreaAfectadaDto
        ultimoRegistroConAreasAfectadas.ForEach(area => area.EsEliminable = idsAreaAfectdasEliminables.Contains(area.Id));
        ultimoRegistroConParametros.ForEach(parametro => parametro.EsEliminable = idsParametrosEliminables.Contains(parametro.Id));
        ultimoRegistroConActivacionDeSistemas.ForEach(activacion => activacion.EsEliminable = idsActivacionSistemas.Contains(activacion.Id));

        ultimoRegistroConIntervencionMedios.ForEach(intervencion => intervencion.EsEliminable = idsIntervencionMediosEliminables.Contains(intervencion.Id));
        ultimoRegistroConDirecciones.ForEach(direccion => direccion.EsEliminable = idsDirecciones.Contains(direccion.Id));
        ultimoRegistroConCoordinacionesCecopi.ForEach(coordinacionCecopi => coordinacionCecopi.EsEliminable = idsCecopi.Contains(coordinacionCecopi.Id));
        ultimoRegistroConCoordinacionesPMA.ForEach(coordinacionPMA => coordinacionPMA.EsEliminable = idsPma.Contains(coordinacionPMA.Id));
        ultimoRegistroConActivacionPlanes.ForEach(activacionPlan => activacionPlan.EsEliminable = idsActivacionPlanEmergenciasEliminables.Contains(activacionPlan.Id));
        ultimoRegistroConConsecuenciasActuaciones.ForEach(impacto => { impacto.EsEliminable = idsConsecuenciaActuacionEliminables.Contains(impacto.Id);
                                                        // Sumar solo los 'Numero' de los impactos no borrados
                                                        impacto.Total = impacto.ImpactosEvoluciones
                                                                    .Sum(ie => ie.Numero);});




        var resultadoUnificado = new RegistrosAnterioresDto
        {
            AreaAfectadas = ultimoRegistroConAreasAfectadas,
            Parametros = ultimoRegistroConParametros,
            TipoImpactosEvoluciones = ultimoRegistroConConsecuenciasActuaciones,
            IntervencionMedios = ultimoRegistroConIntervencionMedios,
            Direcciones = ultimoRegistroConDirecciones,
            CoordinacionesCecopi = ultimoRegistroConCoordinacionesCecopi,
            CoordinacionesPMA = ultimoRegistroConCoordinacionesPMA,
            ActivacionPlanEmergencias = ultimoRegistroConActivacionPlanes,
            ActivacionSistemas = ultimoRegistroConActivacionDeSistemas,
            //ActivacionPlanEmergenciasEstatales = ActivacionPlanEmergenciasEstatales,
            //ActivacionPlanEmergenciasRegionales = todasActivacionPlanesRegionales,
        };

        registroAnterioresDto = new List<RegistrosAnterioresDto> { resultadoUnificado };

        _logger.LogInformation($"{nameof(GetRegistrosAnterioresByIdSucesoListQueryHandler)} - END");

        return registroAnterioresDto;

    }

    private bool IsEliminable(EstadoRegistroEnum estado)
    {
        return estado == EstadoRegistroEnum.Creado ||
            estado == EstadoRegistroEnum.CreadoYModificado;
    }
}
