using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.Documentaciones;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Documentos;
using DGPCE.Sigemad.Application.Specifications.RegistrosActualizaciones;
using DGPCE.Sigemad.Domain.Enums;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DGPCE.Sigemad.Application.Features.Documentaciones.Queries.GetDocumentacion;
public class GetDoumentacionQueryHandler : IRequestHandler<GetDoumentacionQuery, DocumentacionDto>
{
    private readonly ILogger<GetDoumentacionQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetDoumentacionQueryHandler(
        ILogger<GetDoumentacionQueryHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DocumentacionDto> Handle(GetDoumentacionQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(GetDoumentacionQueryHandler)} - BEGIN");

        Documentacion documentacion;
        List<int> idsReferencias = new List<int>();
        List<int> idsDetallesDocumentaciones = new List<int>();
        List<int> idsDetallesDocumentacionesEliminables = new List<int>();

        if (request.IdRegistroActualizacion.HasValue)
        {
            _logger.LogInformation($"Buscando Documentacion para IdRegistroActualizacion: {request.IdRegistroActualizacion}");

            // Obtener RegistroActualizacion con IdReferencia (documentacion.Id)
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

            // Separar IdReferencia según su tipo
            foreach (var detalle in registroActualizacion.DetallesRegistro)
            {
                switch (detalle.IdApartadoRegistro)
                {
                    case (int)ApartadoRegistroEnum.Documentacion:
                        idsDetallesDocumentaciones.Add(detalle.IdReferencia);
                        if (IsEliminable(detalle.IdEstadoRegistro)) idsDetallesDocumentaciones.Add(detalle.IdReferencia);
                        break;
                }
            }

            // Buscar la Documentacion por IdReferencia
            documentacion = await _unitOfWork.Repository<Documentacion>()
                .GetByIdWithSpec(new DocumentacionWithFilteredData(registroActualizacion.IdReferencia, idsDetallesDocumentaciones));
        }
        else
        {
            _logger.LogInformation($"Buscando Dirección y Coordinación de Emergencia para IdSuceso: {request.IdSuceso}");

            // Buscar Documentacion por IdSuceso
            documentacion = await _unitOfWork.Repository<Documentacion>()
                .GetByIdWithSpec(new DetalleDocumentacionById(new DocumentacionParams { IdSuceso = request.IdSuceso }));
        }

        if (documentacion == null)
        {
            _logger.LogWarning($"No se encontró documentacion para Suceso: {request.IdSuceso}");
            throw new NotFoundException(nameof(Documentacion), request.IdSuceso);
        }

        // Mapear y devolver DTO con los datos completos
        var response = _mapper.Map<DocumentacionDto>(documentacion);
        response.Detalles.ForEach(d => d.EsEliminable = idsDetallesDocumentaciones.Contains(d.Id));

        _logger.LogInformation($"{nameof(GetDoumentacionQueryHandler)} - END");
        return response;

    }

    private bool IsEliminable(EstadoRegistroEnum estado)
    {
        return estado == EstadoRegistroEnum.Creado ||
            estado == EstadoRegistroEnum.CreadoYModificado;
    }
}