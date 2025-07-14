using AutoMapper;
using DGPCE.Sigemad.Application.Contracts.Files;
using DGPCE.Sigemad.Application.Contracts.Persistence;
using DGPCE.Sigemad.Application.Contracts.RegistrosActualizacion;
using DGPCE.Sigemad.Application.Dtos.Common;
using DGPCE.Sigemad.Application.Dtos.Documentaciones;
using DGPCE.Sigemad.Application.Exceptions;
using DGPCE.Sigemad.Application.Features.Documentaciones.Commands.ManageDocumentaciones;
using DGPCE.Sigemad.Application.Specifications;
using DGPCE.Sigemad.Domain.Modelos;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace DGPCE.Sigemad.Application.Tests.Features.Documentaciones.Commands;

public class ManageDocumentacionesCommandHandlerTests
{
    private readonly Mock<ILogger<ManageDocumentacionesCommandHandler>> _loggerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IFileService> _fileServiceMock;
    private readonly ManageDocumentacionesCommandHandler _handler;
    private readonly Mock<IRegistroActualizacionService> _registroActualizacionMock;

    public ManageDocumentacionesCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<ManageDocumentacionesCommandHandler>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _fileServiceMock = new Mock<IFileService>();
        _registroActualizacionMock = new Mock<IRegistroActualizacionService>();

        _handler = new ManageDocumentacionesCommandHandler(_loggerMock.Object, _unitOfWorkMock.Object, _mapperMock.Object, _fileServiceMock.Object, _registroActualizacionMock.Object);
    }

    [Fact]
    public async Task Handle_DocumentacionExists_UpdatesDocumentacion()
    {
        // Arrange
        var documentacionId = 1;
        var documentacion = new Documentacion { Id = documentacionId, Borrado = false };
        var command = new ManageDocumentacionesCommand
        {
            IdRegistroActualizacion = documentacionId,
            IdSuceso = 1,
            DetallesDocumentaciones = new List<DetalleDocumentacionDto>
            {
                new DetalleDocumentacionDto
                {
                    Id = 1,
                    FechaHora = DateTime.Now,
                    FechaHoraSolicitud = DateTime.Now,
                    IdTipoDocumento = 1,
                    Descripcion = "Test",
                    Archivo = new FileDto
                    {
                        FileName = "test.pdf",
                        ContentType = "application/pdf",
                        Extension = ".pdf",
                        Length = 1024,
                        Content = new byte[1024]
                    }
                }
            }
        };

        _unitOfWorkMock.Setup(uow => uow.Repository<Documentacion>().GetByIdWithSpec(It.IsAny<ISpecification<Documentacion>>()))
            .ReturnsAsync(documentacion);
        _unitOfWorkMock.Setup(uow => uow.Repository<TipoDocumento>().GetAsync(It.IsAny<Expression<Func<TipoDocumento, bool>>>()))
            .ReturnsAsync(new List<TipoDocumento> { new TipoDocumento { Id = 1 } });
        _unitOfWorkMock.Setup(uow => uow.Repository<ProcedenciaDestino>().GetAsync(It.IsAny<Expression<Func<ProcedenciaDestino, bool>>>()))
            .ReturnsAsync(new List<ProcedenciaDestino> { new ProcedenciaDestino { Id = 1 } });
        _unitOfWorkMock.Setup(uow => uow.Complete()).ReturnsAsync(1);

        _fileServiceMock.Setup(fs => fs.SaveFileAsync(It.IsAny<byte[]>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync("path/to/file");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(documentacionId, result.IdDocumentacion);
        _unitOfWorkMock.Verify(uow => uow.Repository<Documentacion>().UpdateEntity(documentacion), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.Complete(), Times.Once);
    }

    [Fact]
    public async Task Handle_DocumentacionNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var documentacionId = 1;
        var command = new ManageDocumentacionesCommand
        {
            IdRegistroActualizacion = documentacionId,
            IdSuceso = 1
        };

        _unitOfWorkMock.Setup(uow => uow.Repository<Documentacion>().GetByIdWithSpec(It.IsAny<ISpecification<Documentacion>>()))
            .ReturnsAsync((Documentacion)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_SucesoNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var command = new ManageDocumentacionesCommand
        {
            IdSuceso = 1
        };

        _unitOfWorkMock.Setup(uow => uow.Repository<Suceso>().GetByIdAsync(command.IdSuceso))
            .ReturnsAsync((Suceso)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ValidRequest_CreatesDocumentacion()
    {
        // Arrange
        var command = new ManageDocumentacionesCommand
        {
            IdSuceso = 1,
            DetallesDocumentaciones = new List<DetalleDocumentacionDto>
            {
                new DetalleDocumentacionDto
                {
                    FechaHora = DateTime.Now,
                    FechaHoraSolicitud = DateTime.Now,
                    IdTipoDocumento = 1,
                    Descripcion = "Test",
                    Archivo = new FileDto
                    {
                        FileName = "test.pdf",
                        ContentType = "application/pdf",
                        Extension = ".pdf",
                        Length = 1024,
                        Content = new byte[1024]
                    }
                }
            }
        };

        _unitOfWorkMock.Setup(uow => uow.Repository<Suceso>().GetByIdAsync(command.IdSuceso))
            .ReturnsAsync(new Suceso { Id = command.IdSuceso });
        _unitOfWorkMock.Setup(uow => uow.Repository<TipoDocumento>().GetAsync(It.IsAny<Expression<Func<TipoDocumento, bool>>>()))
            .ReturnsAsync(new List<TipoDocumento> { new TipoDocumento { Id = 1 } });
        _unitOfWorkMock.Setup(uow => uow.Repository<ProcedenciaDestino>().GetAsync(It.IsAny<Expression<Func<ProcedenciaDestino, bool>>>()))
            .ReturnsAsync(new List<ProcedenciaDestino> { new ProcedenciaDestino { Id = 1 } });
        _unitOfWorkMock.Setup(uow => uow.Repository<Documentacion>().AddEntity(It.IsAny<Documentacion>()));
        _unitOfWorkMock.Setup(uow => uow.Complete()).ReturnsAsync(1);

        _fileServiceMock.Setup(fs => fs.SaveFileAsync(It.IsAny<byte[]>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync("path/to/file");
        

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        _unitOfWorkMock.Verify(uow => uow.Repository<Documentacion>().AddEntity(It.IsAny<Documentacion>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.Complete(), Times.Once);
    }

    [Fact]
    public async Task Handle_CompleteFails_ThrowsException()
    {
        // Arrange
        var command = new ManageDocumentacionesCommand
        {
            IdSuceso = 1,
            DetallesDocumentaciones = new List<DetalleDocumentacionDto>
            {
                new DetalleDocumentacionDto
                {
                    FechaHora = DateTime.Now,
                    FechaHoraSolicitud = DateTime.Now,
                    IdTipoDocumento = 1,
                    Descripcion = "Test",
                    Archivo = new FileDto
                    {
                        FileName = "test.pdf",
                        ContentType = "application/pdf",
                        Extension = ".pdf",
                        Length = 1024,
                        Content = new byte[1024]
                    }
                }
            }
        };

        _unitOfWorkMock.Setup(uow => uow.Repository<Suceso>().GetByIdAsync(command.IdSuceso))
            .ReturnsAsync(new Suceso { Id = command.IdSuceso });
        _unitOfWorkMock.Setup(uow => uow.Repository<TipoDocumento>().GetAsync(It.IsAny<Expression<Func<TipoDocumento, bool>>>()))
            .ReturnsAsync(new List<TipoDocumento> { new TipoDocumento { Id = 1 } });
        _unitOfWorkMock.Setup(uow => uow.Repository<ProcedenciaDestino>().GetAsync(It.IsAny<Expression<Func<ProcedenciaDestino, bool>>>()))
            .ReturnsAsync(new List<ProcedenciaDestino> { new ProcedenciaDestino { Id = 1 } });
        _fileServiceMock.Setup(fs => fs.SaveFileAsync(It.IsAny<byte[]>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync("path/to/file");

        _unitOfWorkMock.Setup(uow => uow.Repository<Documentacion>().AddEntity(It.IsAny<Documentacion>()));
        _unitOfWorkMock.Setup(uow => uow.Complete()).ReturnsAsync(0);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
    }
}
