using DGPCE.Sigemad.Application.Contracts.Files;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.Archivos.Queries.GetArchivoById;
using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.Extensions.Logging;
using Moq;

namespace DGPCE.Sigemad.Application.Tests.Features.Archivos.Queries;

public class GetArchivoByIdQueryHandlerTests
{
    private readonly Mock<ILogger<GetArchivoByIdQueryHandler>> _loggerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IFileService> _fileServiceMock;
    private readonly GetArchivoByIdQueryHandler _handler;

    public GetArchivoByIdQueryHandlerTests()
    {
        _loggerMock = new Mock<ILogger<GetArchivoByIdQueryHandler>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _fileServiceMock = new Mock<IFileService>();
        _handler = new GetArchivoByIdQueryHandler(_loggerMock.Object, _fileServiceMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ArchivoExists_ReturnsArchivoDownloadDto()
    {
        // Arrange
        var archivoId = Guid.NewGuid();
        var archivo = new Archivo
        {
            Id = archivoId,
            NombreOriginal = "test.pdf",
            Tipo = "application/pdf",
            RutaDeAlmacenamiento = "path/to/test.pdf",
            Borrado = false
        };

        var fileStream = new MemoryStream();

        _unitOfWorkMock.Setup(uow => uow.Repository<Archivo>().GetByIdAsync(archivoId))
            .ReturnsAsync(archivo);
        _fileServiceMock.Setup(fs => fs.GetFileAsync(archivo.RutaDeAlmacenamiento))
            .ReturnsAsync(fileStream);

        var query = new GetArchivoByIdQuery(archivoId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(archivo.NombreOriginal, result.FileName);
        Assert.Equal(archivo.Tipo, result.ContentType);
        Assert.Equal(fileStream, result.FileStream);
    }

    [Fact]
    public async Task Handle_ArchivoNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var archivoId = Guid.NewGuid();

        _unitOfWorkMock.Setup(uow => uow.Repository<Archivo>().GetByIdAsync(archivoId))
            .ReturnsAsync((Archivo)null);

        var query = new GetArchivoByIdQuery(archivoId);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ArchivoBorrado_ThrowsNotFoundException()
    {
        // Arrange
        var archivoId = Guid.NewGuid();
        var archivo = new Archivo
        {
            Id = archivoId,
            Borrado = true
        };

        _unitOfWorkMock.Setup(uow => uow.Repository<Archivo>().GetByIdAsync(archivoId))
            .ReturnsAsync(archivo);

        var query = new GetArchivoByIdQuery(archivoId);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_FileServiceThrowsException_ThrowsException()
    {
        // Arrange
        var archivoId = Guid.NewGuid();
        var archivo = new Archivo
        {
            Id = archivoId,
            NombreOriginal = "test.pdf",
            Tipo = "application/pdf",
            RutaDeAlmacenamiento = "path/to/test.pdf",
            Borrado = false
        };

        _unitOfWorkMock.Setup(uow => uow.Repository<Archivo>().GetByIdAsync(archivoId))
            .ReturnsAsync(archivo);
        _fileServiceMock.Setup(fs => fs.GetFileAsync(archivo.RutaDeAlmacenamiento))
            .ThrowsAsync(new Exception("File not found"));

        var query = new GetArchivoByIdQuery(archivoId);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(query, CancellationToken.None));
    }
}
