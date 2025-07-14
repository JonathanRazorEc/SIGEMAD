using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.Documentaciones;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Specifications.Documentos;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Documentaciones.Queries.GetDetalleDocumentacionesById;

public class GetDocumentacionesByIdQueryHandler : IRequestHandler<GetDocumentacionesByIdQuery, DocumentacionDto>
{
    private readonly ILogger<GetDocumentacionesByIdQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetDocumentacionesByIdQueryHandler(
        ILogger<GetDocumentacionesByIdQueryHandler> logger,
        IUnitOfWork unitOfWork, IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DocumentacionDto> Handle(GetDocumentacionesByIdQuery request, CancellationToken cancellationToken)
    {
        var documentacion = await _unitOfWork.Repository<Documentacion>()
        .GetByIdWithSpec(new DetalleDocumentacionById(new DocumentacionParams{ Id = request.Id }));

        if (documentacion == null)
        {
            _logger.LogWarning($"No se encontro documentación con id: {request.Id}");
            throw new NotFoundException(nameof(Documentacion), request.Id);
        }

        var documentacionDto = _mapper.Map<Documentacion, DocumentacionDto>(documentacion);
        return documentacionDto;
    }
}