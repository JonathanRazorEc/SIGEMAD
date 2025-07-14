using DGPCE.Sigemad.Application.Contracts.Files;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Dtos.Archivos;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Archivos.Queries.GetArchivoById;
public class GetArchivoByIdQueryHandler : IRequestHandler<GetArchivoByIdQuery, ArchivoDownloadDto>
{
    private readonly ILogger<GetArchivoByIdQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileService _fileService;

    public GetArchivoByIdQueryHandler(
        ILogger<GetArchivoByIdQueryHandler> logger,
        IFileService fileService,
        IUnitOfWork unitOfWork
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _fileService = fileService;
    }

    public async Task<ArchivoDownloadDto> Handle(GetArchivoByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(GetArchivoByIdQueryHandler)} - BEGIN");

        var archivo = await _unitOfWork.Repository<Archivo>().GetByIdAsync(request.Id);

        if (archivo == null || archivo.Borrado)
        {
            _logger.LogWarning($"No se encontro archivo con id: {request.Id}");
            throw new NotFoundException(nameof(Archivo), request.Id);
        }

        // Validar que el archivo existe
        var fileStream = await _fileService.GetFileAsync(archivo.RutaDeAlmacenamiento);

        _logger.LogInformation($"{nameof(GetArchivoByIdQueryHandler)} - END");

        return new ArchivoDownloadDto
        {
            FileStream = fileStream,
            ContentType = archivo.Tipo,
            FileName = archivo.NombreOriginal
        };
    }
}
