using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.Evoluciones;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Evoluciones;
using DGPCE.Sigemad.Application.Specifications.Registros;
using DGPCE.Sigemad.Application.Specifications.RegistrosActualizaciones;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Evoluciones.Queries.GetEvolucion;
public class GetEvolucionQueryHandler : IRequestHandler<GetEvolucionQuery, EvolucionDto>
{
    private readonly ILogger<GetEvolucionQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetEvolucionQueryHandler(
        ILogger<GetEvolucionQueryHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<EvolucionDto> Handle(GetEvolucionQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(GetEvolucionQueryHandler)} - BEGIN");

        Evolucion evolucion;
        List<int> idsAreaAfectdas = new List<int>();
        List<int> idsConsecuenciaActuacion = new List<int>();
        List<int> idsIntervencionMedios = new List<int>();
        List<int> idsParametro = new List<int>();
        List<int> idsRegistro = new List<int>();
        List<int> idsDatoPrincipal = new List<int>();

        List<int> idsAreaAfectdasEliminables = new List<int>();
        List<int> idsConsecuenciaActuacionEliminables = new List<int>();
        List<int> idsIntervencionMediosEliminables = new List<int>();
        List<int> idsParametroEliminables = new List<int>();

        bool includeRegistro = false;
        bool includeDatoPrincipal = false;

        if (request.IdRegistroActualizacion.HasValue)
        {
            _logger.LogInformation($"Buscando Evolución para IdRegistroActualizacion: {request.IdRegistroActualizacion}");
            // Obtener RegistroActualizacion con IdReferencia (Evolucion.Id)
            var registroSpec = new RegistroActualizacionSpecificationParams
            {
                Id = request.IdRegistroActualizacion.Value
            };
            var registroActualizacion = await _unitOfWork.Repository<RegistroActualizacion>()
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
                    //case (int)ApartadoRegistroEnum.DatoPrincipal:
                    //    includeDatoPrincipal = true;
                    //    idsDatoPrincipal.Add(detalle.IdReferencia);
                    //    break;

                    //case (int)ApartadoRegistroEnum.AreaAfectada:
                    //    idsAreaAfectdas.Add(detalle.IdReferencia);
                    //    if (IsEliminable(detalle.IdEstadoRegistro)) idsAreaAfectdasEliminables.Add(detalle.IdReferencia);
                    //    break;

                    //case (int)ApartadoRegistroEnum.ConsecuenciaActuacion:
                    //    idsConsecuenciaActuacion.Add(detalle.IdReferencia);
                    //    if (IsEliminable(detalle.IdEstadoRegistro)) idsConsecuenciaActuacionEliminables.Add(detalle.IdReferencia);
                    //    break;

                    //case (int)ApartadoRegistroEnum.IntervencionMedios:
                    //    idsIntervencionMedios.Add(detalle.IdReferencia);
                    //    if (IsEliminable(detalle.IdEstadoRegistro)) idsIntervencionMediosEliminables.Add(detalle.IdReferencia);
                    //    break;

                    //case (int)ApartadoRegistroEnum.Parametro:
                    //    idsParametro.Add(detalle.IdReferencia);
                    //    if (IsEliminable(detalle.IdEstadoRegistro)) idsParametroEliminables.Add(detalle.IdReferencia);
                    //    break;
                }
            }

            // Buscar la Evolución por IdReferencia
            evolucion = await _unitOfWork.Repository<Evolucion>()
                .GetByIdWithSpec(new EvolucionWithFilteredDataSpecification(
                    registroActualizacion.IdReferencia,
                    idsRegistro,
                    idsDatoPrincipal,
                    idsParametro,
                    idsAreaAfectdas,
                    idsConsecuenciaActuacion,
                    idsIntervencionMedios,
                    esFoto: false
                    ));
        }
        else
        {
            _logger.LogInformation($"Buscando Evolución para IdSuceso: {request.IdSuceso}");
            // Buscar Evolución por IdSuceso
            evolucion = await _unitOfWork.Repository<Evolucion>()
                .GetByIdWithSpec(new EvolucionSpecification(new RegistroSpecificationParams { IdSuceso = request.IdSuceso }));
        }

        if(evolucion == null)
        {
            _logger.LogWarning($"No se encontró Evolución para IdSuceso: {request.IdSuceso}");
            throw new NotFoundException(nameof(Evolucion), request.IdSuceso);
        }


        //foreach (Registro registro in evolucion.Registros)
        //{
        //    registro.ProcedenciaDestinos = registro.ProcedenciaDestinos.Where(pd => !pd.Borrado).ToList();
        //}



        // Mapear Evolución a EvolucionDto
        var evolucionDto = _mapper.Map<EvolucionDto>(evolucion);
        //Parametro? parametro = evolucion.Parametros.OrderByDescending(p => p.FechaCreacion).FirstOrDefault();
        //evolucionDto.Parametro = _mapper.Map<ParametroEvolucionDto>(parametro);

        //if (request.IdRegistroActualizacion.HasValue && request.IdRegistroActualizacion.Value > 0)
        //{
        //    //Registro? registro = evolucion.Registros.OrderByDescending(r => r.FechaCreacion).FirstOrDefault();
        //    //DatoPrincipal? datoPrincipal = evolucion.DatosPrincipales.OrderByDescending(d => d.FechaCreacion).FirstOrDefault();

        //    //evolucionDto.Registro = _mapper.Map<RegistroEvolucionDto>(registro);
        //    evolucionDto.DatoPrincipal = _mapper.Map<DatoPrincipalEvolucionDto>(datoPrincipal);
        //}

        //evolucionDto.AreaAfectadas.ForEach(area => area.EsEliminable = idsAreaAfectdasEliminables.Contains(area.Id));
        //evolucionDto.Impactos.ForEach(impacto => impacto.EsEliminable = idsConsecuenciaActuacionEliminables.Contains(impacto.Id.Value));
        evolucionDto.IntervencionMedios.ForEach(intervencionMedios => intervencionMedios.EsEliminable = idsIntervencionMediosEliminables.Contains(intervencionMedios.Id));


        _logger.LogInformation($"{nameof(GetEvolucionQueryHandler)} - END");

        return evolucionDto;
    }

    private bool IsEliminable(EstadoRegistroEnum estado)
    {
        return estado == EstadoRegistroEnum.Creado ||
            estado == EstadoRegistroEnum.CreadoYModificado;
    }
}
