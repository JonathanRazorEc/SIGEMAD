using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.OtraInformaciones;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.OtrasInformaciones;
using DGPCE.Sigemad.Application.Specifications.RegistrosActualizaciones;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DGPCE.Sigemad.Application.Features.OtrasInformaciones.Queries.GetOtraInformacion;
public class GetOtraInformacionQueryHandler : IRequestHandler<GetOtraInformacionQuery, OtraInformacionDto>
{
    private readonly ILogger<GetOtraInformacionQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetOtraInformacionQueryHandler(
        ILogger<GetOtraInformacionQueryHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<OtraInformacionDto> Handle(GetOtraInformacionQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(GetOtraInformacionQueryHandler)} - BEGIN");

        OtraInformacion otraInformacion;
        List<int> idsReferencias = new List<int>();
        List<int> idsDetallesOtraInformacion = new List<int>();
        List<int> idsDetallesOtraInformacionEliminables = new List<int>();

        if (request.IdRegistroActualizacion.HasValue)
        {
            _logger.LogInformation($"Buscando OtraInformacion para IdRegistroActualizacion: {request.IdRegistroActualizacion}");

            // Obtener RegistroActualizacion con IdReferencia (documentacion.Id)
            var registroSpec = new RegistroActualizacionSpecificationParams
            {
                Id = request.IdRegistroActualizacion.Value
            };
            var registroActualizacion = await _unitOfWork.Repository<RegistroActualizacion>()
                .GetByIdWithSpec(new RegistroActualizacionSpecification(registroSpec));

            if (registroActualizacion == null)
            {
                _logger.LogWarning($"No se encontró OtraInformacion con Id: {request.IdRegistroActualizacion}");
                throw new NotFoundException(nameof(RegistroActualizacion), request.IdRegistroActualizacion);
            }

            // Separar IdReferencia según su tipo
            foreach (var detalle in registroActualizacion.DetallesRegistro)
            {
                switch (detalle.IdApartadoRegistro)
                {
                    case (int)ApartadoRegistroEnum.OtraInformacion:
                        idsDetallesOtraInformacion.Add(detalle.IdReferencia);
                        if (IsEliminable(detalle.IdEstadoRegistro)) idsDetallesOtraInformacion.Add(detalle.IdReferencia);
                        break;
                }
            }

            // Buscar la Documentacion por IdReferencia
            otraInformacion = await _unitOfWork.Repository<OtraInformacion>()
                .GetByIdWithSpec(new OtraInformacionWithDetailsAndProcedenciasSpecification(registroActualizacion.IdReferencia, idsDetallesOtraInformacion));
        }
        else
        {
            _logger.LogInformation($"Buscando OtraInformacion para IdSuceso: {request.IdSuceso}");

            // Buscar Documentacion por IdSuceso
            otraInformacion = await _unitOfWork.Repository<OtraInformacion>()
                .GetByIdWithSpec(new GetOtraInformacionWithParams(new OtraInformacionParams { IdSuceso = request.IdSuceso }));
        }

        if (otraInformacion == null)
        {
            _logger.LogWarning($"No se encontró OtraInformacion para Suceso: {request.IdSuceso}");
            throw new NotFoundException(nameof(OtraInformacion), request.IdSuceso);
        }

        // Mapear y devolver DTO con los datos completos
        var response = _mapper.Map<OtraInformacionDto>(otraInformacion);
        response.Lista.ForEach(d => d.EsEliminable = idsDetallesOtraInformacion.Contains(d.Id));

        _logger.LogInformation($"{nameof(GetOtraInformacionQueryHandler)} - END");
        return response;
    }


    private bool IsEliminable(EstadoRegistroEnum estado)
    {
        return estado == EstadoRegistroEnum.Creado ||
            estado == EstadoRegistroEnum.CreadoYModificado;
    }
}
