using DGPCE.Sigemad.Application.Constants;
using DGPCE.Sigemad.Application.Contracts.Files;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Domain.Modelos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DGPCE.Sigemad.Application.Features.Archivos.Commands.CreateFile;
public class CreateFileCommandHandler : IRequestHandler<CreateFileCommand, CreateFileResponse>
{
    private readonly ILogger<CreateFileCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileService _fileService;

    public CreateFileCommandHandler(
        ILogger<CreateFileCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IFileService fileService
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _fileService = fileService;
    }

    public async Task<CreateFileResponse> Handle(CreateFileCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(CreateFileCommand)} - BEGIN");

        var id = Guid.NewGuid();
        var fileEntity = new Archivo
        {
            Id = id,
            NombreOriginal = request.NombreOriginal,
            NombreUnico = $"{id}{request.Extension}",
            Tipo = request.Tipo,
            Extension = request.Extension,
            PesoEnBytes = request.PesoEnBytes
        };

        fileEntity.RutaDeAlmacenamiento =
            await _fileService.SaveFileAsync(request.Archivo, fileEntity.NombreUnico, GetContextFile(request.Context));

        fileEntity.FechaCreacion = DateTime.Now;

        _unitOfWork.Repository<Archivo>().AddEntity(fileEntity);

        var result = await _unitOfWork.Complete();
        if (result <= 0)
        {
            throw new Exception("No se pudo registrar datos de incendio");
        }

        _logger.LogInformation($"Se registro datos de archivos con un {fileEntity.Id}");

        _logger.LogInformation($"{nameof(CreateFileCommand)} - END");

        return new CreateFileResponse { Id = fileEntity.Id };
    }

    private string GetContextFile(ContextFile contextFile)
    {
        return contextFile switch
        {
            ContextFile.PlanEmergencia => "plan-emergencia",
            ContextFile.Documentacion => "documentacion"
        };
    }
}
